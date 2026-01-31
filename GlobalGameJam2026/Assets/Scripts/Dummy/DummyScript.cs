using Unity.VisualScripting;
using UnityEngine;

public class DummyScript : MonoBehaviour, IMoveableFace
{
	// Dummy Data
	private bool locked = false;
	private Vector2 gridPos;
	private int _maskValue = 0;

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

			SetMask(type);
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
                case 1: // Prince
					break;
				case 2: // Princess
					break;
				case 3: // Flip
                    Destroy(GetComponent<FlipScript>());
					// This is temporary
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    break;

            }
        }

        _maskValue = type;

        switch (type)
        {
            case 1:
                gameObject.AddComponent<FlipScript>();
                // This is temporary
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
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
