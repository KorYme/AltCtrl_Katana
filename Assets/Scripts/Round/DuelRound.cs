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
        base.StartRound(data);
        _startTimer = duelData.GetRandomTimer();
        InstanceManager.InputManager.OnPlayerActionInput += OnPlayerActionInput;
    }

    public override void StopRound(RoundResult result)
    {
        InstanceManager.InputManager.OnPlayerActionInput -= OnPlayerActionInput;
        base.StopRound(result);
    }
    protected void OnPlayerActionInput(int playerId, ActionType action)
    {
        if (action == ActionType.Sheath)
        {
            return;
        }
        StopRound(playerId == 0 ? RoundResult.Player1Victory : RoundResult.Player2Victory);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (_startTimer <= 0f)
        {
            return;
        }
        if (false) // CHECK IF PLAYERS SHEATHED
        {
            // CHOOSE THE RIGHT PLAYER TO WIN
            StopRound(RoundResult.Draw);
        }
        _startTimer -= deltaTime;
        if (_startTimer <= 0f)
        {
            OnDuelTriggered();
        }
    }

    protected void OnDuelTriggered()
    {
        // FULL FEEDBACK AND ENABLE CONTROLS
    }
}