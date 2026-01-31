using UnityEngine;

public class FlipScript : MonoBehaviour, IDummyFace
{
   public void DummyAction(int actionNum)
	{
		Debug.Log($"Dummy action number is {actionNum}");
	}
}
