using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelButtonScript : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private SceneLoaderScript _sceneLoaderScript;
    private Image _image;
    private Button _button;
    void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        if (LevelManager.Instance.IsLevelCompleted(_levelData.levelID))
        {
            _image.color = new Color(0.89f, 0.09f, 0.80f);
            _button.onClick.AddListener(LoadThatScene);
        }
        else
        {
            bool isUnlocked = false;
            if (_levelData.requiredLevelCompletions != 0)
            {
                int levelsCompleted = 0;
                for (int i = 0; i < _levelData.requiredLevelCompletions; i++)
                {
                    if (LevelManager.Instance.IsLevelCompleted(_levelData.requiredLevelIDs[i]))
                    {
                        levelsCompleted++;
                    }
                }

                if (levelsCompleted >= _levelData.requiredLevelCompletions)
                {
                    isUnlocked = true;
                }
            }
            else
            {
                isUnlocked = true;
            }

            if (isUnlocked)
            {
                _image.color = new Color(1f, 1f, 1f);
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
                _button.onClick.AddListener(LoadThatScene);
            }
            else
            {
                _image.color = new Color(0f, 0f, 0f);
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0f, 0f, 0f);
            }
        }
    }

    private void LoadThatScene()
    {
        LevelManager.Instance.levelWeAreLoading = _levelData.sceneName;
        _sceneLoaderScript.LoadScene("Menu_Inbetween");
    }
}
