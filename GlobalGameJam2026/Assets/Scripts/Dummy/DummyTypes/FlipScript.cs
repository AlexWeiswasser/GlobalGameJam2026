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
			for (int i = -1; i < 1; i++)
			{
				Vector2 checkPos = transform.position;
				checkPos.x += i;

				if(grid.CheckPosition(checkPos) == null)
				{
					continue;
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

			for (int i = -1; i < 1; i++)
			{
				Vector2 checkPos = transform.position;
				checkPos.y += i;

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

				firstInOrder = null;
				firstFace = null;
				vectorOfTransform = Vector2.zero;
			}
		}
	}
}
