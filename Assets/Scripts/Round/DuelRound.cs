using System;
using System.Collections.Generic;
using UnityEngine;

public class DuelRound : Round
{
    protected float _startTimer;
    protected float _delayTimer;
    protected ActionType _minimumAction;

    protected Dictionary<int, ActionType> _duelActions = new Dictionary<int, ActionType>();
    
    public override void StartRound(RoundData data)
    {
        if (data is not DuelRoundData duelData)
        {
            throw new Exception($"You tried to initialize the {nameof(DuelRound)} with something else than a {nameof(DuelRoundData)}");
        }
        InstanceManager.AudioManager.PlayClip("NewRound");
        InstanceManager.AudioManager.PlayClip("Prepare");
        InstanceManager.AudioManager.PlayClip("DuelTheme");
        InstanceManager.InputManager.OnPlayerActionInput += OnPlayerActionInput;
        InstanceManager.InputManager.OnPlayerPositionChanged += OnPlayerPositionChanged;
        // ADD WAITING OR PREPARING UI
        base.StartRound(data);
        _startTimer = duelData.GetRandomTimer();
        _delayTimer = duelData.DelayAfterInput;
        _minimumAction = duelData.MinimumActionToCount;
        for (int i = 0; i < 2; i++)
        {
            _duelActions.Add(i, ActionType.Sheath);
        }
        Debug.Log($"Duel Round Time = {_startTimer}s");
        InstanceManager.UIManager.OnDuelStarted?.Invoke();
    }

 

    public override void StopRound(RoundResult result)
    {
        InstanceManager.InputManager.OnPlayerActionInput -= OnPlayerActionInput;
        InstanceManager.InputManager.OnPlayerPositionChanged -= OnPlayerPositionChanged;
        base.StopRound(result);
    }
    
    protected void OnPlayerActionInput(int playerId, ActionType action)
    {
        switch (playerId)
        {
            case 0:
                if(action == ActionType.Counter) InstanceManager.AudioManager.PlayClip("Parry1");
                else InstanceManager.AudioManager.PlayClip("Attack1");
                break;
            case 1:
                if (action == ActionType.Counter) InstanceManager.AudioManager.PlayClip("Parry2");
                InstanceManager.AudioManager.PlayClip("Attack2");
                break;
        }
        if (_startTimer > 0f || _roundResult != RoundResult.OnGoing)
        {
            return;
        }
        if (_duelActions.TryGetValue(playerId, out ActionType actionType))
        {
            if (action > actionType)
            {
                _duelActions[playerId] = action;
            }
            else if (action == ActionType.Sheath && actionType >= _minimumAction)
            {
                _roundResult = playerId switch
                {
                    0 => RoundResult.Player1Victory,
                    1 => RoundResult.Player2Victory,
                    _ => RoundResult.Draw,
                };
                InstanceManager.UIManager.OnDuelInput.Invoke(_roundResult);
            }
        }
    }

    protected void OnPlayerPositionChanged(int playerId, ActionType action)
    {
        if (action == ActionType.Sheath || _startTimer <= 0f)
        {
            return;
        }
        StopRound(playerId != 0 ? RoundResult.Player1Victory : RoundResult.Player2Victory);
    }

    public override void Update(float deltaTime)
    {
        if (_roundResult != RoundResult.OnGoing && _delayTimer > 0f)
        {
            _delayTimer -= deltaTime;
            if (_delayTimer <= 0f)
            {
                InstanceManager.UIManager.OnFlashAnimEnded += OnFlashAnimEnded;
                InstanceManager.UIManager.OnDuelFinished.Invoke(_roundResult);
                void OnFlashAnimEnded()
                {
                    InstanceManager.UIManager.OnFlashAnimEnded -= OnFlashAnimEnded;
                    StopRound(_roundResult);
                }
            }
        }
        if (_roundResult != RoundResult.OnGoing)
        {
            return;
        }
        if (_startTimer <= 0f)
        {
            base.Update(deltaTime);
            return;
        }
        _startTimer -= deltaTime;
        if (_startTimer <= 0f)
        {
            InstanceManager.UIManager.OnDuelTriggered?.Invoke();
            InstanceManager.AudioManager.PlayClip(_data.StartRoundClipName);
        }
    }
}