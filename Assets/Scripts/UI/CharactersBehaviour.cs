using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharactersBehaviour : MonoBehaviour
{
    private readonly int _attackTrigger = Animator.StringToHash("Attack");
    private readonly int _deathTrigger = Animator.StringToHash("Death");
    
    [SerializeField] private List<Animator> _animators;
    private Animator BlueSamuraiAnimator => _animators[0];
    private Animator RedSamuraiAnimator => _animators[1];
    [SerializeField] private List<Image> _samuraiVictoryImages;
    private Image BlueSamuraiVictoryImage => _samuraiVictoryImages[0];
    private Image RedSamuraiVictoryImage => _samuraiVictoryImages[1];

    [Header("Gamefeel")]
    [SerializeField] private TweenOptions _tweensData;
    
    private void Start()
    {
        InstanceManager.UIManager.OnDisplayWinner += OnRoundResult;
        InstanceManager.UIManager.OnDuelInput += PlayerAttack;
        // InstanceManager.UIManager.OnFlashAnimEnded += SwitchSamuraiPlaces;
        foreach (Animator animator in _animators)
        {
            DOTween.Sequence()
                .Join(animator.transform.DOScaleX(_tweensData.CharactersBounceX.Value, _tweensData.CharactersBounceX.Duration))
                .SetEase(_tweensData.CharactersBounceX.Ease)
                .Join(animator.transform.DOScaleY(_tweensData.CharactersBounceY.Value, _tweensData.CharactersBounceY.Duration))
                .SetEase(_tweensData.CharactersBounceY.Ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
        foreach (Image image in _samuraiVictoryImages)
        {
            DOTween.Sequence()
                .Join(image.transform.DOScaleX(_tweensData.CharactersBounceX.Value, _tweensData.CharactersBounceX.Duration))
                .SetEase(_tweensData.CharactersBounceX.Ease)
                .Join(image.transform.DOScaleY(_tweensData.CharactersBounceY.Value, _tweensData.CharactersBounceY.Duration))
                .SetEase(_tweensData.CharactersBounceY.Ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
    
    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDisplayWinner -= OnRoundResult;
        InstanceManager.UIManager.OnDuelInput -= PlayerAttack;
        // InstanceManager.UIManager.OnFlashAnimEnded -= SwitchSamuraiPlaces;
    }

    public void SwitchSamuraiPlaces()
    {
        RedSamuraiAnimator.transform.position += BlueSamuraiAnimator.transform.position;
        BlueSamuraiAnimator.transform.position = RedSamuraiAnimator.transform.position - BlueSamuraiAnimator.transform.position;
        RedSamuraiAnimator.transform.position -= BlueSamuraiAnimator.transform.position;
    }

    private void PlayerAttack(RoundResult roundResult)
    {
        switch (roundResult)
        {
            case RoundResult.Player1Victory:
                BlueSamuraiAnimator.SetTrigger(_attackTrigger);
                break;
            case RoundResult.Player2Victory:
                RedSamuraiAnimator.SetTrigger(_attackTrigger);
                break;
            default:
                return;
        }
    }
    
    private void OnRoundResult(RoundResult roundResult)
    {
        switch (roundResult)
        {
            case RoundResult.Player1Victory:
                RedSamuraiAnimator.SetTrigger(_deathTrigger);
                BlueSamuraiAnimator.gameObject.SetActive(false);
                BlueSamuraiVictoryImage.enabled = true;
                return;
            case RoundResult.Player2Victory:
                BlueSamuraiAnimator.SetTrigger(_deathTrigger);
                RedSamuraiAnimator.gameObject.SetActive(false);
                RedSamuraiVictoryImage.enabled = true;
                return;
            default:
                return;
        }
    }
}
