using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
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

    public event Action<int, ActionType> OnPlayerActionInput;

    [SerializeField] List<InputBinding> _bindings;

    private Dictionary<int, ActionType> _currentActions;
    public IReadOnlyDictionary<int, ActionType> CurrentActions => _currentActions;

    private bool _isSequenceStarted;

    private float _elapsedTime;
    private bool _isTimerRunning;

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


    private void Update()
    {
        if (_isTimerRunning) _elapsedTime += Time.fixedDeltaTime;
    }

    private void UpdateInputs(int playerIndex, ActionType type, bool uncovered)
    {
        if ((int)type < (int)_currentActions[playerIndex]) return;
        if (type == ActionType.Sheath)
        {
            if (uncovered && !_isSequenceStarted)
            {
                StartTimer();
                _isSequenceStarted = true;
            }
            else if (!uncovered && _isSequenceStarted)
            {
                StopTimer();
                OnPlayerActionInput.Invoke(playerIndex, _currentActions[playerIndex]);
            }
        }
        else if (uncovered && _isSequenceStarted) _currentActions[playerIndex] = type;
    }

    private void StartTimer()
    {
        _isTimerRunning = true;
    }

    private float StopTimer()
    {
        if (!_isTimerRunning) return 0f;
        _isTimerRunning = false;
        float givenTime = _elapsedTime;
        _elapsedTime = 0;
        return givenTime;
    }

}
