public abstract class Round
{
    protected float _roundTime = -1f;

    public virtual void Update(float deltaTime)
    {
        
    }

    public virtual void StopRound()
    {
        _roundTime = -1f;
    }
}
