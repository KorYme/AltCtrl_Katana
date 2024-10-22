using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayersReadyBehaviour : MonoBehaviour
{
    [Header("Transition Parameters")]
    [SerializeField] private float _transitionDuration;

    [Header("Componnents")]
    [SerializeField] List<Image> UISwordList = new List<Image>();
    [SerializeField] List<RectTransform> UITargetPosList = new List<RectTransform>();

    private Coroutine _swordLerpCoroutine = default;
    private Coroutine _swordCancelLerpCoroutine = default;
    private bool _isTransitionCancelRequested;
    private bool _isTransitionReactivated;
    private void Start()
    {
        InstanceManager.UIManager.OnPlayerReadyStateUpdateRequest += OnPlayerReadyStateUpdate;
        InstanceManager.UIManager.OnTransitionRequest += OnBothPlayersReadyUpdate;
        InstanceManager.UIManager.OnTransitionCancelRequest += OnPlayersReadyTransitionCancelRequest;


    }

    private void OnPlayerReadyStateUpdate(int ind, bool isReady) => UIUpdatePlayerReadyState(ind, isReady);
    private void OnBothPlayersReadyUpdate() => UISwordTransition(0f, false);

    private void OnPlayersReadyTransitionCancelRequest() => UICancelSwordTransition();
    private void UIUpdatePlayerReadyState(int ind, bool isReady)
    {
        Color oldColor = UISwordList[ind].color;
        UISwordList[ind].color = isReady ? new Color(oldColor.r, oldColor.g, oldColor.b, 255) : new Color(oldColor.r, oldColor.g, oldColor.b, 50); // placeholder, CHANGE LATER
    }

    private void UISwordTransition(float time, bool isCancelled)
    {
        if (isCancelled) Debug.Log("Transition CANCEL Requested");
        else Debug.Log("Transition Requested");

        if (!isCancelled) // Normal transition
        {
            if (_swordLerpCoroutine != default) return; // return if transition is already active
            if (_swordCancelLerpCoroutine != default) // if reactivating transition while cancelling (player is ready again)
            {
                // stop the transition cancel (cancel the cancel :pou:)
                _isTransitionReactivated = true;
                _swordCancelLerpCoroutine = default;
                return;
            }
            else _swordLerpCoroutine = StartCoroutine(SwordLerpTransition(time, false)); // start the transition from where it was
        }
        else
        {
            if (_swordCancelLerpCoroutine != default) return; // return if cancelling is already active
            if (_swordLerpCoroutine == default) return; // return if no transition is active
            StopCoroutine(_swordLerpCoroutine);
            _swordLerpCoroutine = default;
            _swordCancelLerpCoroutine = StartCoroutine(SwordLerpTransition(time, true));
        }

    }
    private void UICancelSwordTransition()
    {
        if (_isTransitionCancelRequested || _swordLerpCoroutine == default) return;
        _isTransitionCancelRequested = true;
    }

    private IEnumerator SwordLerpTransition(float time, bool isCancelled)
    {
        float currentTime = time;

        // startPos and targetPos are switched when the transition is cancelled
        Vector3 p1Start = !isCancelled ? UISwordList[0].rectTransform.position : UITargetPosList[0].position;
        Vector3 p2Start = !isCancelled ? UISwordList[1].rectTransform.position : UITargetPosList[1].position;

        Vector3 p1Target = !isCancelled ? UITargetPosList[0].position : UISwordList[0].rectTransform.position;
        Vector3 p2Target = !isCancelled ? UITargetPosList[1].position : UISwordList[1].rectTransform.position;

        while (currentTime < _transitionDuration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / _transitionDuration;

            Vector3 p1LerpVect = Vector3.Lerp(p1Start, p1Target, t);
            Vector3 p2LerpVect = Vector3.Lerp(p2Start, p2Target, t);
            UISwordList[0].rectTransform.position = p1LerpVect;
            UISwordList[1].rectTransform.position = p2LerpVect;

            if (_isTransitionCancelRequested && !isCancelled)
            {
                UISwordTransition(currentTime, isCancelled);
                yield break;
            }
            if (_isTransitionReactivated && isCancelled)
            {
                UISwordTransition(currentTime, isCancelled);
                yield break;
            }
            yield return null;
        }

        UISwordList[0].rectTransform.position = p1Target;
        UISwordList[1].rectTransform.position = p2Target;

        InstanceManager.UIManager.OnTransitionComplete?.Invoke();
    }

}
