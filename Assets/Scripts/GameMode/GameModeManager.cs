using UnityEngine;

public enum GamemodeType
{
    Match = 0,
    Tutorial = 1,
}

public abstract class GameModeManager : MonoBehaviour
{
    public abstract GamemodeType Type { get; }

    public abstract void StartGameMode();
    public abstract void StopGameMode();
    public abstract void UpdateGameMode(float deltaTime);
}