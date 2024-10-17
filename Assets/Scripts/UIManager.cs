using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        if (InstanceManager.UIManager != null)
        {
            Destroy(this);
        }
        InstanceManager.UIManager = this;
        DontDestroyOnLoad(gameObject);
    }

    // DUEL
    public Action<bool> OnDisplayBlackTapes;        // Called when black tapes must be displayed
    public Action OnDuelStarted;                    // Called when players can attack
    public Action OnDuelFinished;                   // Called when a player slash first
}
