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

	// Sprites
	[SerializeField] private Sprite[] _orderIndicatorSprites;

	public void Init(Vector2 pos, GridManager grid, GameplayManager game, int type = 0)
	{
		Vector3 posVector = pos;
		posVector.z = 0;
		transform.position = posVector;

		gridManager = grid;
		AddPosToGrid();

		gameplayManager = game;

		if (type > 0)
		{
			locked = true;

			transform.GetChild(1).gameObject.SetActive(true);

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
					if(TryGetComponent<FlipScript>(out FlipScript fScript))
						Destroy(fScript);
					break;
				case 4: // Hori
					if (TryGetComponent<HoriScript>(out HoriScript hScript))
						Destroy(hScript); 
					break;
				case 5: // Vert
					if (TryGetComponent<VertScript>(out VertScript vScript))
						Destroy(vScript);
					break;
			}

			// Turn puppet back to no mask
			gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		}

		if (gameplayManager.InOrderList(this.gameObject))
		{
            RemoveFromOrder();
        }

		_maskValue = type;

		switch (type)
		{
			case 1: // Prince
				gameObject.GetComponent<SpriteRenderer>().color = Color.lightBlue;
				break;
			case 2: // Princess
				gameObject.GetComponent<SpriteRenderer>().color = Color.pink;
				break;
			case 3: // Flip
				gameObject.AddComponent<FlipScript>();
				// This is temporary
				gameObject.GetComponent<SpriteRenderer>().color = Color.red;
				break;
			case 4:
				gameObject.AddComponent<HoriScript>();
				// This is temporary
				gameObject.GetComponent<SpriteRenderer>().color = Color.green;
				break;
			case 5:
				gameObject.AddComponent<VertScript>();
				// This is temporary
				gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
				break;
			default:
				break;
		}
		if (type > 2)
		{
			AddToOrder();
		}
        gameplayManager.UpdateAllOrderIndicators();
    }

	public int GetMask()
	{
		return _maskValue;
	}

	public int GetOrder()
	{
		if (GetMask() == 1)
		{
			return -99;
		}
		else if (GetMask() == 2)
		{
			return 99;
		}
		else
		{
			return gameplayManager.ReturnOrder(gameObject);
		}
	}

	public void RemoveGridPos()
	{
		gridManager.ClearPos((Vector2)transform.position);
	}

	public void AddPosToGrid()
	{
		gridManager.AddToGrid(gameObject);
	}

	public void AddToOrder()
	{
		gameplayManager.AddToOrderList(gameObject);
		UpdateOrderIndicator();
		transform.GetChild(0).gameObject.SetActive(true);
	}

	public void RemoveFromOrder()
	{
		gameplayManager.RemoveFromOrderList(gameObject);
		gameplayManager.UpdateAllOrderIndicators();
		transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
		transform.GetChild(0).gameObject.SetActive(false);
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}

	public void AddToPosition(Vector3 addition)
	{
		transform.position += addition;
	}

	public void SetPosition(Vector3 pos)
	{
		transform.position = pos;
	}

	public void UpdateOrderIndicator()
	{
        if (GetOrder() == -99 || GetOrder() == 99 || GetOrder() == -1)
        {
			return;
        }
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _orderIndicatorSprites[GetOrder() + 1];
    }
}
