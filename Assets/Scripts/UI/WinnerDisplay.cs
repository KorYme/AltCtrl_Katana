using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerDisplay : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _textComponent;
    [SerializeField] private Image _winnerImageObject;
    [SerializeField] private Image _winnerSwordImage;
    [SerializeField] private List<Sprite> _winnerSwordList;
    [SerializeField] private List<Sprite> _winnerImageList;
    [SerializeField, Min(0.1f)] private float _fadeDuration;
    [SerializeField] private Ease _fadeEase;
    
    private void Start()
    {
        InstanceManager.UIManager.OnDisplayWinner += OnDisplayWinner;
        
        _winnerImageObject.SetNativeSize();
    }
    
    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDisplayWinner -= OnDisplayWinner;
    }

    private void OnDisplayWinner(RoundResult result)
    {
        //_textComponent.SetText(result switch
        //{
        //    RoundResult.Draw => "Draw",
        //    RoundResult.Player1Victory => "Player 1 Victory",
        //    RoundResult.Player2Victory => "Player 2 Victory",
        //    _ => "Bad Result returned",
        //});
        _winnerImageObject.sprite = result switch
        {
            RoundResult.Draw => _winnerImageList[0],
            RoundResult.Player1Victory => _winnerImageList[1],
            RoundResult.Player2Victory => _winnerImageList[2],
            _ => _winnerImageList[0]
        };
        _winnerSwordImage.gameObject.SetActive(true);   
        _winnerSwordImage.sprite = result switch
        {
            RoundResult.Player1Victory => _winnerSwordList[1],
            RoundResult.Player2Victory => _winnerSwordList[2],
        };
        _winnerImageObject.SetNativeSize();
        _winnerSwordImage.SetNativeSize();
        _winnerImageObject.gameObject.SetActive(true);
        _canvasGroup.DOFade(1, _fadeDuration).SetEase(_fadeEase);
    }
}
