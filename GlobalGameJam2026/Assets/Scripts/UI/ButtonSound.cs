using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip ButtonHover;
    [SerializeField] private AudioClip ButtonPress;

    public void PlayPressSound()
    {
        MusicManager.Instance.PlaySound(ButtonPress);
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        MusicManager.Instance.PlaySound(ButtonHover);

    }
}
