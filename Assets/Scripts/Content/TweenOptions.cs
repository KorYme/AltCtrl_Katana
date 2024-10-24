using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "TweenOptions", menuName = "Content/TweenOptions")]
public class TweenOptions : ScriptableObject
{
    [System.Serializable]
    public struct TweenData
    {
        public float Value;
        public float Duration;
        public Ease Ease;
    }
    
    [Header("Characters Tweeners")]
    public TweenData CharactersBounceX;
    public TweenData CharactersBounceY;
}
