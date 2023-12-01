using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class RawFishReset : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private StudioEventEmitter _newFishSound;

    public void ResetAlpha()
    {
        _image.color = new Color(1f, 1f, 1f, 0f);
    }

    public void PlayNewFishSound()
    {
        _newFishSound.Play();
    }
}