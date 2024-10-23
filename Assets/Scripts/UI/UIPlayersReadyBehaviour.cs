using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayersReadyBehaviour : MonoBehaviour
{
    [Header("Transition Parameters")]
    [SerializeField] private float _transitionDuration;
    [SerializeField] private AnimationCurve _transitionEaseEffect;

    [Header("Componnents")]
    [SerializeField] List<Image> UISwordList = new List<Image>();
    private List<Vector3> UISwordPosList = new List<Vector3>();
    [SerializeField] List<RectTransform> UITargetPosList = new List<RectTransform>();


    private float _currentElapsedTime;
    private bool _isTransitionning;
    private bool _isTransitionCanceled;

    private Vector3 _p1StartPos;
    private Vector3 _p2StartPos;
    private Vector3 _p1TargetPos;
    private Vector3 _p2TargetPos;

    private void Start()
    {
        for (int i = 0; i < UISwordList.Count; i++) UISwordPosList.Add(UISwordList[i].rectTransform.position);

        InstanceManager.UIManager.OnPlayerReadyStateUpdateRequest += OnPlayerReadyStateUpdate;
        InstanceManager.UIManager.OnTransitionRequest += OnTransitionRequest;
        InstanceManager.UIManager.OnTransitionCancelRequest += OnTransitionCancelRequest;

    }
    private void OnDestroy()
    {
        InstanceManager.UIManager.OnPlayerReadyStateUpdateRequest -= OnPlayerReadyStateUpdate;
        InstanceManager.UIManager.OnTransitionRequest -= OnTransitionRequest;
        InstanceManager.UIManager.OnTransitionCancelRequest -= OnTransitionCancelRequest;

    }

    private void Update()
    {
        UIProcessTransition();
    }


    private void OnPlayerReadyStateUpdate(int ind, bool isReady) => UIUpdatePlayerReadyState(ind, isReady);
    private void UIUpdatePlayerReadyState(int ind, bool isReady)
    {
        Color oldColor = UISwordList[ind].color;
        UISwordList[ind].color = isReady ? new Color(oldColor.r, oldColor.g, oldColor.b, 1) : new Color(oldColor.r, oldColor.g, oldColor.b, 0.25f); // placeholder, CHANGE LATER
    }
    private void OnTransitionRequest()
    {
        _isTransitionning = true;
        _isTransitionCanceled = false;
        UpdateTransitionDirection();
    }

    private void OnTransitionCancelRequest()
    {
        _isTransitionCanceled = true;
        UpdateTransitionDirection();
    }

    private void UpdateTransitionDirection()
    {
        _p1StartPos = !_isTransitionCanceled ? UISwordPosList[0] : UITargetPosList[0].position;
        _p2StartPos = !_isTransitionCanceled ? UISwordPosList[1] : UITargetPosList[1].position;

        _p1TargetPos = !_isTransitionCanceled ? UITargetPosList[0].position : UISwordPosList[0];
        _p2TargetPos = !_isTransitionCanceled ? UITargetPosList[1].position : UISwordPosList[1];

    }

    private void UIProcessTransition()
    {
        if (!_isTransitionning) return;

        _currentElapsedTime += _isTransitionCanceled ? -Time.fixedDeltaTime : Time.fixedDeltaTime;
        _currentElapsedTime = Mathf.Clamp(_currentElapsedTime, 0, _transitionDuration);
        float t = _isTransitionCanceled ? 1 - (_currentElapsedTime / _transitionDuration) : _currentElapsedTime / _transitionDuration;
        t = _transitionEaseEffect.Evaluate(t);

        // startPos and targetPos are switched when the transition is cancelled
        Vector3 p1LerpVect = Vector3.Lerp(_p1StartPos, _p1TargetPos, t);
        Vector3 p2LerpVect = Vector3.Lerp(_p2StartPos, _p2TargetPos, t);
        UISwordList[0].rectTransform.position = p1LerpVect;
        UISwordList[1].rectTransform.position = p2LerpVect;

        if (_currentElapsedTime >= _transitionDuration)
        {
            _isTransitionning = false;
            Invoke("CompleteTransition", 2f);
        }

    }

    private void CompleteTransition()
    {
        InstanceManager.UIManager.OnTransitionComplete?.Invoke();
        InstanceManager.GameManager.LoadGamemodeScene(0);

    }
}
