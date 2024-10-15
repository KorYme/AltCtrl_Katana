using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    Feint,
    Attack,
    Counter
}

public class InputManager : MonoBehaviour
{
    public Action<int, ActionType> OnPlayerActionInput;
    
    private void Awake()
    {
        if (GameManager.InputManager != null)
        {
            Destroy(this);
        }
        GameManager.InputManager = this;
    }
}
