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

	private enum DummyTypes
	{
		None,
		Flip,
		Pull,
		Nintey,
		Push,
		Clock // Don't try this one yet
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
		BeginDummyOrder();

		segmentTime = turnSpeed / 3;

		StartNewTurn();
	}

	private void StartNewTurn()
	{
		ShowOrderList();
		StartCoroutine(DoTurn());
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
			if (dummies[i].TryGetComponent<DummyScript>(out DummyScript dummyScript))
			{
				if ((int)dummyArray[i].type <= 0)
				{
					dummyScript.Init(offsetPosition, gridManager, this);
				}
				else
				{
					dummyScript.Init(offsetPosition, gridManager, this, (int)dummyArray[i].type);
				}
			}
		}
	}

	private void BeginDummyOrder()
	{
		for (int i = 0; i < dummies.Length; i++)
		{
			if (dummies[i].TryGetComponent<DummyScript>(out DummyScript dummyScript))
			{
				if (dummyScript.GetOrder() > -1)
				{
					if (orderList.Count == 0)
					{
						orderList.Add(dummies[i]);
					}
					else
					{
						for (int j = 0; j < orderList.Count; j++)
						{
							if (orderList[j].TryGetComponent<DummyScript>(out DummyScript comparisonDummyScript))
							{
								if (dummyScript.GetOrder() < comparisonDummyScript.GetOrder())
								{
									orderList.Insert(j, dummies[i]);
									return;
								}
							}
						}
						orderList.Add(dummies[i]);
					}
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
	}

	public void AddToOrderList(GameObject addition)
	{
		orderList.Add(addition);
	}

	public void RemoveFromOrderList(GameObject addition)
	{
		orderList.Remove(addition);
	}

	private void ShowOrderList()
	{
		for (int i = 0; i < orderList.Count; i++)
		{
			Debug.Log($"Number {i} in the order list is {orderList[i]}");
		}
	}

	public int ReturnOrder(GameObject orderObject)
	{
		return orderList.IndexOf(orderObject);
	}
}
