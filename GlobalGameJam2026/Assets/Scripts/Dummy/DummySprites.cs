using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DummySprites", menuName = "Scriptable Objects/DummySprites")]
public class DummySprites : ScriptableObject
{
    public Sprites[] sprites;
    [System.Serializable]
    public struct Sprites
    {
       public Sprite[] sprite;
    }
    
}
