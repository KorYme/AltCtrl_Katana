public class DuelRound : Round
{
    public override void StartRound(RoundData data)
    {
        base.StartRound(data);
        InstanceManager.InputManager.OnPlayerActionInput += OnPlayerActionInput;
    }

    private void OnPlayerActionInput(int playerId, ActionType action)
    {
        if (action == ActionType.Sheath)
        {
            return;
        }
        StopRound(playerId == 0 ? RoundResult.Player1Victory : RoundResult.Player2Victory);
    }
}