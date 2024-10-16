using UnityEngine;

[CreateAssetMenu(fileName = nameof(DuelRoundData), menuName = "Content/RoundData/" + nameof(DuelRoundData))]
public class DuelRoundData : RoundData
{
    [SerializeField, Min(0f)] private float _minValue, _maxValue;

    public float GetRandomTimer()
    {
        return Random.Range(_minValue, _maxValue);
    }

    private void OnValidate()
    {
        if (_minValue > _maxValue)
        {
            _minValue = _maxValue;
        }
        if (MaxRoundTime < _maxValue)
        {
            MaxRoundTime = _maxValue;
        }
    }
}
