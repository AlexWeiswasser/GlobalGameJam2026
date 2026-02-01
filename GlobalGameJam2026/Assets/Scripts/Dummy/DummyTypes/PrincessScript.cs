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
					GameObject.Find("WinCanvas").GetComponent<Animator>().SetTrigger("Win");
					LevelManager.Instance.CompletedLevel(LevelManager.Instance.currentLevelID);
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

	public void SetBackToStart(Vector3 pos)
	{
		if (TryGetComponent<IMoveableFace>(out IMoveableFace moveFace))
		{
			moveFace.RemoveGridPos();
			transform.position = pos;
			moveFace.AddPosToGrid();
		}
	}
}
