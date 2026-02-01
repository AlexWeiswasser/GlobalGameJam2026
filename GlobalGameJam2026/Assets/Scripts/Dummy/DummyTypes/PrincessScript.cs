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
					if (LevelManager.Instance != null)
						LevelManager.Instance.CompletedLevel(LevelManager.Instance.currentLevelID);
                    return true;
                }
				else
				{
					GetComponent<DummyScript>().GetGameplayManager().StopDance();
                    GetComponent<SpriteRenderer>().sprite = GetComponent<DummyScript>().princessDeathSprite;
				}
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
