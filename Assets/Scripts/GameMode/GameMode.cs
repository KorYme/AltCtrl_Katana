using System.Collections;
using System.Runtime.CompilerServices;
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

    protected bool _isTransitionComplete;
    public abstract void StartGameMode();
    public abstract void StopGameMode();
    public abstract void UpdateGameMode(float deltaTime);

    private void OnEnable()
    {
        InstanceManager.InputManager.OnPlayerPositionChanged += OnPlayerPositionChanged;
        InstanceManager.UIManager.OnTransitionComplete += OnTransitionComplete;
    }

    private void OnDisable()
    {
        InstanceManager.InputManager.OnPlayerPositionChanged -= OnPlayerPositionChanged;
    }

    private void OnPlayerPositionChanged(int ind, ActionType state) => CheckPlayersState();
    private void OnTransitionComplete() => _isTransitionComplete = true;


    private void Start()
    {
        Invoke("CheckPlayersState", 2.5f);
    }
    private void CheckPlayersState()
    {
        if (_isTransitionComplete) return;
        bool arePlayersReady = true;
        for (int i = 0; i < 2; i++)
        {
            var playerState = InstanceManager.InputManager.CurrentActions[i];
            if (playerState != ActionType.Sheath)
            {
                arePlayersReady = false;
                InstanceManager.UIManager.OnPlayerReadyStateUpdateRequest?.Invoke(i, false);
            }
            else
            {
                InstanceManager.UIManager.OnPlayerReadyStateUpdateRequest?.Invoke(i, true);
                Debug.Log($"Player {i + 1} is ready !");
            }
        }
        if (arePlayersReady)
        {
            Debug.Log("BOTH PLAYERS READY. Launching transition...");
            InstanceManager.UIManager.OnTransitionRequest?.Invoke();
        }
        else InstanceManager.UIManager.OnTransitionCancelRequest?.Invoke();
    }

}