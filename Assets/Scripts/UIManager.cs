using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        if (InstanceManager.UIManager != null)
        {
            Destroy(gameObject);
            return;
        }
        InstanceManager.UIManager = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

    }

    // COMMON
    // Called when flash anim ended
    public Action OnFlashAnimEnded;
    // Called when you want to display the winner text
    public Action<RoundResult> OnDisplayWinner;
    
    // DUEL
    // Called when duel round starts
    public Action OnDuelStarted;
    // Called when players can attack
    public Action OnDuelTriggered;
    // Called when a player wins thanks to a sheath input
    public Action<RoundResult> OnDuelInput;
    // Called when feedbacks are played X secondes after a player made its input
    public Action<RoundResult> OnDuelFinished;

    // UPDATE
    public Action OnTransitionComplete;

    // REQUEST
    public Action<int, bool> OnPlayerReadyStateUpdateRequest;
    public Action OnTransitionRequest;
    public Action OnTransitionCancelRequest;
    public Action<bool> OnFadeRequested;
    public Action<bool> OnShowRetryActionRequest;
    public Action OnReturnToMenuRequest;
}
