using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private LevelButtonScript levelButtonScript;
    [SerializeField] private AudioClip ButtonHover;
    [SerializeField] private AudioClip ButtonPress;

    public void PlayPressSound()
    {
        MusicManager.Instance.PlaySound(ButtonPress);
    }

    private void Start()
    {
        levelButtonScript = GetComponent<LevelButtonScript>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (levelButtonScript != null && LevelManager.Instance.revealedLevels.Contains(levelButtonScript.GetLevelData().levelID))
        {
            MusicManager.Instance.PlaySound(ButtonHover);
        }
        if (levelButtonScript == null)
        {
            MusicManager.Instance.PlaySound(ButtonHover);
        }
    }
}
