using UnityEditor;
using UnityEngine;

public abstract class BasicEditor<T> : Editor where T : Object
{
    protected T _target;
    
    protected virtual void OnEnable()
    {
        _target = (T)target;
    }
}
