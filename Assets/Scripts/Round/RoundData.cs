using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundData_", menuName = "Content/RoundData")]
public class RoundData : ScriptableObject
{
    [field: SerializeField] public float MaxRoundTime { get; set; }
}
