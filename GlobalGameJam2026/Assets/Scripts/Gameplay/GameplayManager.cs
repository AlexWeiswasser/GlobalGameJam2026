using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	// Gameplay Stats
	private float turnSpeed = 1.5f;
	private float segmentTime = 0;

	// Dummy Caching
	[SerializeField] private GameObject dummyPrefab;
	[SerializeField] private Dummy[] dummyArray;
	private GameObject[] dummies;

	private enum DummyTypes
	{
		None,
		Flip,
		Pull,
		Nintey,
		Push,
		Clock
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

		StartNewTurn();
	}

	private void StartNewTurn()
	{
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
		dummies = new GameObject[dummyArray.Length];
		int orderTracker = 0;

		for(int i = 0; i < dummyArray.Length; i++)
		{
			dummies[i] = Instantiate(dummyPrefab, transform.position, Quaternion.identity);
			if(dummies[i].TryGetComponent<DummyScript>(out DummyScript dummyScript))
			{
				dummyScript.Init(dummyArray[i].position, (int)dummyArray[i].type, orderTracker);
				orderTracker++;
			}
		}
	}

	private void ActivateDummies(int currentAction)
	{ 
		for(int i = 0; i < dummies.Length; i++)
		{
			if (dummies[i].TryGetComponent<IDummyFace>(out IDummyFace dummyInterface))
			{
				dummyInterface.DummyAction(currentAction);
			}
		}
	}
}
