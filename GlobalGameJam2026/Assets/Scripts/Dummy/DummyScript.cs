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
	public Sprite princessDeathSprite;

	// Sounds
	[SerializeField] private AudioClip[] dummySounds;
	
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

			if (type > 3)
			{
				transform.GetChild(1).gameObject.SetActive(true);
			}

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
                    GetComponent<SpriteRenderer>().sortingOrder = 0;
                    break;
				case 2: // Table
					break;
				case 3: // Princess
					GetComponent<SpriteRenderer>().sortingOrder = 0;
					break;
				case 4: // Flip
					if(TryGetComponent<FlipScript>(out FlipScript flipScript))			
						Destroy(flipScript);
					break;
				case 5: // Hori
					if (TryGetComponent<HoriScript>(out HoriScript horiScript))
						Destroy(horiScript);
					break;
				case 6: // Vert
					if (TryGetComponent<VertScript>(out VertScript vScript))
						Destroy(vScript);
					break;
				case 7: // Nintey
					if (TryGetComponent<NinteyScript>(out NinteyScript nScript))
						Destroy(nScript);
					break;
				case 8: // Pull
					if (TryGetComponent<PullScript>(out PullScript pScript))
						Destroy(pScript);
					break;
				case 9: // xFlip
					if (TryGetComponent<xFlipScript>(out xFlipScript xScript))
						Destroy(xScript);
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
                    //gameObject.GetComponent<SpriteRenderer>().color = Color.lightBlue;
                GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
			case 2: // Table
				gameObject.GetComponent<SpriteRenderer>().color = Color.black;
				break;
			case 3: // Princess
                GetComponent<SpriteRenderer>().sortingOrder = 4;
                gameObject.AddComponent<PrincessScript>();
				//gameObject.GetComponent<SpriteRenderer>().color = Color.lightPink;
				break;
			case 4: // Flip
				gameObject.AddComponent<FlipScript>();
				gameObject.GetComponent<SpriteRenderer>().color = Color.red;
				break;
			case 5: // Hori
				gameObject.AddComponent<HoriScript>();
				gameObject.GetComponent<SpriteRenderer>().color = Color.green;
				break;
			case 6: // Vert
				gameObject.AddComponent<VertScript>();
				gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
				break;
			case 7: // Nintey
				gameObject.AddComponent<NinteyScript>();
				gameObject.GetComponent<SpriteRenderer>().color = Color.orange;
				break;
			case 8: // Pull
				gameObject.AddComponent<PullScript>();
				gameObject.GetComponent<SpriteRenderer>().color = Color.deepPink;
				break;
			case 9: // xFlip
				gameObject.AddComponent<xFlipScript>();
				gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
				break;

		}
		if (type > 3)
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
		if (GetMask() == 3)
		{
			return -99;
		}
		else if (GetMask() < 3)
		{
			return 99;
		}
		else
		{
			return gameplayManager.ReturnOrder(gameObject);
		}
	}

	public bool GetLocked()
	{
		return locked;
	}

	public GameplayManager GetGameplayManager()
	{
		return gameplayManager;
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

	public void SetToStart(Vector3 pos)
	{
		if (locked && GetMask() > 3)
		{
			transform.GetChild(1).gameObject.SetActive(true);
		}
		RemoveGridPos();
		transform.position = pos;
		AddPosToGrid();
	}

	public void AddToPosition(Vector3 addition, bool finalMove = false)
	{
		if (GetMask() == 3 && finalMove)
		{
			if (GetComponent<PrincessScript>().Slay(gridManager, transform.position + addition))
			{
				transform.position += addition;
				gameplayManager.WinLevel();
                return;
			}
		}

		transform.position += addition;
	}

	public void SetPosition(Vector3 pos, bool finalMove = false)
	{
        if (GetMask() == 3 && finalMove)
		{
			if (GetComponent<PrincessScript>().Slay(gridManager, pos))
			{
                transform.position = pos;
                gameplayManager.WinLevel();
                return;
			}
		}

		transform.position = pos;
	}

	public void UpdateOrderIndicator()
	{
		if(GetOrder() == 99 || GetOrder() == -99 || GetOrder() == -1)
		{
			return;
		}
		transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _orderIndicatorSprites[GetOrder() + 1];
		switch (GetOrder())
		{
			case 0:
				transform.GetChild(0).localPosition = new Vector2(-0.3f, 0.5f);
				break;
            case 1:
                transform.GetChild(0).localPosition = new Vector2(0.3f, 0.5f);
                break;
            case 2:
                transform.GetChild(0).localPosition = new Vector2(-0.3f, -0.1f);
                break;
            case 3:
                transform.GetChild(0).localPosition = new Vector2(0.3f, -0.1f);
                break;
			default:
                transform.GetChild(0).localPosition = new Vector2(-0.3f, 0.5f);
                break;
        }
	}
}
