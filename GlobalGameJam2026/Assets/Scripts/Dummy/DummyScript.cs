using Unity.VisualScripting;
using UnityEngine;

public class DummyScript : MonoBehaviour, IMoveableFace
{
	// Dummy Data
	private bool locked = false;
	private int orderPos = -1;
	private Vector2 gridPos;
	private int _maskValue = 0;

	// Caching
	private GameplayManager gameplayManager;
	private GridManager gridManager;

	public void Init(Vector2 pos, GridManager grid, GameplayManager game, int type = 0, int order = 0)
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

			SetMask(type);

			orderPos = order;
		}
	}

	public bool IsValidMaskTarget()
	{
		return !locked && (_maskValue == 0);
	}

	public void SetMask(int type)
	{
		if (_maskValue != 0)
		{
            switch (_maskValue)
            {
                case 1:
                    Destroy(GetComponent<FlipScript>());
                    break;
            }
        }

        _maskValue = type;

        switch (type)
        {
            case 1:
                gameObject.AddComponent<FlipScript>();
                break;
			default:
				break;
        }
    }

	public int GetMask()
	{
		return _maskValue;
	}

	public int GetOrder()
	{
		return orderPos;
	}

	public void SetOrder(int newOrder)
	{
		orderPos = newOrder;
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
