using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameMode> _gameModeManagers;
    private GameMode _currentGameMode;
    
    
    private void Awake()
    {
        if (InstanceManager.GameManager != null)
        {
            Destroy(gameObject);
            return;
        }
        InstanceManager.GameManager = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        InstanceManager.UIManager.OnReturnToMenuRequest += ReturnToMainMenu;
        
    }

    private void OnDestroy()
    {
        InstanceManager.UIManager.OnReturnToMenuRequest -= ReturnToMainMenu;

    }
    private void Update()
    {
        _currentGameMode?.UpdateGameMode(Time.deltaTime);
    }

    public void LoadGamemodeScene(int gamemodeIndex)
    {
        if (!Enum.IsDefined(typeof(GamemodeType), gamemodeIndex))
        {
            Debug.LogError("The gamemode index you're trying to start is not linked to any existing gamemode");
            return;
        }
        LoadGamemodeScene((GamemodeType)gamemodeIndex);
    }
    
    public void LoadGamemodeScene(GamemodeType gamemodeType)
    {
        GameMode gameMode = _gameModeManagers.FirstOrDefault(manager => manager.Type == gamemodeType);
        if (gameMode is null)
        {
            Debug.LogError("No gamemode has been found with this id, make sure it is referenced in the GameManager");
            return;
        }
        SceneManager.LoadScene(gameMode.SceneName, LoadSceneMode.Single);
    }
    
    public void StartGamemode(int gamemodeIndex)
    {
        if (!Enum.IsDefined(typeof(GamemodeType), gamemodeIndex))
        {
            Debug.LogError("The gamemode index you're trying to start is not linked to any existing gamemode");
            return;
        }
        StartGamemode((GamemodeType)gamemodeIndex);
    }

    public void StartGamemode(GamemodeType gamemodeType)
    {
        GameMode gameMode = _gameModeManagers.FirstOrDefault(manager => manager.Type == gamemodeType);
        if (gameMode is null)
        {
            Debug.LogError("No gamemode has been found with this id, make sure it is referenced in the GameManager");
            return;
        }
        _currentGameMode = gameMode;
        _currentGameMode?.StartGameMode();
    }

    private void ReturnToMainMenu()
    {
        _currentGameMode.StopGameMode();

    }
}
