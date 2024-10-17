using System;
using UnityEngine;

public class FlashScreen : MonoBehaviour
{
    private static readonly int FlashAnimId = Animator.StringToHash("Flash");
    
    [SerializeField] private CharactersBehaviour _charactersBehaviour;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        InstanceManager.UIManager.OnDuelFinished += StartFlash;
    }

    private void OnDisable()
    {
        InstanceManager.UIManager.OnDuelFinished -= StartFlash;
    }
    
    private void StartFlash()
    {
        _animator.SetTrigger(FlashAnimId);
    }
    
    public void OnFlashFull()
    {
        _charactersBehaviour.SwitchSamuraiPlace();
    }
}
