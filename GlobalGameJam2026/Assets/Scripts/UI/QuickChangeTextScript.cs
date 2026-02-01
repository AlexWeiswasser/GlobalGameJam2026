using TMPro;
using UnityEngine;

public class QuickChangeTextScript : MonoBehaviour
{
    void Start()
    {
        if (LevelManager.Instance.currentLevelID == 32)
        {
            GetComponent<TextMeshProUGUI>().text = "The Day of the Ball";
        }
    }
}
