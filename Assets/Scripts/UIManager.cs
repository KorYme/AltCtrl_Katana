using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.UIManager != null)
        {
            Destroy(this);
        }
        GameManager.UIManager = this;
    }
}
