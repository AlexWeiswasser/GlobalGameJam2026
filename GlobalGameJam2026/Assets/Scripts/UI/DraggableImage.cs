using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 _startPosition;
    private Vector2 _imageSize;
    private Vector2 _imageScale;
    private GameplayManager _gameplayManager;
    [SerializeField] private int maskNumber;
    private DummyScript _previousDummy;
    private Camera _zoomedCamera;
    private Image _icon;

    private void Start()
    {
        _gameplayManager = Camera.main.GetComponent<GameplayManager>();
        _zoomedCamera = _gameplayManager.zoomedCamera.GetComponent<Camera>();
        _startPosition = GetComponent<RectTransform>().anchoredPosition;
        _imageSize = GetComponent<RectTransform>().sizeDelta;
        _imageScale = transform.localScale;
        _icon = transform.GetChild(0).GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_gameplayManager.IsDancing()) return;
        // Return Image Size to Normal
        transform.SetAsLastSibling();
        GetComponent<RectTransform>().sizeDelta = _imageSize;
        Color color = GetComponent<Image>().color;
        color.a = 1;
        GetComponent<Image>().color = color;
        color = _icon.color;
        color.a = 1;
        _icon.color = color;
        transform.localScale = _imageScale;
        
        if (_previousDummy != null)
        {
            _previousDummy.SetMask(0);
            _gameplayManager.ChangeMasksPlaced(-1);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_gameplayManager.IsDancing()) return;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(Mouse.current.position.ReadValue().x - 960, Mouse.current.position.ReadValue().y - 540);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_gameplayManager.IsDancing()) return;
        bool successful = false;

        Ray ray = _zoomedCamera.ScreenPointToRay(new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        Vector2 mousePos = _zoomedCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            hit.collider.gameObject.TryGetComponent<DummyScript>(out DummyScript dummyScript);
            if (dummyScript != null)
            {
                if (dummyScript.GetMask() == 0)
                {
                    successful = true;
                }
            }
        }
        if (successful)
        {
            _gameplayManager.ChangeMasksPlaced(1);
            hit.collider.gameObject.TryGetComponent<DummyScript>(out DummyScript dummyScript);
            if (_previousDummy != null)
            {
                _previousDummy.SetMask(0);
            }
            _previousDummy = dummyScript;
            GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(_zoomedCamera.WorldToScreenPoint(hit.collider.gameObject.transform.position).x - 960, _zoomedCamera.WorldToScreenPoint(hit.collider.gameObject.transform.position).y - 540);
            Color color = GetComponent<Image>().color;
            color.a = 0;
            transform.localScale = new Vector2(1,1);
            GetComponent<Image>().color = color;
            color = _icon.color;
            color.a = 0;
            _icon.color = color;
            dummyScript.SetMask(maskNumber);
        }
        else
        {
            if (_previousDummy != null)
            {
                _previousDummy.SetMask(0);
            }
            _previousDummy = null;
            GetComponent<RectTransform>().anchoredPosition = _startPosition;
            GetComponent<RectTransform>().sizeDelta = _imageSize;
            transform.localScale = _imageScale;
        }
    }
}
