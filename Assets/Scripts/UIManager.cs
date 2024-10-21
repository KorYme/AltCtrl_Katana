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

    // UPDATE
    public Action OnSwordTransitionComplete;

    // REQUEST
    public Action OnDuelTriggered;
    public Action<int, bool> OnPlayerReadyStateUpdateRequest;
    public Action OnTransitionRequest;
    public Action OnTransitionCancelRequest;
    public Action<bool> OnFadeRequested;
}
