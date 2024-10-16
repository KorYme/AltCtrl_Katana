using UnityEngine;

[CreateAssetMenu(fileName = nameof(RoundData), menuName = "Content/RoundData/" + nameof(RoundData), order = -1)]
public class RoundData : ScriptableObject
{
    [field: SerializeField, Min(0f)] public float MaxRoundTime { get; set; }
}
