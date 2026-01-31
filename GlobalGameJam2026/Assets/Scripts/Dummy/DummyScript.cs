using UnityEngine;

public class DummyScript : MonoBehaviour
{
	// Dummy Data
	private bool locked = false;
	private int orderPos = -1;

	public void Init(Vector2 pos, int type = 0, int order = 0)
    {
		Vector3 posVector = pos;
		posVector.z = 0;
		transform.position = posVector;

		if (type > 0)
		{
			locked = true;

			switch (type)
			{
				case 1:
					gameObject.AddComponent<FlipScript>();
					break;
			}

			orderPos = order;
		}
	}

	public int GetOrder()
	{
		return orderPos;
	}
}
