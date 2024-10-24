using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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

    public abstract void StartGameMode();
    public virtual void StopGameMode()
    {
        SceneManager.LoadScene(0);
    }
    public abstract void UpdateGameMode(float deltaTime);



}