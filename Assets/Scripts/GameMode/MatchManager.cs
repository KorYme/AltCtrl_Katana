using UnityEngine;

public class MatchManager : GameModeManager
{
    public override GamemodeType Type => GamemodeType.Match;

    [SerializeField] private RoundData _roundData;

    private Round _currentRound;

    public override void StartGameMode()
    {
        // Open the scene and play animation
        // Wait for animation to end
        // Wait for player to be ready => Add a mehod in InputManager
        // Start round
    }

    public override void StopGameMode()
    {
        // Stop round
        _currentRound = null;
    }

    public override void UpdateGameMode(float deltaTime)
    {
        _currentRound?.Update(deltaTime);
    }
}
