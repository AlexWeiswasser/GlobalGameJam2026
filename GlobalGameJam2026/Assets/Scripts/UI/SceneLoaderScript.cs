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

    private bool _isLocked = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _openTrigger = Animator.StringToHash("OpenScene");
        _closeTrigger = Animator.StringToHash("CloseScene");
        _loadSceneCoroutine = null;
        _isLocked = true;
    }

    private void Start()
    {
        StartCoroutine(UnlockOnStart());
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
        if (_loadSceneCoroutine == null && !_isLocked)
        {
            Debug.Log("Loading");
            _loadSceneCoroutine = StartCoroutine(SceneLoader(sceneName));
        }
    }

    private IEnumerator SceneLoader(string sceneName)
    {
        PlayCloseAnimation();
        yield return new WaitForSeconds(ANIMATION_TIME);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator UnlockOnStart()
    {
        yield return new WaitForSeconds(ANIMATION_TIME);
        Debug.Log("Unlock");
        _isLocked = false;
    }
}
