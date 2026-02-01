using UnityEngine;

public class HoriScript : MonoBehaviour, IDummyFace
{
	private IMoveableFace myFace;
	private IMoveableFace firstFace = null;
	private Vector3 vectorOfTransform = Vector2.zero;
	private bool right = true; 

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
			firstFace = grid.CheckVert(transform.position);

			if (firstFace != null)
			{
				firstFace.RemoveGridPos();
			}

			if (right)
			{
				Vector2 newLocation = transform.position + new Vector3(1, 0);
				if (!CanMoveSelf(newLocation, grid))
				{
					right = false;
				}
			}
			else
			{
				Vector2 newLocation = transform.position + new Vector3(-1, 0);
				if (!CanMoveSelf(newLocation, grid))
				{
					right = true;
				}
			}
		}
		else if (actionNum == 2)
		{
			if (right)
			{
				Vector2 newLocation = transform.position + new Vector3(1, 0);
				if (CanMoveSelf(newLocation, grid))
				{
					myFace.RemoveGridPos();
					transform.position = newLocation;
					myFace.AddPosToGrid();
				}
			}
			else
			{
				Vector2 newLocation = transform.position + new Vector3(-1, 0);
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
					if (right)
					{
						Vector2 newLocation = firstFace.GetPosition() + new Vector3(1, 0);
						if (CanMoveObject(newLocation, grid))
						{
							firstFace.SetPosition(newLocation, true);
							firstFace.AddPosToGrid();
						}
						else
						{
							if (TryGetComponent<DummyScript>(out DummyScript dummyScript))
							{
								dummyScript.PlayBump();
							}
							firstFace.AddPosToGrid();
						}
					}
					else
					{
						Vector2 newLocation = firstFace.GetPosition() + new Vector3(-1, 0);
						if (CanMoveObject(newLocation, grid))
						{
							firstFace.SetPosition(newLocation, true);
							firstFace.AddPosToGrid();
						}
						else
						{
							if (TryGetComponent<DummyScript>(out DummyScript dummyScript))
							{
								dummyScript.PlayBump();
							}
							firstFace.AddPosToGrid();
						}
					}

					firstFace = null;
					vectorOfTransform = Vector2.zero;
				}
			}

			firstFace = null;
			vectorOfTransform = Vector2.zero;
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
		if(possibleCollision != null)
		{
			if(possibleCollision.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
			{
				return false; 
			}
		}
		return (!grid.CheckForWall(spaceToCheck));
	}

	public void SetBackToStart(Vector3 pos)
	{
		firstFace = null;
		right = true; 

		if (TryGetComponent<IMoveableFace>(out IMoveableFace moveFace))
		{
			moveFace.RemoveGridPos();
			transform.position = pos;
			moveFace.AddPosToGrid();
		}
	}
}
