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

    public Action OnDuelTriggered;
    
    // REQUEST
    public Action<bool> OnFadeRequested;
}
