using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionType
{
    Sheath,
    Feint,
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

    private Dictionary<int, ActionType> _currentActions = new Dictionary<int, ActionType>();
    public IReadOnlyDictionary<int, ActionType> CurrentActions => _currentActions;

    private Dictionary<int, ActionType> _sequenceActions = new Dictionary<int, ActionType>();
    public IReadOnlyDictionary<int, ActionType> SequenceActions => _sequenceActions;

    private bool _isSequenceStarted;
    private float _elapsedTime;
    private bool _isTimerRunning;


    private void OnEnable()
    {
        foreach (InputBinding binding in _bindings)
        {
            binding.actionRef.action.Enable();
            binding.actionRef.action.canceled += binding.BindStartInput;
            binding.actionRef.action.started += binding.BindCancelInput;
            binding.OnInputBind += UpdateInputs;
        }
    }

    private void OnDisable()
    {
        foreach (InputBinding binding in _bindings)
        {
            binding.actionRef.action.canceled -= binding.BindStartInput;
            binding.actionRef.action.started -= binding.BindCancelInput;
            binding.OnInputBind -= UpdateInputs;
            binding.actionRef.action.Disable();
        }
    }

    private void Awake()
    {
        if (InstanceManager.InputManager != null)
        {
            Destroy(gameObject);
        }
        InstanceManager.InputManager = this;
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < 2; i++)
        {
            _currentActions.Add(i, ActionType.Sheath);
            _sequenceActions.Add(i, ActionType.Sheath);
        }
    }


    private void Update()
    {
        if (_isTimerRunning) _elapsedTime += Time.fixedDeltaTime;
    }

    public void UpdateInputs(int playerIndex, ActionType type, bool uncovered)
    {
        Debug.Log($"Updating input for player {playerIndex}: {type} is {(uncovered ? "un" : "")}covered.");
        _currentActions[playerIndex] = UpdatePosition(type, uncovered);
        OnPlayerPositionChanged?.Invoke(playerIndex, _currentActions[playerIndex]);

        if ((int)type < (int)_sequenceActions[playerIndex]) return;
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
                _isSequenceStarted = false;
                OnPlayerActionInput?.Invoke(playerIndex, _sequenceActions[playerIndex]);
                _sequenceActions[playerIndex] = ActionType.Sheath;
            }
        }
    }

    private ActionType UpdatePosition(ActionType type, bool uncov)
    {
        return type switch
        {
            ActionType.Sheath => (uncov ? ActionType.Feint : ActionType.Sheath),
            ActionType.Attack => (uncov ? ActionType.Attack : ActionType.Feint),
            ActionType.Crit => (uncov ? ActionType.Crit : ActionType.Attack),
            ActionType.Counter => (uncov ? ActionType.Counter : ActionType.Crit),
            _ => ActionType.Sheath,
        };
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