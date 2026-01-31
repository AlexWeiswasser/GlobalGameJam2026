using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public string sceneName;
    public int requiredLevelCompletions;
    public int[] requiredLevelIDs;
}
