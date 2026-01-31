using UnityEngine;

public class GridManager : MonoBehaviour
{
    private GameObject[][] gridArray;

    public GameObject CheckPosition(Vector2 pos)
    {
        return gridArray[(int)pos.x][(int)pos.y];
    }
	public GameObject CheckPosition(int x, int y)
	{
		return gridArray[x][y];
	}

    public void ClearPos(Vector2 pos)
    {
		gridArray[(int)pos.x][(int)pos.y] = null;
	}

    public void AddToPosition()
    {

    }
}
