using UnityEngine;

public class MoveWorldMapScript : MonoBehaviour
{
    [SerializeField] private GameObject _prevButton;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _worldMap;
    [SerializeField] private int _pages;
    private int _currentPage = 0;
    private const int MOVE_AMOUNT = 2160; 

    private void Start()
    {
        _prevButton.SetActive(false);
        _worldMap.GetComponent<RectTransform>().anchoredPosition = new Vector2(-LevelManager.Instance.pageWeAreOn * MOVE_AMOUNT, 0);
        CheckButtons();
    }
    public void NextPage()
    {
        _currentPage++;
        LevelManager.Instance.pageWeAreOn = _currentPage;
        _worldMap.GetComponent<RectTransform>().anchoredPosition = new Vector2(-_currentPage * MOVE_AMOUNT, 0);
        CheckButtons();
    }

    public void PrevPage()
    {
        _currentPage--;
        LevelManager.Instance.pageWeAreOn = _currentPage;
        _worldMap.GetComponent<RectTransform>().anchoredPosition = new Vector2(-_currentPage * MOVE_AMOUNT, 0);
        CheckButtons();
    }

    public void CheckButtons()
    {
        if (_currentPage >= _pages)
        {
            _nextButton.SetActive(false);
        }
        else
        {
            _nextButton.SetActive(true);
        }
        if (_currentPage <= 0)
        {
            _prevButton.SetActive(false);
        }
        else
        {
            _prevButton.SetActive(true);
        }
    }
}
