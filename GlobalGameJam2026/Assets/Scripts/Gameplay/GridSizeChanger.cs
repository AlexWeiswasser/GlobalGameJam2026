using UnityEngine;

public class GridSizeChanger : MonoBehaviour
{
    // X ranges from -1.5 to 2
    // Y ranges from -1.5 to 0
    public void ChangeGridSize(int width, int height)
    {
        GetComponent<SpriteRenderer>().size = new Vector2(width, height);
        float widthF = (width - 4) / 7f;
        float heightF = (height - 4) / 3f;
        transform.position = new Vector2((3.5f * widthF) - 1.5f, (1.5f * heightF) - 1.5f);
    }
}
