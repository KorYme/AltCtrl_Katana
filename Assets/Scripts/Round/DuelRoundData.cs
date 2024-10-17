using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = nameof(DuelRoundData), menuName = "Content/RoundData/" + nameof(DuelRoundData))]
public class DuelRoundData : RoundData
{
    [SerializeField, Min(0f)] private float _triggerMinTime;
    [SerializeField, Min(0f)] private float _triggerMaxTime;

    public float GetRandomTimer()
    {
        return Random.Range(_triggerMinTime, _triggerMaxTime);
    }

    private void OnValidate()
    {
        if (_triggerMinTime > _triggerMaxTime)
        {
            _triggerMinTime = _triggerMaxTime;
        }
        if (MaxRoundTime < _triggerMaxTime)
        {
            MaxRoundTime = _triggerMaxTime;
        }
    }
}
