using UnityEngine;

[CreateAssetMenu(fileName = nameof(DuelRoundData), menuName = "Content/RoundData/" + nameof(DuelRoundData))]
public class DuelRoundData : RoundData
{
    [SerializeField, Min(0f)] private float _triggerMinTime;
    [SerializeField, Min(0f)] private float _triggerMaxTime;
    [field: SerializeField] public ActionType MinimumActionToCount { get; private set; }
    [field: SerializeField, Range(0f, 10f)] public float DelayAfterInput { get; private set; }

    public float GetRandomTimer()
    {
        return Random.Range(_triggerMinTime, _triggerMaxTime);
    }

    private void OnValidate()
    {
        if (_triggerMinTime > _triggerMaxTime)
        {
            _triggerMaxTime = _triggerMinTime;
        }
    }
}
