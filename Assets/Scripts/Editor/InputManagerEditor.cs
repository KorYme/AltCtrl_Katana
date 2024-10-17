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
            EditorGUILayout.LabelField($"Player{i+1} sword position", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            float tmpValue = _swordValues[i];
            _swordValues[i] = EditorGUILayout.Slider(_swordValues[i], 0f, 4.01f);
            if (EditorGUI.EndChangeCheck() && ((int)tmpValue != (int)_swordValues[i] || (tmpValue != _swordValues[i] && (tmpValue == 0f || _swordValues[i] == 0f))))
            {
                _target.UpdateInputs(i, tmpValue < _swordValues[i] ? (ActionType)(int)_swordValues[i] : (ActionType)(int)tmpValue, tmpValue < _swordValues[i]);
            }
        }
    }
}