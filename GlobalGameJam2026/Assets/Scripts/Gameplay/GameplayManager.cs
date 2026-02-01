using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	// Gameplay Stats
	private float turnSpeed = 1f;
	private float segmentTime = 0;

	// Grid Offset
	private int xOffset = -3;
	private int yOffset = -3;

	// Dummy Caching
	[SerializeField] private GameObject dummyPrefab;
	[SerializeField] private Dummy[] dummyArray;
	private GameObject[] dummies;

	// Mask Caching
	[SerializeField] private MaskChoice[] maskChoices;
	[SerializeField] private Vector2[] maskPositions;
	[SerializeField] private GameObject[] maskPrefabs;
	private int _masksPlaced = 0;

	// Dummy Order
	private List<GameObject> orderList = new List<GameObject>();
	private int currentDummy = 0;

	// Other Caching
	[SerializeField] private Canvas canvas;
	[SerializeField] private GameObject winCanvas;

	// Script Caching
	GridManager gridManager;

	// Game Running States
	private bool _isDancing;
	private int _measureCount;
	private Coroutine danceCo = null;

	// Zoomed in Camera
	[SerializeField] public GameObject zoomedCamera;

	// Dummy Sprites
	[SerializeField] private DummySprites danceSprites;
	[SerializeField] private DummySprites idleSprites;
	[SerializeField] private Sprite[] princessDanceSprites;
    [SerializeField] private Sprite[] princeDanceSprites;
    public enum DummyTypes
	{
		None,
		Prince,
		Table,
		Princess,
		Flip,
		Hori,
		Vert,
		Nintey,
		Pull,
		xFlip
	}

	// Timer stuff
	[SerializeField] private TextMeshProUGUI turnsText;
	[SerializeField] private int allowedTurns;
	private int turnsTaken = 0;

	[Serializable]
	private struct Dummy
	{
		public Vector2 position;
		public DummyTypes type;
	}

	[Serializable]
	private struct MaskChoice
	{
		public DummyTypes type;
		public int amount;
	}

	private void Start()
	{
		turnsText.text = $"Turns left: {allowedTurns - turnsTaken}";

		SpawnMasks();
		CreateDummies();

		segmentTime = turnSpeed / 3;
	}

	public void ResetDance()
	{
		_isDancing = false;
		if (danceCo != null)
		{
			StopCoroutine(danceCo);
			danceCo = null;
		}

		canvas.transform.GetChild(5).gameObject.SetActive(false);

        for (int i = 0; i < dummies.Length; i++)
		{
			Vector2 offsetPosition = new Vector2(dummyArray[i].position.x + xOffset, dummyArray[i].position.y + yOffset);
			if (dummies[i].TryGetComponent<IDummyFace>(out IDummyFace dummyFace))
			{
				dummyFace.SetBackToStart(offsetPosition);
			}
			else if (dummies[i].TryGetComponent<DummyScript>(out DummyScript dummyScript))
			{
				dummyScript.SetToStart(offsetPosition);
			}

            ChangeSpriteOfAnimation(0, dummies[i], true);
        }

		_measureCount = 0;
		turnsTaken = 0;
		currentDummy = 0;

        turnsText.text = $"Turns left: {allowedTurns - turnsTaken}";
		turnsText.color = Color.white;
    }

	public void BeginDance()
	{
		if (_masksPlaced < GetMaskChoiceNumber())
		{
			canvas.transform.GetChild(6).gameObject.SetActive(true);
			return;
		}
		canvas.transform.GetChild(0).gameObject.SetActive(true);
        canvas.transform.GetChild(1).gameObject.SetActive(false);
        canvas.transform.GetChild(6).gameObject.SetActive(false);
        _isDancing = true;
        for (int i = 0; i < dummies.Length; i++)
        {
			dummies[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        StartNewTurn();
	}

	private void StartNewTurn()
	{
		_measureCount++;
		turnsTaken++;
		if (turnsTaken > allowedTurns)
		{
            turnsText.color = Color.red;
            StopDance();
			return;
		}
		danceCo = StartCoroutine(DoTurn());
	}

	private IEnumerator DoTurn()
	{
		turnsText.text = $"Turns left: {allowedTurns - turnsTaken}";

		// Animation One
		ActivateDummies(1);
		yield return new WaitForSeconds(segmentTime);

		// Animation Two
		ActivateDummies(2);
		yield return new WaitForSeconds(segmentTime);

		// Animation three
		ActivateDummies(3);
		yield return new WaitForSeconds(segmentTime);

		StartNewTurn();
	}

	private void SpawnMasks()
	{
		int currentMaskSpot = 0;

		for(int i = 0; i < maskChoices.Length; i++)
		{
			for (int j = 0; j < maskChoices[i].amount; j++)
			{
				GameObject newMask = Instantiate(maskPrefabs[(int)maskChoices[i].type - 4], Vector3.zero, Quaternion.identity);
				if(newMask.TryGetComponent<RectTransform>(out RectTransform rTrans))
				{
					newMask.transform.SetParent(canvas.transform, false);
					rTrans.position = maskPositions[currentMaskSpot];
					Vector3 adjustment = new Vector2(Screen.width / 2, Screen.height / 2);
					rTrans.position += adjustment;
				}
				currentMaskSpot++;
			}
		}
	}

	private void CreateDummies()
	{
		gridManager = GetComponent<GridManager>();
		dummies = new GameObject[dummyArray.Length];

		for (int i = 0; i < dummyArray.Length; i++)
		{
			Vector2 offsetPosition = new Vector2(dummyArray[i].position.x + xOffset, dummyArray[i].position.y + yOffset);

			dummies[i] = Instantiate(dummyPrefab, transform.position, Quaternion.identity);
			dummies[i].name = "Dummy" + i;
			
			if (dummies[i].TryGetComponent<DummyScript>(out DummyScript dummyScript))
			{
				dummyScript.Init(offsetPosition, gridManager, this, (int)dummyArray[i].type);
			}

            ChangeSpriteOfAnimation(0, dummies[i], true);
        }
	}

	// Unused because SetMask adds to order
	private void BeginDummyOrder()
	{
		for (int i = 0; i < dummies.Length; i++)
		{
			if (dummies[i].TryGetComponent<DummyScript>(out DummyScript dummyScript))
			{
				if (dummyScript.GetMask() > 1)
				{
					orderList.Add(dummies[i]);
				}
			}
		}
	}
	
	private void ActivateDummies(int currentAction)
	{
		if (currentDummy < orderList.Count)
		{
			if (orderList[currentDummy].TryGetComponent<IDummyFace>(out IDummyFace dummyInterface))
			{
				dummyInterface.DummyAction(currentAction, gridManager);
				orderList[currentDummy].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
            }

			if (currentAction == 3)
			{
                orderList[currentDummy].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                currentDummy++;
				if (currentDummy >= orderList.Count)
				{
					currentDummy = 0;
				}
			}
		}

		if (danceCo == null) return;

		for (int i = 0; i < dummies.Length; i++)
		{
            ChangeSpriteOfAnimation((currentAction - 1) + (3 * ((_measureCount + 1) % 2)), dummies[i], false);
		}
	}

	public void AddToOrderList(GameObject addition)
	{
		orderList.Add(addition);
    }

	public void RemoveFromOrderList(GameObject addition)
	{
		orderList.Remove(addition);
    }

	public void UpdateAllOrderIndicators()
	{
		if (dummies.Length < 1) return;
        for (int i = 0; i < dummies.Length; i++)
        {
			if (dummies[i] != null)
				dummies[i].GetComponent<DummyScript>().UpdateOrderIndicator();
        }
    }

	public bool InOrderList(GameObject dummy)
	{
		return orderList.Contains(dummy);
	}

	private void ChangeSpriteOfAnimation(int index, GameObject dummy, bool idle)
	{
		if (idle)
		{
            dummy.GetComponent<SpriteRenderer>().sprite = idleSprites.sprites[dummy.GetComponent<DummyScript>().GetMask()].sprite[index];
        }
		else
		{
            dummy.GetComponent<SpriteRenderer>().sprite = danceSprites.sprites[dummy.GetComponent<DummyScript>().GetMask()].sprite[index];
        }
	}

	public void WinLevel()
	{
		if (danceCo != null) StopCoroutine(danceCo);
		_measureCount = 1;
        for (int i = 0; i < dummies.Length; i++)
        {
            if (dummies[i].GetComponent<DummyScript>().GetMask() == 3)
            {
				dummies[i].transform.position = new Vector2(dummies[i].transform.position.x + 0.25f, dummies[i].transform.position.y);
            }
            if (dummies[i].GetComponent<DummyScript>().GetMask() == 1)
            {
                dummies[i].transform.position = new Vector2(dummies[i].transform.position.x - 0.25f, dummies[i].transform.position.y);
            }
        }
		canvas.transform.GetChild(0).gameObject.SetActive(false);
        canvas.transform.GetChild(1).gameObject.SetActive(false);
        canvas.transform.GetChild(2).gameObject.SetActive(false);
        WinDance();
		StartCoroutine(EndLevelRoutine());
    }

	public void StopDance()
	{
        if (danceCo != null) StopCoroutine(danceCo);
        danceCo = null;
        canvas.transform.GetChild(5).gameObject.SetActive(true);
    }

	public void WinDance()
	{
        StartCoroutine(WinLevelDanceRoutine());
    }

	IEnumerator WinLevelDanceRoutine()
	{
		GameObject princess = null;
		GameObject prince = null;
		for (int i = 0; i < dummies.Length; i++)
		{
			if (dummies[i].GetComponent<DummyScript>().GetMask() == 3)
			{
				princess = dummies[i];
			}
            if (dummies[i].GetComponent<DummyScript>().GetMask() == 1)
            {
                prince = dummies[i];
            }
        }

		princess.GetComponent<SpriteRenderer>().sprite = princessDanceSprites[0 + (3 * ((_measureCount + 1) % 2))];
        prince.GetComponent<SpriteRenderer>().sprite = princeDanceSprites[0 + (3 * ((_measureCount + 1) % 2))];
        yield return new WaitForSeconds(segmentTime);

        princess.GetComponent<SpriteRenderer>().sprite = princessDanceSprites[1 + (3 * ((_measureCount + 1) % 2))];
        prince.GetComponent<SpriteRenderer>().sprite = princeDanceSprites[1 + (3 * ((_measureCount + 1) % 2))];
        yield return new WaitForSeconds(segmentTime);

        princess.GetComponent<SpriteRenderer>().sprite = princessDanceSprites[2 + (3 * ((_measureCount + 1) % 2))];
        prince.GetComponent<SpriteRenderer>().sprite = princeDanceSprites[2 + (3 * ((_measureCount + 1) % 2))];
        yield return new WaitForSeconds(segmentTime);

        _measureCount++;
        WinDance();
    }

	IEnumerator EndLevelRoutine()
	{
		yield return new WaitForSeconds(segmentTime * 6);
        winCanvas.GetComponent<Animator>().SetTrigger("Win");
    }

	public int ReturnOrder(GameObject orderObject)
	{
		return orderList.IndexOf(orderObject);
	}

	public bool IsDancing()
	{
		return _isDancing;
	}

	public int GetMaskChoiceNumber()
	{
		int amount = 0;
		for (int i = 0; i < maskChoices.Length; i++)
		{
			amount += maskChoices[i].amount;
		}
		return amount;
	}

	public void ChangeMasksPlaced(int amount)
	{
		_masksPlaced += amount;
	}

	public int GetMasksPlaced()
	{
		return _masksPlaced;
	}
}
