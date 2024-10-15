using System;
using UnityEngine;

public enum ActionType
{
    Feint,
    Attack,
    Counter
}

public class InputManager : MonoBehaviour
{
    public event Action<int, ActionType> OnPlayerActionInput;
    
    private void Awake()
    {
        if (InstanceManager.InputManager != null)
        {
            Destroy(this);
        }
        InstanceManager.InputManager = this;
        DontDestroyOnLoad(gameObject);
    }
}
