using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{

    public void LoadGameMode(int gamemodeIndex)
    {
        InstanceManager.GameManager.LoadGamemodeScene(gamemodeIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
