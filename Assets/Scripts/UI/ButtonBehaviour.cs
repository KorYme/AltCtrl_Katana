using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public void StartGameMode(int gamemodeIndex)
    {
        InstanceManager.GameManager.StartGamemode(gamemodeIndex);
    }
}
