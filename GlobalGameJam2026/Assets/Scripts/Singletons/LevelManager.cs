using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<int> completedLevels = new List<int>();
    public List<int> revealedLevels = new List<int>();
    [HideInInspector] public string levelWeAreLoading;
    [HideInInspector] public int currentLevelID;
    public int pageWeAreOn;
    [SerializeField] private GameObject _worldMap;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            pageWeAreOn = 0;
        }
    }

    public void CompletedLevel(int levelID)
    {
        if (!completedLevels.Contains(levelID))
        {
            completedLevels.Add(levelID);
        }
    }

    public bool IsLevelCompleted(int levelID)
    {
        return completedLevels.Contains(levelID);
    }
}
