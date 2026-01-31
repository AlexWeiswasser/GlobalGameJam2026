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
		pos += offset;

		IMoveableFace moveableObject = null;
		GameObject temp = null;

		temp = CheckPosition(pos + new Vector2(1, 0));
		if(temp != null) 
		{
			if (temp.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				moveableObject = moveableFace;
			}
		}
		temp = CheckPosition(pos + new Vector2(-1, 0));
		if (temp != null)
		{
			if (temp.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				if (moveableFace.GetOrder() < moveableObject.GetOrder())
				{
					moveableObject = moveableFace;
				}
			}
		}
		temp = CheckPosition(pos + new Vector2(1, 0));
		if (temp != null)
		{
			if (temp.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				if (moveableFace.GetOrder() < moveableObject.GetOrder())
				{
					moveableObject = moveableFace;
				}
			}
		}
		temp = CheckPosition(pos + new Vector2(1, 0));
		if (temp != null)
		{
			if (temp.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				if (moveableFace.GetOrder() < moveableObject.GetOrder())
				{
					moveableObject = moveableFace;
				}
			}
		}

		return moveableObject;
	}

	public IMoveableFace CheckVert(Vector2 pos)
	{
		pos += offset;

		IMoveableFace moveableObject = null;
		GameObject temp = null;

		temp = CheckPosition(pos + new Vector2(1, 0));
		if (temp != null)
		{
			if (temp.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				moveableObject = moveableFace;
			}
		}
		temp = CheckPosition(pos + new Vector2(-1, 0));
		if (temp != null)
		{
			if (temp.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				if (moveableFace.GetOrder() < moveableObject.GetOrder())
				{
					moveableObject = moveableFace;
				}
			}
		}

		return moveableObject;
	}

	public IMoveableFace CheckHori(Vector2 pos)
	{
		pos += offset;

		IMoveableFace moveableObject = null;
		GameObject temp = null;

		temp = CheckPosition(pos + new Vector2(1, 0));
		if (temp != null)
		{
			if (temp.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				if (moveableFace.GetOrder() < moveableObject.GetOrder())
				{
					moveableObject = moveableFace;
				}
			}
		}
		temp = CheckPosition(pos + new Vector2(1, 0));
		if (temp != null)
		{
			if (temp.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				if (moveableFace.GetOrder() < moveableObject.GetOrder())
				{
					moveableObject = moveableFace;
				}
			}
		}

		return moveableObject;
	}
}
