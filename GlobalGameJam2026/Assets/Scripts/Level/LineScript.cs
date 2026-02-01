using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    [SerializeField] private LevelData _levelICameFrom;
    [SerializeField] private LevelData _levelIGoTo;
    private List<GameObject> _lineParts;

    void Start()
    {
        _lineParts = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            _lineParts.Add(transform.GetChild(i).gameObject);
        }

        bool anyLevelCompleted = false;
        for (int i = 0; i < _levelIGoTo.requiredLevelIDs.Length; i++)
        {
            if (LevelManager.Instance.completedLevels.Contains(_levelIGoTo.requiredLevelIDs[i]))
            {
                anyLevelCompleted = true;
            }
        }

        if (anyLevelCompleted)
        {
            if (LevelManager.Instance.IsLevelCompleted(_levelIGoTo.levelID) || LevelManager.Instance.revealedLevels.Contains(_levelIGoTo.levelID))
            {
                foreach (GameObject part in _lineParts)
                {
                    part.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject part in _lineParts)
                {
                    part.SetActive(false);
                }
                StartCoroutine(RevealLine());
            }
        }
        else
        {
            foreach (GameObject part in _lineParts)
            {
                part.SetActive(false);
            }
        }
    }

    IEnumerator RevealLine()
    {
        yield return new WaitForSeconds(1f);
        float timeToWait = 1f / _lineParts.Count;
        int linePart = 0;
        int levelsCompleted = 0;
        for (int i = 0; i < _levelIGoTo.requiredLevelIDs.Length; i++)
        {
            if (LevelManager.Instance.completedLevels.Contains(_levelIGoTo.requiredLevelIDs[i]))
            {
                levelsCompleted++;
            }
        }

        foreach (GameObject part in _lineParts)
        {
            if((linePart < levelsCompleted) || levelsCompleted >= _levelIGoTo.requiredLevelCompletions)
            {
                part.SetActive(true);
            }
            else
            {
                part.SetActive(false);
            }
            linePart++;
            yield return new WaitForSeconds(timeToWait);
        }
    }
}
