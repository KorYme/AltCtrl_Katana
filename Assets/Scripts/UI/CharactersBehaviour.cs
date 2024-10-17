using UnityEngine;
using UnityEngine.UI;

public class CharactersBehaviour : MonoBehaviour
{
    [SerializeField] private Image _redSamuraiImage, _blueSamuraiImage;

    public void SwitchSamuraiPlaces()
    {
        Vector3 samuraiPos = _redSamuraiImage.transform.position;
        _redSamuraiImage.transform.position = _blueSamuraiImage.transform.position;
        _blueSamuraiImage.transform.position = samuraiPos;
    }
}
