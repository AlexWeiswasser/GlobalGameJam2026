using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 _startPosition;
    private Vector2 _imageSize;
    [SerializeField] private int maskNumber;

    private void Start()
    {
        _startPosition = GetComponent<RectTransform>().anchoredPosition;
        _imageSize = GetComponent<RectTransform>().sizeDelta;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Return Image Size to Normal
        GetComponent<RectTransform>().sizeDelta = _imageSize;
        Color color = GetComponent<Image>().color;
        color.a = 1;
        GetComponent<Image>().color = color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(Mouse.current.position.ReadValue().x - 960, Mouse.current.position.ReadValue().y - 540);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool successful = false;
        // Create a ray from the main camera through the current mouse position
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y));
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
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
            Debug.Log("Hit 2D object: " + hit.collider.name);
        }
        if (successful)
        {
            Debug.Log("Drag Successful");
            GetComponent<RectTransform>().sizeDelta = new Vector2(110, 110);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(Camera.main.WorldToScreenPoint(hit.collider.gameObject.transform.position).x - 960, Camera.main.WorldToScreenPoint(hit.collider.gameObject.transform.position).y - 540);
            Color color = GetComponent<Image>().color;
            color.a = 0;
            GetComponent<Image>().color = color;
            hit.collider.gameObject.TryGetComponent<DummyScript>(out DummyScript dummyScript);
            dummyScript.SetMask(maskNumber);
        }
        else
        {
            Debug.Log("Drag Unsuccessful");
            GetComponent<RectTransform>().anchoredPosition = _startPosition;
            GetComponent<RectTransform>().sizeDelta = _imageSize;
        }
    }
}
