using UnityEngine;

public class PrincessScript : MonoBehaviour
{
   public bool Slay(GridManager grid, Vector2 pos)
	{
		GameObject possibleCollision = grid.CheckPosition(pos);
		if (possibleCollision != null)
		{
			if (possibleCollision.TryGetComponent<IMoveableFace>(out IMoveableFace moveFace))
			{
				if (moveFace.GetMask() == 1)
				{
					Debug.Log("You win!!!!111!!!");
				}
				else
				{
					Debug.Log("You DIEEE!!!!");
				}
				return true;
			}
		}
		return false; 
	}
}
