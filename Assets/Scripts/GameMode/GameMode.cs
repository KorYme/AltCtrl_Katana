using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public enum GamemodeType
{
    Match = 0,
    Tutorial = 1,
}

public abstract class GameMode : MonoBehaviour
{
    public abstract GamemodeType Type { get; }

    protected Round _currentRound;

    [SerializeField] protected string _sceneName;
    public string SceneName => _sceneName;
    
    protected bool _areBothPlayersReady;
    public abstract void StartGameMode();
    public abstract void StopGameMode();
    public abstract void UpdateGameMode(float deltaTime);

    private void OnEnable()
    {
        InstanceManager.InputManager.OnPlayerPositionChanged += OnPlayerPositionChanged;
    }

    private void OnDisable()
    {
        InstanceManager.InputManager.OnPlayerPositionChanged -= OnPlayerPositionChanged;
    }

    private void OnPlayerPositionChanged(int ind, ActionType state) => CheckPlayersState();


    protected virtual void OnStartNewRound()
    {

    }

    private void CheckPlayersState()
    {
        if (_areBothPlayersReady) return;
        bool arePlayersReady = true;
        for (int i = 0; i < 2; i++)
        {
            var playerState = InstanceManager.InputManager.CurrentActions[i];
            if (playerState != ActionType.Sheath)
            {
                arePlayersReady = false;
                InstanceManager.UIManager.OnPlayerReadyStateUpdateRequest?.Invoke(i, false);
                break;
            }
            else
            {
                InstanceManager.UIManager.OnPlayerReadyStateUpdateRequest?.Invoke(i, true);
                break;
            }
        }
        if (arePlayersReady) InstanceManager.UIManager.OnTransitionRequest?.Invoke();
        else InstanceManager.UIManager.OnTransitionCancelRequest?.Invoke();
    }

}