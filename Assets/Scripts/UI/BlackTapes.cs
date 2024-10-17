using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BlackTapes : MonoBehaviour
{
    [Serializable]
    private class BlackTapeTween
    {
        [field:SerializeField]
        public RectTransform Rect { get; private set; }
        public Tween Tween { get; set; }
    }

    [SerializeField] private List<BlackTapeTween> _blackTapes;
    [Header("Parameters"), SerializeField, Min(0)] private float _maxSize;
    [SerializeField, Min(0)] private float _animationTime;
    [SerializeField] private Ease _easeValue;

    private void Start()
    {
        if (_blackTapes?.Count != 0)
        {
            Vector2 targetSize = new Vector2(_blackTapes[0].Rect.sizeDelta.x, 0);
            foreach (BlackTapeTween btt in _blackTapes)
            {
                btt.Tween?.Kill();
                btt.Rect.sizeDelta = targetSize;
            }
        }
    }

    private void OnEnable()
    {
        InstanceManager.UIManager.OnDisplayBlackTapes += OnBlackTapeDisplayed;
    }

    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDisplayBlackTapes -= OnBlackTapeDisplayed;
    }

    private void OnBlackTapeDisplayed(bool displayed)
    {
        if (_blackTapes?.Count != 0)
        {
            Vector2 targetSize = new Vector2(_blackTapes[0].Rect.sizeDelta.x, displayed ? _maxSize : 0);
            foreach (BlackTapeTween btt in _blackTapes)
            {
                btt.Tween?.Kill();
                btt.Tween = btt.Rect.DOSizeDelta(targetSize, _animationTime).SetEase(_easeValue);
            }
        }
    }
}
