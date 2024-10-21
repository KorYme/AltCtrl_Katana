using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    private bool _isSwordTransitionCancelled;

    private void Start()
    {
        InstanceManager.UIManager.OnPlayerReadyStateUpdateRequest += OnPlayerReadyStateUpdate;
        InstanceManager.UIManager.OnTransitionRequest += OnBothPlayersReadyUpdate;
        InstanceManager.UIManager.OnTransitionCancelRequest += OnPlayersReadyTransitionCancelRequest;
    }

    private void OnPlayerReadyStateUpdate(int ind, bool isReady) => UIUpdatePlayerReadyState(ind, isReady);
    private void OnBothPlayersReadyUpdate() => UIStartSwordTransition();

    private void OnPlayersReadyTransitionCancelRequest() => UICancelSwordTransition();
    private void UIUpdatePlayerReadyState(int ind, bool isReady)
    {
        UISwordList[ind].color = isReady ? Color.white : Color.black; // placeholder
    }

    private void UIStartSwordTransition()
    {

        // uhhhhhhh


        //if (_swordCancelLerpCoroutine != default)
        //{
        //    StopCoroutine(_swordCancelLerpCoroutine);
        //    _swordCancelLerpCoroutine = default;
        //}
        //else if (_swordLerpCoroutine == default) _swordLerpCoroutine = StartCoroutine(SwordLerpIntro(0f, false));

    }

    private void UICancelSwordTransition()
    {
        // UHHHHHHHH

        //if(!_isSwordTransitionCancelled && _swordLerpCoroutine == null) return;
        //_isSwordTransitionCancelled = true;
    }
    private IEnumerator SwordLerpIntro(float time, bool isCancelled)
    {
        float currentTime = time;

        Vector3 p1SwordPos = UISwordList[0].rectTransform.position;
        Vector3 p2SwordPos = UISwordList[1].rectTransform.position;

        Vector3 p1LerpVect;
        Vector3 p2LerpVect;


        while (currentTime < _transitionDuration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / _transitionDuration;
            p1LerpVect = Vector3.Lerp(p1SwordPos, UITargetPosList[0].position, t);
            p2LerpVect = Vector3.Lerp(p2SwordPos, UITargetPosList[1].position, t);

            UISwordList[0].rectTransform.position = p1LerpVect;
            UISwordList[1].rectTransform.position = p2LerpVect;
            if (_isSwordTransitionCancelled)
            {
                if (isCancelled)
                    // should call canceltrnasition instead
                    _swordCancelLerpCoroutine = StartCoroutine(SwordLerpCancel());
                yield break;
            }
            yield return null;
        }

        UISwordList[0].rectTransform.position = UITargetPosList[0].position;
        UISwordList[1].rectTransform.position = UITargetPosList[1].position;
        InstanceManager.UIManager.OnSwordTransitionComplete?.Invoke();
    }
    private IEnumerator SwordLerpCancel()
    {
        float currentTime = 0.0f;

        Vector3 p1SwordPos = UISwordList[0].rectTransform.position;
        Vector3 p2SwordPos = UISwordList[1].rectTransform.position;

        Vector3 p1LerpVect;
        Vector3 p2LerpVect;


        while (currentTime < _transitionDuration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / _transitionDuration;
            p1LerpVect = Vector3.Lerp(p1SwordPos, UITargetPosList[0].position, t);
            p2LerpVect = Vector3.Lerp(p2SwordPos, UITargetPosList[1].position, t);

            UISwordList[0].rectTransform.position = p1LerpVect;
            UISwordList[1].rectTransform.position = p2LerpVect;
            //if ()
            //{
            //    _swordCancelLerpCoroutine = StartCoroutine();
            //    yield break;
            //}
            yield return null;
        }

        UISwordList[0].rectTransform.position = UITargetPosList[0].position;
        UISwordList[1].rectTransform.position = UITargetPosList[1].position;
        InstanceManager.UIManager.OnSwordTransitionComplete?.Invoke();
    }

}
