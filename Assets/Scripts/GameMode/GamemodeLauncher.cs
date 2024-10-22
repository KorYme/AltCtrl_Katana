using System.Collections;
using UnityEngine;

public class GamemodeLauncher : MonoBehaviour
{
    [SerializeField] private GamemodeType _gamemodeType;
    
    private IEnumerator Start()
    {
        yield return null;
        InstanceManager.GameManager.StartGamemode(_gamemodeType);
    }
}
