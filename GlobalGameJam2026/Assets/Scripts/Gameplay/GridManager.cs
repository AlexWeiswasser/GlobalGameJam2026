using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	// Grid Size
	[SerializeField] private Vector2 size;
	[SerializeField] private GridSizeChanger gridObject;
	[SerializeField] private Camera scaleCam;

	// The grid itself
    private GameObject[,] gridArray;

	// Grid Offset
	private Vector2 CorrectiveOffset = new Vector2(3, 3);

	private void Awake()
	{
        float additionToCamHorizontal = ((size.x - 4) * .1f);
        float additionToCamHeight = ((size.y - 4) * .5f);
		Vector3 newCamPos = scaleCam.transform.position;
		newCamPos.x += additionToCamHorizontal;
		newCamPos.y += additionToCamHeight;
		scaleCam.transform.position = newCamPos;

		gridArray = new GameObject[(int)size.x, (int)size.y];
		gridObject.ChangeGridSize((int)size.x, (int)size.y);
	}

	public bool CheckForWall(Vector2 pos)
	{
		pos += CorrectiveOffset;

		if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0)
		{
			return true;
		}
		return false;
	}
	public GameObject CheckPosition(Vector2 pos)
    {
		pos += CorrectiveOffset;

		if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0) return null;
        return gridArray[(int)pos.x, (int)pos.y];
    }

    public void ClearPos(Vector2 pos)
    {
		pos += CorrectiveOffset;

		if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0) return;
		gridArray[(int)pos.x, (int)pos.y] = null;
	}

	public void AddToGrid(GameObject addition)
	{
		Vector2 pos = (Vector2)addition.transform.position + CorrectiveOffset;

		if (pos.x >= size.x || pos.y >= size.y || pos.x < 0 || pos.y < 0) return;
		gridArray[(int)pos.x, (int)pos.y] = addition;
	}

	public IMoveableFace CheckCross(Vector2 pos)
	{
		IMoveableFace moveableFace = null;

		GameObject[] temps = new GameObject[4];

		temps[0] = CheckPosition(pos + new Vector2(1, 0));
		temps[1] = CheckPosition(pos + new Vector2(-1, 0));
		temps[2] = CheckPosition(pos + new Vector2(0, 1));
		temps[3] = CheckPosition(pos + new Vector2(0, -1));

		moveableFace = WhosOnFirst(temps);

		return moveableFace;
	}

	public IMoveableFace CheckX(Vector2 pos)
	{
		IMoveableFace moveableFace = null;

		GameObject[] temps = new GameObject[4];

		temps[0] = CheckPosition(pos + new Vector2(1, 1));
		temps[1] = CheckPosition(pos + new Vector2(1, -1));
		temps[2] = CheckPosition(pos + new Vector2(-1, 1));
		temps[3] = CheckPosition(pos + new Vector2(-1, -1));

		moveableFace = WhosOnFirst(temps);

		return moveableFace;
	}

	public IMoveableFace CheckCrossInf(Vector2 pos)
	{
		IMoveableFace moveableFace = null;

		List<GameObject> temps = new List<GameObject>();

		GetAll(pos, new Vector2(1, 0), temps);
		GetAll(pos, new Vector2(0, 1), temps);
		GetAll(pos, new Vector2(-1, 0), temps);
		GetAll(pos, new Vector2(0, -1), temps);

		moveableFace = WhosOnFirst(temps);

		return moveableFace;
	}

	private void GetAll(Vector2 beginningPos, Vector2 direction, List<GameObject> possibleCollisions)
	{
		Vector2 pos = beginningPos + direction + direction;
		bool reachedEnd = false;

		while (!reachedEnd)
		{
			if (CheckForWall(pos)) reachedEnd = true;
			GameObject possibleCollision = CheckPosition(pos);
			if(possibleCollision != null)
			{
				possibleCollisions.Add(possibleCollision);
			}
			pos += direction;
		}
	}

	public IMoveableFace CheckVert(Vector2 pos)
	{
		IMoveableFace moveableFace = null;

		GameObject[] temps = new GameObject[4];

		temps[0] = CheckPosition(pos + new Vector2(0, 1));
		temps[1] = CheckPosition(pos + new Vector2(0, -1));

		moveableFace = WhosOnFirst(temps);

		return moveableFace;
	}

	public IMoveableFace CheckHori(Vector2 pos)
	{
		IMoveableFace moveableFace = null;

		GameObject[] temps = new GameObject[4];

		temps[0] = CheckPosition(pos + new Vector2(1, 0));
		temps[1] = CheckPosition(pos + new Vector2(-1, 0));

		moveableFace = WhosOnFirst(temps);

		return moveableFace;
	}

	private IMoveableFace WhosOnFirst(GameObject[] possibleObjects)
	{
		IMoveableFace moveableFace = null;

		for(int i = 0; i < possibleObjects.Length; i++)
		{
			if (possibleObjects[i] != null)
			{
				if (possibleObjects[i].TryGetComponent<IMoveableFace>(out IMoveableFace tempFace))
				{
					if (moveableFace == null)
					{
						moveableFace = tempFace;
					}
					else
					{
						if (tempFace.GetOrder() < moveableFace.GetOrder())
						{
							moveableFace = tempFace;
						}
					}
				}
			}
		}

		return moveableFace;
	}

	private IMoveableFace WhosOnFirst(List<GameObject> possibleObjects)
	{
		IMoveableFace moveableFace = null;

		for (int i = 0; i < possibleObjects.Count; i++)
		{
			if (possibleObjects[i] != null)
			{
				if (possibleObjects[i].TryGetComponent<IMoveableFace>(out IMoveableFace tempFace))
				{
					if (moveableFace == null)
					{
						moveableFace = tempFace;
					}
					else
					{
						if (tempFace.GetOrder() < moveableFace.GetOrder())
						{
							moveableFace = tempFace;
						}
					}
				}
			}
		}

		return moveableFace;
	}
}
