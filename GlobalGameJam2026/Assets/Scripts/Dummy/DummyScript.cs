using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public void LockToGrid()
    {
        Vector2 newPos;
        
        float xPos = Mathf.CeilToInt(transform.position.x);
		float yPos = Mathf.CeilToInt(transform.position.y);

        newPos = new Vector2(xPos, yPos);

        transform.position = newPos;
	}
}
