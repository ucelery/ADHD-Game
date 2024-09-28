using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Utilities.UIGrid;

public class UIGridObject : UIInteractable {
	[SerializeField] protected UIGridManager grid;
	[SerializeField] protected List<RectTransform> cells;

	private GridObjectStates state = GridObjectStates.Idle;

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

		// For Testing
		Initialize(grid);
	}

	public void OnInput_Press(EnhancedTouch.Finger finger) {
		switch (finger.index) {
			case 0:
				// First Finger
				break;
			case 1:
				// Second Finger
				if (state == GridObjectStates.Dragging)
					ToggleRotate();
				break;
		}
	}

	protected virtual void Initialize(UIGridManager grid) {
		this.grid = grid;
	}

	protected virtual bool CanOccupy(List<RectTransform> cells_to_occupy) {
		foreach (RectTransform cell in cells_to_occupy) {
			if (grid.Cells[cell].Count > 0) return false;
		}

		return true;
	}

	public override void OnBeginDrag(PointerEventData eventData) {
		Debug.Log("Drag Start");

		state = GridObjectStates.Dragging;

		// Detach from the grid
		List<RectTransform> valid_cells = GetTouchingGridCells();
		grid.VacateCells(valid_cells, this);

		// Place this object on the top
		transform.SetSiblingIndex(transform.parent.childCount - 1);

		base.OnBeginDrag(eventData);
	}

	public override void OnDrag(PointerEventData eventData) {
		base.OnDrag(eventData);
	}

	public override void OnEndDrag(PointerEventData eventData) {
		List<RectTransform> valid_cells = GetTouchingGridCells();

		state = GridObjectStates.Idle;

		if (valid_cells.Count >= cells.Count && CanOccupy(valid_cells)) {
			// Snap this Game Object on the center of the detected cells
			Vector2 valid_cells_center = grid.GetCenter(valid_cells);
			Vector2 cells_center = grid.GetCenter(cells);

			rectTransform.anchoredPosition = valid_cells_center - cells_center + grid.GridContainer.anchoredPosition;

			// Register the data to the cells
			grid.OccupyCells(valid_cells, this);
		} else {
			Debug.Log("Invalid Drop");
		}
	}

	public virtual void ToggleRotate() {
		// Rotate Cells
		foreach (RectTransform cell in cells) {
			cell.anchoredPosition = new Vector2(cell.anchoredPosition.y, -cell.anchoredPosition.x);
		}
	}

	public bool HasItemAbove() {
		foreach (RectTransform cell in cells) {
			RectTransform cell_pos = grid.GetCellAtPosition(cell);
			if (cell_pos == null) continue;

			// If the current grid object's index is is greater than its count - 1 that means there is something above it
			int object_index = grid.Cells[cell_pos].IndexOf(this);
			if (object_index < grid.Cells[cell_pos].Count - 1)
				return true;
		}

		return false;
	}

	private List<RectTransform> GetTouchingGridCells() {
		List<RectTransform> in_cells = new();
		foreach (RectTransform cell in cells) {
			RectTransform? is_in_cell = grid.GetCellAtPosition(cell);
			if (is_in_cell != null) {
				in_cells.Add(is_in_cell);
			}
		}

		return in_cells;
	}

	private void PrintCells(List<RectTransform> cells) {
		string str = string.Empty;
		foreach (RectTransform cell in cells) {
			str += $"{cell.anchoredPosition}, ";
		}

		Debug.Log($"Cells: {str}");
	}
}