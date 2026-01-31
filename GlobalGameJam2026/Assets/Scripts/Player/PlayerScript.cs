using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
	// Player Data
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
		if (context.performed)
		{
			// Do click things here
		}
	}
}
