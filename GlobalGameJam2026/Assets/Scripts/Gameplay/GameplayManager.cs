using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	// Gameplay Stats
	private float turnSpeed = 1.5f;
	private float segmentTime = 0;

	// Dummy Caching
	[SerializeField] private GameObject dummyPrefab;
	[SerializeField] private Vector2[] dummyArray;
	private GameObject[] dummies;

	private void Start()
	{
		segmentTime = segmentTime / 3;

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

		StartCoroutine(DoTurn());
	}

	private void CreateDummies()
	{
		dummies = new GameObject[dummyArray.Length];

		for(int i = 0; i < dummyArray.Length; i++)
		{
			dummies[i] = Instantiate(dummyPrefab, transform.position, Quaternion.identity);
			if(TryGetComponent<DummyScript>(out DummyScript dummyScript))
			{
				dummyScript.LockToGrid(dummyArray[i]);
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
