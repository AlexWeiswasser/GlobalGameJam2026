using System.Collections;
using UnityEngine;

public class FinalScene : MonoBehaviour
{
    bool locked = false;
    private Animator _animator;
    [SerializeField] private AudioClip _kiss;
    [SerializeField] private SceneLoaderScript _loaderScript;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayOut()
    {
        if (locked) return;
        locked = true;
        _animator.SetTrigger("Play");
        StartCoroutine(SyncCutscene());
    }

    IEnumerator SyncCutscene()
    {
        yield return new WaitForSeconds(7f);

        MusicManager.Instance.PlaySound(_kiss);

        yield return new WaitForSeconds(3f);

        _loaderScript.LoadScene("Menu_WinScene");
    }
}
