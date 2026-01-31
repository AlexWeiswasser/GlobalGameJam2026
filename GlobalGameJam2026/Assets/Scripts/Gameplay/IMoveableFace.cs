using UnityEngine;

public interface IMoveableFace
{
	public void RemoveGridPos();
	public void AddPosToGrid();
	public int GetOrder();
	public int GetMask();
	public Vector3 GetPosition();
	public void AddToPosition(Vector3 addtion, bool finalMove = false);
	public void SetPosition(Vector3 pos, bool finalMove = false);
}
