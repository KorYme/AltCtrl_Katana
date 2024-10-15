using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static UIManager UIManager { get; set; }
    public static InputManager InputManager { get; set; }

    [Header("Gamemode Managers")] 
    [SerializeField] private MatchManager _matchManager;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public void StartGamemode(int gamemodeIndex)
    {
        switch (gamemodeIndex)
        {
            case 0:
                break;
            case 1:
                break;
            default:
                break;
        }
    }
}
