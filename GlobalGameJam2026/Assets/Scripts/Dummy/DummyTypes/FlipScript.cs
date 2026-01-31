using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlipScript : MonoBehaviour, IDummyFace
{
	private IMoveableFace firstFace = null;
	private Vector3 vectorOfTransform = Vector2.zero;

	public void DummyAction(int actionNum, GridManager grid)
	{
		if (actionNum == 1)
		{
			firstFace = grid.CheckCross(transform.position);

			if (firstFace != null)
			{
				firstFace.RemoveGridPos();
			}
		}
		else if (actionNum == 2)
		{
			if (firstFace != null)
			{
				if(firstFace.GetMask() == 1) // Can't move the prince!
				{
					firstFace = null;
					vectorOfTransform = Vector2.zero;
				}

				vectorOfTransform = transform.position - firstFace.GetPosition();

				if (FilledSpaceCheck(firstFace.GetPosition() + vectorOfTransform + vectorOfTransform, grid))
				{
					firstFace = null;
					vectorOfTransform = Vector2.zero;
				}
				else
				{
					firstFace.AddToPosition(vectorOfTransform);
				}
			}
		}
		else if (actionNum == 3)
		{
			if (firstFace != null)
			{
				firstFace.AddToPosition(vectorOfTransform);
				firstFace.AddPosToGrid();

				firstFace = null;
				vectorOfTransform = Vector2.zero;
			}
		}
	}

	private bool FilledSpaceCheck(Vector2 spaceToCheck, GridManager grid)
	{
		if (firstFace.GetMask() != 2) // If not princess, check the position square of what you're moving to
		{
			if (grid.CheckPosition(spaceToCheck) != null) return true;
		}
		return (grid.CheckForWall(spaceToCheck));
	}
}
