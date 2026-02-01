using UnityEngine;

public class VertScript : MonoBehaviour, IDummyFace
{
	private IMoveableFace myFace;
	private IMoveableFace firstFace = null;
	private Vector3 vectorOfTransform = Vector3.zero;
	private bool up = true;

	public void DummyAction(int actionNum, GridManager grid)
	{
		if (TryGetComponent<IMoveableFace>(out IMoveableFace moveFace))
		{
			myFace = moveFace;
		}
		else
		{
			return;
		}

		if (actionNum == 1)
		{
			firstFace = grid.CheckHori(transform.position);

			if (firstFace != null)
			{
				firstFace.RemoveGridPos();
			}

			if (up)
			{
				Vector2 newLocation = transform.position + new Vector3(0, 1);
				if (!CanMoveSelf(newLocation, grid))
				{
					up = false;
				}
			}
			else
			{
				Vector2 newLocation = transform.position + new Vector3(0, -1);
				if (!CanMoveSelf(newLocation, grid))
				{
					up = true;
				}
			}
		}
		else if (actionNum == 2)
		{ 
			if (up)
			{
				Vector2 newLocation = transform.position + new Vector3(0, 1);
				if (CanMoveSelf(newLocation, grid))
				{
					myFace.RemoveGridPos();
					transform.position = newLocation;
					myFace.AddPosToGrid();
				}
			}
			else
			{
				Vector2 newLocation = transform.position + new Vector3(0, -1);
				if (CanMoveSelf(newLocation, grid))
				{
					myFace.RemoveGridPos();
					transform.position = newLocation;
					myFace.AddPosToGrid();
				}
			}
		}
		else if (actionNum == 3)
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
					if (up)
					{
						Vector2 newLocation = firstFace.GetPosition() + new Vector3(0, 1);
						if (CanMoveObject(newLocation, grid))
						{
							firstFace.SetPosition(newLocation, true);
							firstFace.AddPosToGrid();
						}
						else
						{
							firstFace.AddPosToGrid();
						}
					}
					else
					{
						Vector2 newLocation = firstFace.GetPosition() + new Vector3(0, -1);
						if (CanMoveObject(newLocation, grid))
						{
							firstFace.SetPosition(newLocation, true);
							firstFace.AddPosToGrid();
						}
						else
						{
							firstFace.AddPosToGrid();
						}
					}

					firstFace = null;
					vectorOfTransform = Vector2.zero;
				}
			}

			firstFace = null;
			vectorOfTransform = Vector3.zero;
		}
	}

	private bool CanMoveObject(Vector2 spaceToCheck, GridManager grid)
	{
		if (firstFace.GetMask() != 3) // If not princess, check the position square of what you're moving to
		{
			if (grid.CheckPosition(spaceToCheck) != null) return false;
		}
		return (!grid.CheckForWall(spaceToCheck));
	}

	private bool CanMoveSelf(Vector2 spaceToCheck, GridManager grid)
	{
		GameObject possibleCollision = null;
		possibleCollision = grid.CheckPosition(spaceToCheck);
		if (possibleCollision != null)
		{
			if (possibleCollision.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				if (moveableFace.GetMask() != 2)
				{
					return false;
				}
			}
		}
		return (!grid.CheckForWall(spaceToCheck));
	}

	public void SetBackToStart(Vector3 pos)
	{
		firstFace = null;
		up = true;

		if (TryGetComponent<IMoveableFace>(out IMoveableFace moveFace))
		{
			moveFace.RemoveGridPos();
			transform.position = pos;
			moveFace.AddPosToGrid();
		}
	}
}
