using System;
using UnityEngine;

public enum RoundResult
{
    OnGoing,
    Player1Victory,
    Player2Victory,
    Draw,
}

public abstract class Round
{
    protected RoundData _data;

    protected float _roundTime = -1f;

    public event Action<RoundResult> OnRoundEnd;

    public virtual void StartRound(RoundData data)
    {
        _data = data;
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
        Debug.Log(result);
        OnRoundEnd?.Invoke(result);
        _roundTime = -1f;
    }
}
