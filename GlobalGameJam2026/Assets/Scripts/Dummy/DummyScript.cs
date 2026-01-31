using UnityEngine;

public class DummyScript : MonoBehaviour, IMoveableFace
{
	// Dummy Data
	private bool locked = false;
	private int orderPos = -1;
	private Vector2 gridPos;

	// Caching
	private GridManager gridManager;

	public void Init(Vector2 pos, GridManager grid, int type = 0, int order = 0)
    {
		Vector3 posVector = pos;
		posVector.z = 0;
		transform.position = posVector;

		gridManager = grid;
		UpdatePositionOnGrid();

		if (type > 0)
		{
			locked = true;

			switch (type)
			{
				case 1:
					gameObject.AddComponent<FlipScript>();
					break;
			}

			orderPos = order;
		}
	}

	public int GetOrder()
	{
		return orderPos;
	}

	public void UpdatePositionOnGrid()
	{
		gridManager.ClearPos((Vector2)transform.position);
		gridManager.AddToGrid(gameObject);
	}
}
