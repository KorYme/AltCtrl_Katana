using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{

    protected bool _isTransitionComplete;

    private void OnPlayerPositionChanged(int ind, ActionType state) => CheckPlayersState();
    private void OnTransitionComplete() => _isTransitionComplete = true;


    private void Start()
    {
        InstanceManager.InputManager.OnPlayerPositionChanged += OnPlayerPositionChanged;
        InstanceManager.UIManager.OnTransitionComplete += OnTransitionComplete;
    }

    private void OnDestroy()
    {
        InstanceManager.InputManager.OnPlayerPositionChanged -= OnPlayerPositionChanged;
        InstanceManager.UIManager.OnTransitionComplete -= OnTransitionComplete;
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
