using UnityEngine;

public class DummyScript : MonoBehaviour, IMoveableFace
{
	// Dummy Data
	private bool locked = false;
	private Vector2 gridPos;

	// Caching
	private GameplayManager gameplayManager;
	private GridManager gridManager;

	public void Init(Vector2 pos, GridManager grid, GameplayManager game, int type = 0)
    {
		Vector3 posVector = pos;
		posVector.z = 0;
		transform.position = posVector;

		gridManager = grid;
		AddGridPos();

		gameplayManager = game;

		if (type > 0)
		{
			locked = true;

			switch (type)
			{
				case 1:
					gameObject.AddComponent<FlipScript>();
					break;
			}
		}
	}

	public int GetOrder()
	{
		return gameplayManager.ReturnOrder(gameObject);
	}

	public void RemoveGridPos()
	{
		gridManager.ClearPos((Vector2)transform.position);
	}

	public void AddGridPos()
	{
		gridManager.AddToGrid(gameObject);
	}

	public void AddToOrder()
	{
		gameplayManager.AddToOrderList(gameObject);
	}

	public void RemoveFromOrder()
	{
		gameplayManager.RemoveFromOrderList(gameObject);
	}
}
