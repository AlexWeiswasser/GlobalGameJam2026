using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour, IMoveableFace
{
	// Player Data
	private Vector2 mousePos;

	// Caching
	private GridManager gridManager;

	private void Start()
	{
		if (Camera.main.TryGetComponent<GridManager>(out GridManager grid))
		{
			gridManager = grid;
		}
	}

	private void Update()
	{
		SetMousePosition();
	}

	private void SetMousePosition()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
	}

	public int GetOrder()
	{
		return 99;
	}

	public void UpdatePositionOnGrid()
	{
		gridManager.ClearPos((Vector2)transform.position);
		gridManager.AddToGrid(gameObject);
	}
	public void RemoveGridPos()
	{
		gridManager.ClearPos((Vector2)transform.position);
	}

	public void AddGridPos()
	{
		if (Camera.main.TryGetComponent<GridManager>(out GridManager gridManager))
		{
			gridManager.AddToGrid(gameObject);
		}
	}

	public void OnLeftClick(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			// Do click things here
		}
	}
}
