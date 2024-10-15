public class ClimaxRound : Round
{
    public ClimaxRound(RoundData data)
    {
        _roundTime = data.MaxRoundTime;
    }
    
    public override void Update(float deltaTime)
    {
    }
}
