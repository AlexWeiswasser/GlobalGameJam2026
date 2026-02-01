using UnityEngine;

public class xFlipScript : MonoBehaviour, IDummyFace
{
	private IMoveableFace firstFace = null;
	private Vector3 vectorOfTransform = Vector3.zero;

	public void DummyAction(int actionNum, GridManager grid)
	{
		if (actionNum == 1)
		{
			firstFace = grid.CheckX(transform.position);

			if (firstFace != null)
			{
				firstFace.RemoveGridPos();
			}
		}
		else if (actionNum == 2)
		{
			if (firstFace != null)
			{
				if (firstFace.GetOrder() >= 99) // Can't move the prince or table!
				{
					firstFace.AddPosToGrid();
					firstFace = null;
					vectorOfTransform = Vector2.zero;
				}
				else
				{
					vectorOfTransform = transform.position - firstFace.GetPosition();

					if (CanMoveObject(firstFace.GetPosition() + vectorOfTransform + vectorOfTransform, grid))
					{
						firstFace.SetPosition(transform.position);
					}
					else
					{
						firstFace.AddPosToGrid();
						firstFace = null;
						vectorOfTransform = Vector2.zero;
					}
				}
			}
		}
		else if (actionNum == 3)
		{
			if (firstFace != null)
			{
				firstFace.AddToPosition(vectorOfTransform, true);
				firstFace.AddPosToGrid();

				firstFace = null;
				vectorOfTransform = Vector2.zero;
			}

			firstFace = null;
			vectorOfTransform = Vector3.zero;
		}
	}

	private bool CanMoveObject(Vector2 spaceToCheck, GridManager grid)
	{
		if (firstFace.GetMask() != 3) // If not princess, check the position square of what you're moving to
		{
			if (grid.CheckPosition(spaceToCheck) != null)
			{
				return false;
			}
		}
		return (!grid.CheckForWall(spaceToCheck));
	}

	public void SetBackToStart(Vector3 pos)
	{
		firstFace = null;

		if (TryGetComponent<IMoveableFace>(out IMoveableFace moveFace))
		{
			moveFace.RemoveGridPos();
			transform.position = pos;
			moveFace.AddPosToGrid();
		}
	}
}
