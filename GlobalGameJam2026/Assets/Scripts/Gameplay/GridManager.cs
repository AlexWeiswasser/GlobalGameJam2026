using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridManager : MonoBehaviour
{
	// Grid Size
	[SerializeField] private Vector2 size;

	// The grid itself
    private GameObject[,] gridArray;

	// Grid Offset
	private Vector2 offset = new Vector2(3, 3);

	private void Awake()
	{
		gridArray = new GameObject[(int)size.x, (int)size.y];
	}

	public bool CheckForWall(Vector2 pos)
	{
		pos += offset;

		if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0) return true;
		return false;
	}
	public GameObject CheckPosition(Vector2 pos)
    {
		pos += offset;

		if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0) return null;
        return gridArray[(int)pos.x, (int)pos.y];
    }

    public void ClearPos(Vector2 pos)
    {
		pos += offset;

		if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0) return;
		gridArray[(int)pos.x, (int)pos.y] = null;
	}

	public void AddToGrid(GameObject addition)
	{
		Vector2 pos = (Vector2)addition.transform.position + offset;

		if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0) return;
		gridArray[(int)pos.x, (int)pos.y] = addition;
	}

	public IMoveableFace CheckCross(Vector2 pos)
	{
		IMoveableFace moveableFace = null;
		GameObject temp = null;

		temp = CheckPosition(pos + new Vector2(1, 0));
		moveableFace = WhosOnFirst(temp, moveableFace);
		temp = CheckPosition(pos + new Vector2(-1, 0));
		moveableFace = WhosOnFirst(temp, moveableFace);
		temp = CheckPosition(pos + new Vector2(0, 1));
		moveableFace = WhosOnFirst(temp, moveableFace);
		temp = CheckPosition(pos + new Vector2(0, -1));
		moveableFace = WhosOnFirst(temp, moveableFace);

		return moveableFace;
	}

	public IMoveableFace CheckVert(Vector2 pos)
	{
		IMoveableFace moveableFace = null;
		GameObject temp = null;

		temp = CheckPosition(pos + new Vector2(0, 1));
		moveableFace = WhosOnFirst(temp, moveableFace);
		temp = CheckPosition(pos + new Vector2(0, -1));
		moveableFace = WhosOnFirst(temp, moveableFace);

		return moveableFace;
	}

	public IMoveableFace CheckHori(Vector2 pos)
	{
		IMoveableFace moveableFace = null;
		GameObject temp = null;

		temp = CheckPosition(pos + new Vector2(1, 0));
		moveableFace = WhosOnFirst(temp, moveableFace);
		temp = CheckPosition(pos + new Vector2(-1, 0));
		moveableFace = WhosOnFirst(temp, moveableFace);

		return moveableFace;
	}

	private IMoveableFace WhosOnFirst(GameObject temp, IMoveableFace moveableFace)
	{
		if (temp != null)
		{
			if (temp.TryGetComponent<IMoveableFace>(out IMoveableFace currentFace))
			{
				if (moveableFace != null)
				{
					if (currentFace.GetOrder() < moveableFace.GetOrder())
					{
						return currentFace;
					}
					else
					{
						return moveableFace;
					}
				}
				else
				{
					return currentFace;
				}
			}
			else
			{
				if (moveableFace != null)
				{
					return moveableFace;
				}
				else
				{
					return null;
				}
			}
		}
		if (moveableFace != null)
		{
			return moveableFace;
		}
		else
		{
			return null;
		}
	}
}
