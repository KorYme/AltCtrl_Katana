using UnityEngine;

public class FlashScreen : MonoBehaviour
{
    private static readonly int FlashAnimId = Animator.StringToHash("Flash");
    
    [SerializeField] private CharactersBehaviour _charactersBehaviour;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        InstanceManager.UIManager.OnDuelFinished += StartFlash;
    }

    private void OnDestroy()
    {
        InstanceManager.UIManager.OnDuelFinished -= StartFlash;
    }
    
    private void StartFlash(RoundResult roundResult)
    {
        switch (roundResult)
        {
            case RoundResult.Player1Victory:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                InstanceManager.AudioManager.PlayClip("Attack1");
                break;
            case RoundResult.Player2Victory:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                InstanceManager.AudioManager.PlayClip("Attack2");
                break;
            default:
                return;
        }
        _animator.SetTrigger(FlashAnimId);
    }
    
    public void OnFlashFull()
    {
        _charactersBehaviour.SwitchSamuraiPlaces();
        InstanceManager.UIManager.OnFlashAnimEnded?.Invoke();
    }
}
