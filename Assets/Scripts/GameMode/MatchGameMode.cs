using UnityEngine;

public class MatchGameMode : GameMode
{
    public override GamemodeType Type => GamemodeType.Match;
    
    [SerializeField] protected RoundData _climaxRoundData;

    public override void StartGameMode()
    {
        _currentRound = new DuelRound();
        // Open the scene and play animation
        // Wait for animation to end
        // Wait for player to be ready => Add a method in InputManager
        _currentRound?.StartRound(_climaxRoundData);
    }

    public override void StopGameMode()
    {
        _currentRound?.StopRound(RoundResult.Draw);
        _currentRound = null;
    }

    public override void UpdateGameMode(float deltaTime)
    {
        _currentRound?.Update(deltaTime);
    }
}
