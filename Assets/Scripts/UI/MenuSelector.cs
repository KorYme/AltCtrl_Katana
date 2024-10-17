using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private float _secondsBeforeMovement;

    private int _currentIndex;
    private int _indexModifier;
    private float _movementTimer;
    private bool _hasJustMoved;
    
    private void Start()
    {
        if (_buttons?.Count == 0)
        {
            Debug.Log("No buttons serialized");
            return;
        }
        _hasJustMoved = false;
        _currentIndex = 0;
        _buttons?[_currentIndex].Select();
        _movementTimer = _secondsBeforeMovement;
        InstanceManager.InputManager.OnPlayerPositionChanged += OnSwordPositionChanged;
    }

    private void OnDestroy()
    {
        InstanceManager.InputManager.OnPlayerPositionChanged -= OnSwordPositionChanged;
    }

    private void Update()
    {
        _movementTimer -= Time.deltaTime;
        if (_movementTimer <= 0f)
        {
            _hasJustMoved = false;
            _movementTimer = _secondsBeforeMovement;
            _currentIndex = (_currentIndex + _indexModifier + _buttons.Count) % _buttons.Count;
            _buttons[_currentIndex].Select();
        }
    }

    private void OnSwordPositionChanged(int playerId, ActionType actionType)
    {
        if (playerId != 0) return;
        if (_hasJustMoved && actionType == ActionType.Sheath)
        {
            _buttons[_currentIndex]?.onClick?.Invoke();
            return;
        }
        _hasJustMoved = true;
        _movementTimer = _secondsBeforeMovement;
        _indexModifier = actionType switch
        {
            ActionType.Feint => -1,
            ActionType.Counter => 1,
            _ => 0
        };
    }
}
