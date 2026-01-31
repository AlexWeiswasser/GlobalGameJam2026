using UnityEngine;
using UnityEngine.Rendering;

public class NinteyScript : MonoBehaviour, IDummyFace
{
	private IMoveableFace firstFace = null;
	private Vector3[] directions = new Vector3[]
	{
		new Vector3(-1, 0, 0),
		new Vector3(0, 1, 0),
		new Vector3(1, 0, 0),
		new Vector3(0, -1, 0)
	};
	private int arrayDirection;

	public void DummyAction(int actionNum, GridManager grid)
	{
		if (actionNum == 1)
		{
			firstFace = grid.CheckCross(transform.position);

			if (firstFace != null)
			{
				firstFace.RemoveGridPos();

				Vector3 difference = transform.position - firstFace.GetPosition();
				for(int i = 0; i < 4; i++)
				{
					if (directions[i] == difference)
					{
						arrayDirection = i;
					}
				}
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
				}
				else
				{ 
					if (CanMoveObject(transform.position + directions[arrayDirection - 1], grid))
					{
						firstFace.SetPosition(transform.position);
					}
					else
					{
						firstFace = null;
					}
				}
			}
		}
		else if (actionNum == 3)
		{
			if (firstFace != null)
			{
				firstFace.AddToPosition(directions[arrayDirection - 1], true);
				firstFace.AddPosToGrid();

				firstFace = null;
			}

			firstFace = null;
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
}
