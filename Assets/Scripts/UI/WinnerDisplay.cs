using DG.Tweening;
using TMPro;
using UnityEngine;

public class WinnerDisplay : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _textComponent;
    [SerializeField, Min(0.1f)] private float _fadeDuration;
    [SerializeField] private Ease _fadeEase;
    
    private void Start()
    {
        InstanceManager.UIManager.OnDisplayWinner += OnDisplayWinner;
    }
    
    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDisplayWinner -= OnDisplayWinner;
    }

    private void OnDisplayWinner(RoundResult result)
    {
        _textComponent.SetText(result switch
        {
            RoundResult.Draw => "Draw",
            RoundResult.Player1Victory => "Player 1 Victory",
            RoundResult.Player2Victory => "Player 2 Victory",
            _ => "Bad Result returned",
        });
        _canvasGroup.DOFade(1, _fadeDuration).SetEase(_fadeEase);
    }
}
