using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputManager))]
public class InputManagerEditor : BasicEditor<InputManager>
{
    private float[] _swordValues;

    protected override void OnEnable()
    {
        base.OnEnable();
        _swordValues = new float[] { 0, 0 };
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (_target.CurrentActions?.Count == 0)
        {
            return;
        }
        for (int i = 0; i < _target.CurrentActions.Count; i++)
        {
            EditorGUILayout.LabelField($"Player{i+1} sword position : {_target.CurrentActions[i]}", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            float tmpValue = _swordValues[i];
            _swordValues[i] = EditorGUILayout.Slider(_swordValues[i], 0f, 3f);
            if (EditorGUI.EndChangeCheck() && ComputeActionZone(tmpValue) != ComputeActionZone(_swordValues[i]))
            {
                bool isUncovered = tmpValue < _swordValues[i];
                _target.UpdateInputs(i, GetInputNeeded(ComputeActionZone(_swordValues[i]), isUncovered), isUncovered);
            }
        }
    }
    
    private ActionType ComputeActionZone(float i)
    {
        return i switch
        {
            <= 0f => ActionType.Sheath,
            < 1f => ActionType.Feint,
            < 2f => ActionType.Attack,
            < 3f => ActionType.Crit,
            _ => ActionType.Counter,
        };
    }
    
    private ActionType GetInputNeeded(ActionType resultAction, bool uncov)
    {
        return resultAction switch
        {
            ActionType.Sheath => ActionType.Sheath,
            ActionType.Attack => (uncov ? ActionType.Attack : ActionType.Crit),
            ActionType.Crit => (uncov ? ActionType.Crit : ActionType.Counter),
            ActionType.Counter => ActionType.Counter,
            ActionType.Feint => (uncov ? ActionType.Sheath : ActionType.Attack),
            _ => ActionType.Feint,
        };
    }
}