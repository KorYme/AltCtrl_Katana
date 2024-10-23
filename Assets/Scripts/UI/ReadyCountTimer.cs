using DG.Tweening;
using TMPro;
using UnityEngine;

public class ReadyCountTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _textDisplay;
    [SerializeField, Range(0f, 5f)] private float _fadeInTime;
    [SerializeField] private Ease _fadeIn;

    private void Start()
    {
        InstanceManager.UIManager.OnDuelStarted += DisplayReady;
        InstanceManager.UIManager.OnDuelTriggered += DisplayGo;
    }
    
    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDuelStarted -= DisplayReady;
        InstanceManager.UIManager.OnDuelTriggered -= DisplayGo;
    }

    private void DisplayReady()
    {
        // _textDisplay.color = Color.white;
        // _textDisplay.text = "Ready ?";
    }

    private void DisplayGo()
    {
        _textDisplay.color = Color.white;
        _textDisplay.text = "GO";
        DOTween.Sequence()
            .Append(_textDisplay.DOFade(1, .2f).SetEase(_fadeIn)).Join(_textDisplay.transform.DOScale(1, .2f).SetEase(_fadeIn))
            .Play();
    }
    
    private void HideGo()
    {
        _textDisplay.color = Color.clear;
    }
    
}
