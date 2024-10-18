 using UnityEditor;

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
            _swordValues[i] = EditorGUILayout.Slider(_swordValues[i], 0f, 3.01f);
            bool isUncovering = tmpValue < _swordValues[i];
            if (EditorGUI.EndChangeCheck() && ComputeActionType(tmpValue) != ComputeActionType(_swordValues[i]))
            {
                _target.UpdateInputs(i, ComputeActionType(_swordValues[i]), isUncovering);
            }
        }
    }
    
    private ActionType ComputeActionType(float i)
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
}