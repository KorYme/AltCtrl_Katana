using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryAction : MonoBehaviour
{
    [SerializeField] float _returnToMenuLag;

    private Coroutine _returnCoroutine;
    private void OnEnable()
    {
    }

    private void Start()
    {
                InstanceManager.UIManager.OnDuelFinished += StartGameOptions;
        InstanceManager.JoyconManager.OnPlayersBow += Retry;

    }
    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDuelFinished -= StartGameOptions;
        InstanceManager.JoyconManager.OnPlayersBow -= Retry;

    }

    private void StartGameOptions(RoundResult nah) => _returnCoroutine = StartCoroutine(QueueReturnToMenu());

    private void Retry()
    {
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }
        Debug.Log("RETRYING");

        InstanceManager.GameManager.LoadGamemodeScene(0);
    }
    private IEnumerator QueueReturnToMenu()
    {
        InstanceManager.AudioManager.StopAllClips();
        InstanceManager.AudioManager.PlayClip("RoundEnd");
        InstanceManager.UIManager.OnShowRetryActionRequest?.Invoke(true);
        yield return new WaitForSeconds(_returnToMenuLag);
        Debug.Log("Returning to menu");
        InstanceManager.UIManager.OnReturnToMenuRequest?.Invoke();
    }
}
