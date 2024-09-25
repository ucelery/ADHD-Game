using System.Collections.Generic;
using UnityEngine;

public class GridObject : Interactable {
	[SerializeField] private GridManager grid;
	[SerializeField] private Transform[] cells;

	private float rotation = 0;

	protected override void Start() {
		base.Start();
		Initialize(grid);
	}

	public void Initialize(GridManager grid) {
		this.grid = grid;

		foreach (Transform t in cells) {
			BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
			collider.offset = t.localPosition;
			collider.size = grid.CellSize;
			collider.usedByComposite = true;
		}
	}

	protected override void HandleDrop() {
		List<Vector2Int> valid_cells = GetTouchingGridCells();
		if (valid_cells.Count >= cells.Length && CanOccupy(valid_cells)) {
			// Register the data
			grid.OccupyCells(valid_cells, this);

			// Snap this Game Object on the center of the detected cells
			Vector2 valid_cells_center = GetCenter(valid_cells) + (Vector2)grid.transform.position;
			Vector2 cells_center = GetCenter(cells);

			transform.position = valid_cells_center - cells_center;
		} else {
			Debug.Log("Invalid Drop");
		}
	}

	protected virtual bool CanOccupy(List<Vector2Int> cells_to_occupy) {
		foreach (Vector2Int cell in cells_to_occupy) {
			if (grid.Cells[cell].Count > 0) return false;
		}

		return true;
	}

	public override void DragStart() {
		// Detach from the grid
		List<Vector2Int> valid_cells = GetTouchingGridCells();
		grid.VacateCells(valid_cells, this);

		base.DragStart();
	}

	public override void Dragging() {
		base.Dragging();
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

		Quaternion quat = Quaternion.Euler(0, 0, rotation);

		transform.rotation = quat;
	}

	private List<Vector2Int> GetTouchingGridCells() {
		List<Vector2Int> in_cells = new();
		foreach (Transform cell in cells) {
			Vector2Int? is_in_cell = grid.GetCellAtPosition(cell.position);
			if (is_in_cell != null) {
				in_cells.Add((Vector2Int)is_in_cell);
			}
		}

		return in_cells;
	}

	private Vector2 GetCenter(List<Vector2Int> valid_cells) {
		if (valid_cells == null || valid_cells.Count == 0) {
			Debug.LogWarning("valid_cells list is empty or null.");
			return Vector2.zero;
		}

		int totalX = 0;
		int totalY = 0;

		foreach (Vector2Int cell in valid_cells) {
			totalX += cell.x;
			totalY += cell.y;
		}

		float centerX = (float)totalX / valid_cells.Count;
		float centerY = (float)totalY / valid_cells.Count;

		return new Vector2(centerX, centerY);
	}

	private Vector2 GetCenter(Transform[] cells) {
		if (cells == null || cells.Length == 0) {
			Debug.LogWarning("cells list is empty or null.");
			return Vector2.zero;
		}

		float totalX = 0;
		float totalY = 0;

		foreach (Transform cell in cells) {
			Vector3 relativePosition = cell.position - transform.position;

			totalX += relativePosition.x;
			totalY += relativePosition.y;
		}

		float centerX = totalX / cells.Length;
		float centerY = totalY / cells.Length;

		return new Vector2(centerX, centerY);
	}
}