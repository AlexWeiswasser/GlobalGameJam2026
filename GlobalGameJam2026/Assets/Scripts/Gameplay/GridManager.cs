using UnityEngine;

public class GridManager : MonoBehaviour
{
    private GameObject[][] gridArray;

    public GameObject CheckPosition(Vector2 pos)
    {
        return gridArray[(int)pos.x][(int)pos.y];
    }

    public void ClearPos(Vector2 pos)
    {
		gridArray[(int)pos.x][(int)pos.y] = null;
	}

	public void AddToGrid(GameObject addition)
	{
        Vector2 pos = addition.transform.position;
		gridArray[(int)pos.x][(int)pos.y] = addition;
	}
}
