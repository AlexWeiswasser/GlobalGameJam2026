using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	// Gameplay Stats
	private float turnSpeed = 1.5f;
	private float segmentTime = 0;

	// Grid Offset
	private int xOffset = -3;
	private int yOffset = -3;

	// Dummy Caching
	[SerializeField] private GameObject dummyPrefab;
	[SerializeField] private Dummy[] dummyArray;
	private GameObject[] dummies;

	// Dummy Order
	private List<GameObject> orderList = new List<GameObject>();
	private int currentDummy = 0;

	// Script Caching
	GridManager gridManager;

	// Game Running States
	private bool _isDancing;
	private int _measureCount;
	private Coroutine danceCo = null;

	// Dummy Sprites
	[SerializeField] private DummySprites danceSprites;
	[SerializeField] private DummySprites idleSprites;

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

	[Serializable]
	private struct Dummy
	{
		public Vector2 position;
		public DummyTypes type;
	}

	private void Start()
	{
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

		for(int i = 0; i < dummies.Length; i++)
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
		}

		_measureCount = 0;
		currentDummy = 0;
	}

	public void BeginDance()
	{
		_isDancing = true;
		StartNewTurn();
	}

	private void StartNewTurn()
	{
		_measureCount++;
		danceCo = StartCoroutine(DoTurn());
	}

	private IEnumerator DoTurn()
	{
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
			}

			if (currentAction == 3)
			{
				currentDummy++;
				if (currentDummy >= orderList.Count)
				{
					currentDummy = 0;
				}
			}
		}

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
		Debug.Log(dummy.GetComponent<DummyScript>().GetMask());
	}

	public void WinLevel()
	{

	}

	public int ReturnOrder(GameObject orderObject)
	{
		return orderList.IndexOf(orderObject);
	}

	public bool IsDancing()
	{
		return _isDancing;
	}
}
