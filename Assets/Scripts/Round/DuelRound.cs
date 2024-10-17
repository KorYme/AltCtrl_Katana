using System;

public class DuelRound : Round
{
    protected float _startTimer;

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
    }

    public override void StopRound(RoundResult result)
    {
        InstanceManager.InputManager.OnPlayerActionInput -= OnPlayerActionInput;
        InstanceManager.InputManager.OnPlayerPositionChanged -= OnPlayerPositionChanged;
        base.StopRound(result);
    }
    protected void OnPlayerActionInput(int playerId, ActionType action)
    {
        if (action == ActionType.Sheath || _startTimer > 0f)
        {
            return;
        }
        StopRound(playerId == 0 ? RoundResult.Player1Victory : RoundResult.Player2Victory);
    }

    protected void OnPlayerPositionChanged(int playerId, ActionType action)
    {
        if (action == ActionType.Sheath || _startTimer <= 0f)
        {
            return;
        }
        // PLAY ANIMATION SLASH AND GIVE WIN OR LOSE POSITION TO CHARACTERS
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