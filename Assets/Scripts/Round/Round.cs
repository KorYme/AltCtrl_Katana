using System;

public enum RoundType
{
    Duel,
}

public enum RoundResult
{
    Player1Victory,
    Player2Victory,
    Draw,
}

public abstract class Round
{
    protected float _roundTime = -1f;

    public event Action<RoundResult> OnRoundEnd;

    public virtual void StartRound(RoundData data)
    {
        _roundTime = data.MaxRoundTime;
    }
    
    public virtual void Update(float deltaTime)
    {
        if (_roundTime <= 0f)
        {
            return;
        }
        _roundTime -= deltaTime;
        if (_roundTime <= 0f)
        {
            StopRound(RoundResult.Draw);
        }
    }

    public virtual void StopRound(RoundResult result)
    {
        OnRoundEnd?.Invoke(result);
        _roundTime = -1f;
    }
}
