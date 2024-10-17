using System;
using DG.Tweening;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public static Action OnFadeInComplete;
    public static Action OnFadeOutComplete;
    
    private Tweener _tweener;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [Header("Parameters"), SerializeField, Min(0.01f)] private float _fadeScreenTime;

    private void OnEnable()
    {
        InstanceManager.UIManager.OnFadeRequested += Fade;
    }

    private void OnDisable()
    {
        InstanceManager.UIManager.OnFadeRequested -= Fade;
    }

    private void Fade(bool fadeIn)
    {
        _tweener?.Kill();
        _tweener = _canvasGroup.DOFade(fadeIn ? 1 : 0, _fadeScreenTime).OnComplete(() => { if (fadeIn) OnFadeInComplete?.Invoke(); else OnFadeOutComplete?.Invoke(); });
    }
}
