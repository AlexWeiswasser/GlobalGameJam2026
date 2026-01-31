using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
	private Vector2 mousePos;

	private void Update()
	{
		SetMousePosition();
	}

	private void SetMousePosition()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
	}

	public void OnLeftClick(InputAction.CallbackContext context)
	{
		if(context.performed)
			Debug.Log("clicked");
	}
}
