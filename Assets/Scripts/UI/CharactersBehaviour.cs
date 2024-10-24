using UnityEngine;

public class CharactersBehaviour : MonoBehaviour
{
    private readonly int _attackTrigger = Animator.StringToHash("Attack");
    private readonly int _deathTrigger = Animator.StringToHash("Death");
    
    [SerializeField] private Animator _redSamuraiAnimator;
    [SerializeField] private Animator _blueSamuraiAnimator;

    private void Start()
    {
        InstanceManager.UIManager.OnDisplayWinner += OnRoundResult;
        InstanceManager.UIManager.OnDuelInput += PlayerAttack;
    }
    
    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDisplayWinner -= OnRoundResult;
        InstanceManager.UIManager.OnDuelInput -= PlayerAttack;
    }

    public void SwitchSamuraiPlaces()
    {
        _redSamuraiAnimator.transform.position += _blueSamuraiAnimator.transform.position;
        _blueSamuraiAnimator.transform.position = _redSamuraiAnimator.transform.position - _blueSamuraiAnimator.transform.position;
        _redSamuraiAnimator.transform.position -= _blueSamuraiAnimator.transform.position;
    }

    private void PlayerAttack(RoundResult roundResult)
    {
        switch (roundResult)
        {
            case RoundResult.Player1Victory:
                _blueSamuraiAnimator.SetTrigger(_attackTrigger);
                break;
            case RoundResult.Player2Victory:
                _redSamuraiAnimator.SetTrigger(_attackTrigger);
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
                _redSamuraiAnimator.SetTrigger(_deathTrigger);
                return;
            case RoundResult.Player2Victory:
                _blueSamuraiAnimator.SetTrigger(_deathTrigger);
                return;
            default:
                return;
        }
    }
}
