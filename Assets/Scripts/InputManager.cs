using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionType
{
    None,
    Sheath,
    Attack,
    Crit,
    Counter
}

public class InputManager : MonoBehaviour
{

    [Serializable]
    public class InputBinding
    {
        public InputActionReference actionRef;
        [Range(0, 1)]
        public int playerID;
        public ActionType bindType;

        public Action<int, ActionType, bool> OnInputBind;


        public void BindStartInput(InputAction.CallbackContext ctx)
        {
            OnInputBind?.Invoke(playerID, bindType, true);
        }
        public void BindCancelInput(InputAction.CallbackContext ctx)
        {
            OnInputBind?.Invoke(playerID, bindType, false);
        }
    }

    public event Action<int, ActionType> OnPlayerActionInput; // IS INVOKED WHEN A PLAYER SHEATH HIS SWORD, RETURNS MAX ACTION TYPE REACHED
    public event Action<int, ActionType> OnPlayerPositionChanged; // IS INVOKED WHEN A PLAYER TRIGGER OR UNTRIGGER A ZONE ON THE SWORD, RETURN THE POSITION OF THE SWORD

    [SerializeField] List<InputBinding> _bindings;

    private Dictionary<int, ActionType> _currentActions;
    public IReadOnlyDictionary<int, ActionType> CurrentActions => _currentActions;

    private void OnEnable()
    {
        foreach (InputBinding binding in _bindings)
        {
            binding.actionRef.action.started += binding.BindStartInput;
            binding.actionRef.action.canceled += binding.BindCancelInput;
            binding.OnInputBind += UpdateInputs;
        }
        // set the action's performed as a method to update the input;
    }

    private void OnDisable()
    {
        foreach (InputBinding binding in _bindings)
        {
            binding.actionRef.action.started -= binding.BindStartInput;
            binding.actionRef.action.canceled -= binding.BindCancelInput;
            binding.OnInputBind -= UpdateInputs;
        }
    }


    private void Awake()
    {
        if (InstanceManager.InputManager != null)
        {
            Destroy(this);
        }
        InstanceManager.InputManager = this;
        DontDestroyOnLoad(gameObject);
    }



    private void UpdateInputs(int playerIndex, ActionType type, bool uncovered)
    {
        switch (type)
        {
            case ActionType.Sheath:

                break;
        }
    }


}
