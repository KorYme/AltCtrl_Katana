using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyCountTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _textDisplay;
    [SerializeField] private Image _imageDisplay;
    [SerializeField] private Sprite _readyImage;
    [SerializeField] private Sprite _goImage;
    [SerializeField, Range(0f, 5f)] private float _fadeInTime;
    [SerializeField] private Ease _fadeIn;

    private void Start()
    {
        InstanceManager.UIManager.OnDuelTriggered += DisplayGo;
        InstanceManager.UIManager.OnDuelFalseStart += HideGo;
        InstanceManager.UIManager.OnDisplayWinner += HideGo;
    }
    
    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDuelTriggered -= DisplayGo;
        InstanceManager.UIManager.OnDuelFalseStart -= HideGo;
        InstanceManager.UIManager.OnDisplayWinner -= HideGo;
    }

    private void DisplayGo()
    {
        _textDisplay.text = "GO";
        _imageDisplay.sprite = _goImage;
        _imageDisplay.SetNativeSize();
        DOTween.Sequence()
            .Append(_textDisplay.DOFade(1, .2f).SetEase(_fadeIn)).Join(_textDisplay.transform.DOScale(1, .2f).SetEase(_fadeIn))
            .Play();

    }

    private void HideGo()
    {
        _textDisplay.text = "";
        _imageDisplay.color = Color.clear;
    }
    
    private void HideGo(RoundResult roundResult)
    {
        _textDisplay.text = "";
        _imageDisplay.color = Color.clear;
        if (roundResult == RoundResult.Draw)
        {
        }
    }
}
