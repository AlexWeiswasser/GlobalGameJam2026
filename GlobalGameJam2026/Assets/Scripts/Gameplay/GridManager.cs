using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridManager : MonoBehaviour
{
	// Grid Size
	[SerializeField] private Vector2 size;

	// The grid itself
    private GameObject[,] gridArray;

	private void Awake()
	{
		gridArray = new GameObject[(int)size.x, (int)size.y];
	}

	public GameObject CheckPosition(Vector2 pos)
    {
		if(pos.x >= size.x || pos.y >= size.y) return null;
        return gridArray[(int)pos.x, (int)pos.y];
    }

    public void ClearPos(Vector2 pos)
    {
		if (pos.x >= size.x || pos.y >= size.y) return;
		gridArray[(int)pos.x, (int)pos.y] = null;
	}

	public void AddToGrid(GameObject addition)
	{
		if (addition.transform.position.x >= size.x || addition.transform.position.y >= size.y) return;
		Vector2 pos = addition.transform.position;
		gridArray[(int)pos.x, (int)pos.y] = addition;
	}
}
