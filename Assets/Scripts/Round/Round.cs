public abstract class Round
{
    private float _roundTime = -1f;

    public virtual Round Initialize(RoundData data)
    {
        _roundTime = data.MaxRoundTime;
        return this;
    }

    public void Update(float deltaTime)
    {
        if (_roundTime <= 0f)
        {
            return;
        }
    }
}
