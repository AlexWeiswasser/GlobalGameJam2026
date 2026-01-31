using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAfterASecond : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadSoon());
    }
    
    IEnumerator LoadSoon()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<SceneLoaderScript>().LoadScene(LevelManager.Instance.levelWeAreLoading);
    }
}
