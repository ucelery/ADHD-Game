using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Utilities.UIGrid;

public class UIGridObject : UIInteractable {
	[SerializeField] protected UIGridManager grid;
	[SerializeField] protected List<RectTransform> cells;

	private float rotation = 0;

	private void OnEnable() {
		EnhancedTouch.TouchSimulation.Enable();
		EnhancedTouch.EnhancedTouchSupport.Enable();


	}

	private void OnDisable() {
		EnhancedTouch.TouchSimulation.Disable();
		EnhancedTouch.EnhancedTouchSupport.Disable();
	}

	private void Start() {
		EnhancedTouch.Touch.onFingerDown += OnInput_Press;
	}

	public void OnInput_Press(EnhancedTouch.Finger finger) {
		switch (finger.index) {
			case 0:
				// First Finger
				break;
			case 1:
				// Second Finger
				ToggleRotate();
				break;
		}
	}

	protected virtual void Initialize(UIGridManager grid) {
		this.grid = grid;

		foreach (RectTransform t in cells) {
			BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
			collider.offset = t.localPosition;
			collider.size = grid.CellSize;
			collider.usedByComposite = true;
		}
	}

	protected virtual bool CanOccupy(List<Vector2Int> cells_to_occupy) {
		foreach (Vector2Int cell in cells_to_occupy) {
			if (grid.Cells[cell].Count > 0) return false;
		}

		return true;
	}

	public override void OnBeginDrag(PointerEventData eventData) {
		Debug.Log("Drag Start");

		// Detach from the grid
		//List<Vector2Int> valid_cells = GetTouchingGridCells();
		//grid.VacateCells(valid_cells, this);

		base.OnBeginDrag(eventData);
	}

	public override void OnDrag(PointerEventData eventData) {
		Debug.Log("Dragging");

		base.OnDrag(eventData);
	}

	public override void OnEndDrag(PointerEventData eventData) {
		List<Vector2Int> valid_cells = GetTouchingGridCells();

		string str = string.Empty;
		foreach (Vector2Int cell in valid_cells) {
			str += $"{cell}, ";
		}

		// Debug.Log($"Valid Cells: {str}");

		if (valid_cells.Count >= cells.Count && CanOccupy(valid_cells)) {
			Debug.Log("Valid Drop");
			// Snap this Game Object on the center of the detected cells
			Vector2 valid_cells_center = grid.GetCenter(valid_cells);
			// Debug.Log($"Valid Cells Center: {valid_cells_center}");
			// Vector2 cells_center = grid.GetCenter(cells);

			// rectTransform.anchoredPosition = valid_cells_center - cells_center;

			//// Register the data and update Z Index
			// grid.OccupyCells(valid_cells, this);
		} else {
			Debug.Log("Invalid Drop");
		}
	}

	public void ToggleRotate() {
		switch (rotation) {
			case 0f:
				rotation = 270f;
				break;
			case 270f:
				rotation = 180f;
				break;
			case 180f:
				rotation = 90f;
				break;
			case 90f:
				rotation = 0f;
				break;
		}

		// PrintCells(cells);

		Quaternion quat = Quaternion.Euler(0, 0, rotation);

		transform.rotation = quat;

		// PrintCells(cells);
	}

	public bool HasItemAbove() {
		foreach (RectTransform cell in cells) {
			Vector2Int? cell_pos = grid.GetCellAtPosition(cell);
			if (cell_pos == null) continue;

			// If the current grid object's index is is greater than its count - 1 that means there is something above it
			int object_index = grid.Cells[(Vector2Int)cell_pos].IndexOf(this);
			if (object_index < grid.Cells[(Vector2Int)cell_pos].Count - 1)
				return true;
		}

		return false;
	}

	private List<Vector2Int> GetTouchingGridCells() {
		List<Vector2Int> in_cells = new();
		foreach (RectTransform cell in cells) {
			Vector2Int? is_in_cell = grid.GetCellAtPosition(cell);
			if (is_in_cell != null) {
				in_cells.Add((Vector2Int)is_in_cell);
			}
		}

		return in_cells;
	}

	private void PrintCells(List<RectTransform> cells) {
		string str = string.Empty;
		foreach (RectTransform cell in cells) {
			str += $"{UIGridUtility.GetGridPosition(cell, grid.RectTransform)}, ";
		}

		Debug.Log($"Cells: {str}");
	}
}