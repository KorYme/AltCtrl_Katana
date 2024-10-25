using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetryAction : MonoBehaviour
{
    [Header("Componnents")]
    [SerializeField] private RectTransform _retryRect;
    private Vector3 _retryPos;
    [SerializeField] private Vector3 _retryOffset;
    [SerializeField] private Image _retryTime;

    [Header("Parameters")]
    [SerializeField] private float _returnToMenuLag;
    [SerializeField] private float _translationDuration;
    [SerializeField] private AnimationCurve _translationEaseEffect;

    private Coroutine _returnCoroutine;
    private Coroutine _UIReturnCoroutine;

    private void Awake()
    {
        _retryPos = _retryRect.position;
    }

    private void Start()
    {
        _retryRect.gameObject.SetActive(false);
        //_retryRect.transform.position = _retryPos + _retryOffset;
        InstanceManager.UIManager.OnDisplayWinner += StartGameOptions;

    }

    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDisplayWinner -= StartGameOptions;
        InstanceManager.JoyconManager.OnPlayersBow -= Retry;
    }

    private void StartGameOptions(RoundResult nah)
    {
        _retryRect.gameObject.SetActive(true);
        _returnCoroutine = StartCoroutine(QueueReturnToMenu());

    }

    private void Retry()
    {
        InstanceManager.JoyconManager.OnPlayersBow -= Retry;
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
        InstanceManager.JoyconManager.OnPlayersBow += Retry;
        InstanceManager.JoyconManager.ResetPlayersState();
        InstanceManager.AudioManager.StopAllClips();
        InstanceManager.AudioManager.PlayClip("RoundEnd");
        InstanceManager.UIManager.OnShowRetryActionRequest?.Invoke(true);

        //_UIReturnCoroutine = StartCoroutine(UIShowGameOptions()); // ANIM
        float elapsedT = 0;
        while (elapsedT < _returnToMenuLag)
        {
            float t = elapsedT / _returnToMenuLag;
            t = _translationEaseEffect.Evaluate(t);
            _retryTime.fillAmount = Mathf.Lerp(1,0, t);
            elapsedT += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Returning to menu");
        _retryRect.gameObject.SetActive(false);
        InstanceManager.UIManager.OnReturnToMenuRequest?.Invoke();
    }

    private IEnumerator UIShowGameOptions()
    {
        Debug.Log($"Positions : {_retryRect.position}, {_retryOffset}");

        _retryRect.gameObject.SetActive(true);
        float elapsedTime = 0;
        while (elapsedTime < _translationDuration)
        {
            float t = elapsedTime / _translationDuration;
            t = _translationEaseEffect.Evaluate(t);
            _retryRect.position = Vector3.Lerp(_retryPos + _retryOffset, _retryPos, t);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        _retryRect.transform.position = _retryPos;
        Debug.Log($"Positions : {_retryRect.position}, {_retryOffset}");

    }
}
