using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchGameMode : GameMode
{
    public override GamemodeType Type => GamemodeType.Match;
    
    [SerializeField] protected DuelRoundData _duelRoundData;

    public override void StartGameMode()
    {
        _currentRound = new DuelRound();
        // Wait for animation to end
        // Wait for player to be ready => Add a method in InputManager
        _currentRound?.StartRound(_duelRoundData);
    }

    public override void StopGameMode()
    {
        _currentRound?.StopRound(RoundResult.Draw);
        _currentRound = null;
        base.StopGameMode();
    }

    public override void UpdateGameMode(float deltaTime)
    {
        _currentRound?.Update(deltaTime);
    }
}