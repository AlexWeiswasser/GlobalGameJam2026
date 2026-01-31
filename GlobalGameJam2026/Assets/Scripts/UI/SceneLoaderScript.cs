using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{
    public const float ANIMATION_TIME = 1f;
    private Animator _animator;
    private int _openTrigger;
    private int _closeTrigger;

    private Coroutine _loadSceneCoroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _openTrigger = Animator.StringToHash("OpenScene");
        _closeTrigger = Animator.StringToHash("CloseScene");
        _loadSceneCoroutine = null;
    }

    public void PlayCloseAnimation()
    {
        _animator.SetTrigger(_closeTrigger);
    }

    public void PlayOpenAnimation()
    {
        _animator.SetTrigger(_openTrigger);
    }

    public void LoadScene(string sceneName)
    {
        if (_loadSceneCoroutine == null)
        {
            _loadSceneCoroutine = StartCoroutine(SceneLoader(sceneName));
        }
    }

    private IEnumerator SceneLoader(string sceneName)
    {
        PlayCloseAnimation();
        yield return new WaitForSecondsRealtime(ANIMATION_TIME);
        SceneManager.LoadScene(sceneName);
    }
}
