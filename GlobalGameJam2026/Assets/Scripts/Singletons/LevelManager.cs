using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<int> completedLevels = new List<int>();

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
