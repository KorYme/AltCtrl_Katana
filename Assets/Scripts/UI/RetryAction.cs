using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryAction : MonoBehaviour
{
    [SerializeField] float _returnToMenuLag;

    private void OnEnable()
    {
        InstanceManager.UIManager.OnDuelFinished += StartGameOptions;
    }

    private void StartGameOptions() => StartCoroutine(QueueReturnToMenu());

    private IEnumerator QueueReturnToMenu()
    {
        InstanceManager.UIManager.OnShowRetryActionRequest?.Invoke(true);
        yield return new WaitForSeconds(_returnToMenuLag);
        InstanceManager.UIManager.OnReturnToMenuRequest?.Invoke();
    }
}
