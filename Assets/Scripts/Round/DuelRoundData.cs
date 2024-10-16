using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DuelRoundData_", menuName = "Content/RoundData")]
public class DuelRoundData : RoundData
{
    public Vector2 RandomRange;

    private void OnValidate()
    {
        if (RandomRange.x > RandomRange.y)
        {
            RandomRange = new Vector2(RandomRange.y, RandomRange.y);
        }
        if (MaxRoundTime < RandomRange.y)
        {
            MaxRoundTime = RandomRange.y;
        }
    }
}
