using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DuelRound : Round
{
    protected float _startTimer;
    protected ActionType _minimumAction;

    public override void StartRound(RoundData data)
    {
        if (data is not DuelRoundData duelData)
        {
            throw new Exception($"You tried to initialize the {nameof(DuelRound)} with something else than a {nameof(DuelRoundData)}");
        }
        InstanceManager.InputManager.OnPlayerActionInput += OnPlayerActionInput;
        InstanceManager.InputManager.OnPlayerPositionChanged += OnPlayerPositionChanged;
        // ADD WAITING OR PREPARING UI
        base.StartRound(data);
        _startTimer = duelData.GetRandomTimer();
        _minimumAction = duelData.MinimumActionToCount;
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
        if (action < _minimumAction || _startTimer > 0f)
        {
            return;
        }
        InstanceManager.UIManager.OnFlashAnimEnded += OnFlashAnimEnded;
        InstanceManager.UIManager.OnDuelFinished.Invoke();
        void OnFlashAnimEnded()
        {
            InstanceManager.UIManager.OnFlashAnimEnded -= OnFlashAnimEnded;
            StopRound(playerId == 0 ? RoundResult.Player1Victory : RoundResult.Player2Victory);
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