using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        if (InstanceManager.UIManager != null)
        {
            Destroy(this);
        }
        InstanceManager.UIManager = this;
        DontDestroyOnLoad(gameObject);
    }
}
