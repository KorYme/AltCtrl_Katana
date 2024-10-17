using UnityEngine;

public enum GamemodeType
{
    Match = 0,
    Tutorial = 1,
}

public abstract class GameMode : MonoBehaviour
{
    public abstract GamemodeType Type { get; }
    
    protected Round _currentRound;
    
    public abstract void StartGameMode();
    public abstract void StopGameMode();
    public abstract void UpdateGameMode(float deltaTime);

    protected virtual void OnStartNewRound()
    {
        
    }
}