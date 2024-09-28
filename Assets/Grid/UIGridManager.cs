using System.Collections.Generic;
using UnityEngine;
using Utilities.UIGrid;

/// <summary>
/// General Grid Generator
/// </summary>
public class UIGridManager : MonoBehaviour {
	[Header("Grid Properties")]
	[SerializeField] private RectTransform cell_prefab;
	[SerializeField] private Vector2 cellSize = Vector2.one;
	[SerializeField] private Vector2Int gridSize;

	[SerializeField] private RectTransform gridContainer;
	[SerializeField] private RectTransform objectsContainer;

	private Dictionary<RectTransform, List<UIGridObject>> cells = new();
	private RectTransform rectTransform;

	public Dictionary<RectTransform, List<UIGridObject>> Cells { get { return cells; } }
	public RectTransform RectTransform { get { return rectTransform; } }
	public RectTransform GridContainer { get { return gridContainer; } }

	public Vector2 CellSize { get { return cellSize; } }

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
	}

	private void Start() {
		InitializeCells(gridSize);
	}

	private void InitializeCells(Vector2Int grid_size) {
		// Generate the coordinates of the grid
		int totalCells = grid_size.x * grid_size.y;
		for (int i = 0; i < totalCells; i++) {
			int x = i % grid_size.x;
			int y = i / grid_size.x;

			Vector2Int position = new Vector2Int(x, y);
			RectTransform new_cell = Instantiate(cell_prefab, gridContainer);
			new_cell.sizeDelta = cellSize;
			new_cell.anchoredPosition = position * cellSize;

			cells[new_cell] = new List<UIGridObject>();
		}
	}

	public RectTransform GetCellAtPosition(RectTransform check_cell) {
		foreach (RectTransform cell in cells.Keys) {
			Vector2 check_cell_pos = UIGridUtility.GetGridPosition(check_cell, rectTransform);
			Vector2 final_cell_pos = UIGridUtility.GetGridPosition(cell, rectTransform);

			float x_cell_size = (cellSize.x / 2);
			float y_cell_size = (cellSize.y / 2);

			bool is_within_x = check_cell_pos.x > (final_cell_pos.x - x_cell_size) && check_cell_pos.x < (final_cell_pos.x + x_cell_size);
			bool is_within_y = check_cell_pos.y > (final_cell_pos.y - y_cell_size) && check_cell_pos.y < (final_cell_pos.y + y_cell_size);
			if (is_within_x & is_within_y) {
				return cell;
			}
		}

		return null;
	}

	public void OccupyCells(List<RectTransform> grid_cells, UIGridObject grid_object) {
		string cells_str = string.Empty;
		foreach (RectTransform cell in grid_cells) {
			cells_str += $"{cell.anchoredPosition} ";
			cells[cell].Add(grid_object);
		}

		Debug.Log($"Adding {grid_object} to Cells: {cells_str}");
	}

	public void VacateCells(List<RectTransform> grid_cells, UIGridObject grid_object) {
		string cells_str = string.Empty;
		foreach (RectTransform cell in grid_cells) {
			cells_str += $"{cell.anchoredPosition} ";
			cells[cell].Remove(grid_object);
		}

		Debug.Log($"Removing {grid_object} from Cells: {cells_str}");
	}

	public Vector2 GetCenter(List<RectTransform> cells) {
		if (cells == null || cells.Count == 0) {
			Debug.LogWarning("cells list is empty or null.");
			return Vector2.zero;
		}

		// Get Top, Bot, Left Right points
		float up = float.NegativeInfinity;
		float down = float.PositiveInfinity;
		float right = float.NegativeInfinity;
		float left = float.PositiveInfinity;

		foreach (RectTransform cell in cells) {
			Vector2 cell_pos = cell.anchoredPosition;

			if (cell_pos.x > right)
				right = cell_pos.x;

			if (cell_pos.y > up)
				up = cell_pos.y;

			if (cell_pos.x < left)
				left = cell_pos.x;

			if (cell_pos.y < down)
				down = cell_pos.y;
		}

		float centerX = (right + left) / 2;
		float centerY = (up + down) / 2;

		return new Vector2(centerX, centerY);
	}
}