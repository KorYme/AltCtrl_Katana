using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;

    private int _currentIndex;
    
    private void Start()
    {
        if (_buttons?.Count == 0)
        {
            Debug.Log("No buttons serialized");
            return;
        }
        _buttons[0].Select();
        _currentIndex = 0;
        // BEHAVIOUR TO DETERMINE => LINK TO INPUTMANAGER
    }

    private void OnClick()
    {
        EventSystem.current.currentSelectedGameObject?.GetComponent<Button>()?.onClick?.Invoke();
    }
}
