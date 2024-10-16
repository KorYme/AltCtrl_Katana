using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Gamemode Managers")] 
    [SerializeField] private List<GameMode> _gameModeManagers;

    private GameMode _currentGameMode;
    
    private void Awake()
    {
        if (InstanceManager.GameManager != null)
        {
            Destroy(this);
        }
        InstanceManager.GameManager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        _currentGameMode?.UpdateGameMode(Time.deltaTime);
    }

    public void StartGamemode(int gamemodeIndex)
    {
        if (!Enum.IsDefined(typeof(GamemodeType), gamemodeIndex))
        {
            Debug.LogError("The gamemode index you're trying to start is not linked to any existing gamemode");
        }
        GameMode gameMode = _gameModeManagers.FirstOrDefault(manager => manager.Type == (GamemodeType)gamemodeIndex);
        if (gameMode is null)
        {
            Debug.LogError("No gamemode has been found with this id, make sure it is referenced in the GameManager");
        }
        _currentGameMode?.StopGameMode();
        _currentGameMode = gameMode;
        _currentGameMode?.StartGameMode();
    }
}