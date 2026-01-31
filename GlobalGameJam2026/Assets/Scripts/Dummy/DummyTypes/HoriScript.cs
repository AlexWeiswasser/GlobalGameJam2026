using UnityEngine;

public class HoriScript : MonoBehaviour, IDummyFace
{
	private IMoveableFace firstFace = null;
	private Vector3 vectorOfTransform = Vector2.zero;
	private bool right = true; 

	public void DummyAction(int actionNum, GridManager grid)
	{
		if (actionNum == 1)
		{
			firstFace = grid.CheckVert(transform.position);

			if (firstFace != null)
			{
				firstFace.RemoveGridPos();
			}

			if(right)
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
			if(right)
			{
				Vector2 newLocation = transform.position + new Vector3(1, 0);
				if(CanMoveSelf(newLocation, grid))
				{
					transform.position = newLocation;
				}
			}
			else
			{
				Vector2 newLocation = transform.position + new Vector3(-1, 0);
				if(CanMoveSelf(newLocation, grid))
				{
					transform.position = newLocation;
				}
			}
		}
		else if (actionNum == 3)
		{
			if (firstFace != null)
			{
				if (right)
				{
					Vector2 newLocation = firstFace.GetPosition() + new Vector3(1, 0);
					if (CanMoveObject(newLocation, grid))
					{
						firstFace.SetPosition(newLocation);
						firstFace.AddPosToGrid();
					}
					else
					{
						firstFace.AddPosToGrid();
					}
				}
				else
				{
					Vector2 newLocation = firstFace.GetPosition() + new Vector3(-1, 0);
					if (CanMoveObject(newLocation, grid))
					{
						firstFace.SetPosition(newLocation);
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
	}

	private bool CanMoveObject(Vector2 spaceToCheck, GridManager grid)
	{
		if (firstFace.GetMask() != 2) // If not princess, check the position square of what you're moving to
		{
			if (grid.CheckPosition(spaceToCheck) != null) return true;
		}
		return (grid.CheckForWall(spaceToCheck));
	}

	private bool CanMoveSelf(Vector2 spaceToCheck, GridManager grid)
	{
		GameObject possibleCollision = null;
		if(possibleCollision = grid.CheckPosition(spaceToCheck))
		{
			if(possibleCollision != null)
			{
				if(possibleCollision.TryGetComponent<IMoveableFace>(out IMoveableFace moveableFace))
				{
					if(moveableFace.GetMask() != 2)
					{
						return false; 
					}
				}
			}
		}
		return (grid.CheckForWall(spaceToCheck));
	}
}
