using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlipScript : MonoBehaviour, IDummyFace
{
	private GameObject firstInOrder = null;
	private IMoveableFace firstFace = null;
	private Vector3 vectorOfTransform = Vector2.zero;

	public void DummyAction(int actionNum, GridManager grid)
	{
		if (actionNum == 1)
		{
			AreaChecker(new Vector2(1, 0), grid);
			AreaChecker(new Vector2(-1, 0), grid);
			AreaChecker(new Vector2(0, 1), grid);
			AreaChecker(new Vector2(0, -1), grid);

			Debug.Log(GetComponent<DummyScript>().GetOrder() + " is the order and the first in order is " + firstInOrder);
		}
		else if (actionNum == 2)
		{
			if (firstInOrder != null)
			{
				vectorOfTransform = transform.position - firstInOrder.transform.position;
				firstInOrder.transform.position += vectorOfTransform;
			}
		}
		else if (actionNum == 3)
		{
			if (firstInOrder != null)
			{
				firstInOrder.transform.position += vectorOfTransform;
				firstFace.UpdatePositionOnGrid();

				firstInOrder = null;
				firstFace = null;
				vectorOfTransform = Vector2.zero;
			}
		}
	}

	private void AreaChecker(Vector2 addition, GridManager grid)
	{
		Vector2 checkPos = transform.position;
		checkPos += addition;

		if (grid.CheckPosition(checkPos) == null)
		{
			return;
		}
		GameObject possibleObject = grid.CheckPosition(checkPos);
		if (possibleObject != null)
		{
			if (possibleObject.TryGetComponent<IMoveableFace>(out IMoveableFace moveable))
			{
				if (firstInOrder == null)
				{
					firstInOrder = possibleObject;
					firstFace = moveable;
				}
				else
				{
					if (moveable.GetOrder() > firstFace.GetOrder())
					{
						firstInOrder = possibleObject;
						firstFace = moveable;
					}
				}
			}
		}
	}
}
