using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        if (InstanceManager.UIManager != null)
        {
            Destroy(gameObject);
        }
        InstanceManager.UIManager = this;
        DontDestroyOnLoad(gameObject);
    }

    // COMMON
    public Action<bool> OnDisplayBlackTapes;        // Called when black tapes must be displayed
    public Action<RoundResult> OnDisplayWinner;     // Called when you want to display the winner text
    
    // DUEL
    public Action OnDuelStarted;                    // Called when players can attack
    public Action OnDuelFinished;                   // Called when a player slash first
    public Action OnFlashAnimEnded;                 // Called when flash anim ended
}
