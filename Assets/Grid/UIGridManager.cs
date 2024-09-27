using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utilities.UIGrid;
using static UnityEditor.PlayerSettings;

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

	private Dictionary<Vector2Int, List<UIGridObject>> cells = new();
	private RectTransform rectTransform;

	public Dictionary<Vector2Int, List<UIGridObject>> Cells { get { return cells; } }
	public RectTransform RectTransform { get { return rectTransform; } }

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
			cells[position] = new List<UIGridObject>();

			RectTransform new_cell = Instantiate(cell_prefab, gridContainer);
			new_cell.sizeDelta = cellSize;
			new_cell.anchoredPosition = position * cellSize;
		}
	}

	public Vector2Int? GetCellAtPosition(RectTransform cell_pos) {
		foreach (Vector2Int cell in cells.Keys) {
			Vector2 pos = UIGridUtility.GetGridPosition(cell_pos, rectTransform);

			Vector2 final_cell_pos = cell * cellSize;

			float x_cell_size = (cellSize.x / 2);
			float y_cell_size = (cellSize.y / 2);

			//Debug.Log($"[{pos}] ({(final_cell_pos.x - x_cell_size)}, {(final_cell_pos.y + y_cell_size)})");
			//Debug.Log($"[{pos}] ({(final_cell_pos.x)}, {(final_cell_pos.y)})");

			bool is_within_x = pos.x > (final_cell_pos.x - x_cell_size) && pos.x < (final_cell_pos.x + x_cell_size);
			bool is_within_y = pos.y > (final_cell_pos.y - y_cell_size) && pos.y < (final_cell_pos.y + y_cell_size);
			if (is_within_x & is_within_y) {
				return cell;
			}
		}

		return null;
	}

	public void OccupyCells(List<Vector2Int> objectCells, UIGridObject gridObject) {
		int highest_index = -1;
		foreach (Vector2Int cell in objectCells) {
			Debug.Log($"Adding {this} from {cell}");

			cells[cell].Add(gridObject);

			if (cells[cell].IndexOf(gridObject) > highest_index) {
				highest_index = cells[cell].IndexOf(gridObject);
			}
		}

		gridObject.transform.position += new Vector3(0, 0, -highest_index);
	}

	public void VacateCells(List<Vector2Int> objectCells, UIGridObject gridObject) {
		foreach (Vector2Int cell in objectCells) {
			Debug.Log($"Removing {this} from {cell}");
			cells[cell].Remove(gridObject);
		}
	}

	public int GetObjectIndexInCell(Vector2Int cell_position, UIGridObject object_to_check) {
		return cells[cell_position].IndexOf(object_to_check);
	}

	private string GridObjectString(List<UIGridObject> objects) {
		string str = string.Empty;
		foreach (UIGridObject obj in objects) {
			str += $"{obj.gameObject.name}, ";
		}

		return str;
	}

	public Vector2 GetCenter(List<Vector2Int> valid_cells) {
		if (valid_cells == null || valid_cells.Count == 0) {
			Debug.LogWarning("valid_cells list is empty or null.");
			return Vector2.zero;
		}

		// Get Top, Bot, Left Right points
		float up = float.NegativeInfinity;
		float down = float.PositiveInfinity;
		float right = float.NegativeInfinity;
		float left = float.PositiveInfinity;

		foreach (Vector2Int cell in valid_cells) {
			if (cell.x > right)
				right = cell.x;

			if (cell.y > up)
				up = cell.y;

			if (cell.x < left)
				left = cell.x;

			if (cell.y < down)
				down = cell.y;
		}

		Debug.Log($"Top Right: ({right}, {up}); Bottom Left: ({left}, {down})");

		float centerX = (right + left) / 2;
		float centerY = (up + down) / 2;

		return new Vector2(centerX, centerY) * cellSize;
	}

	public Vector2 GetCenter(List<RectTransform> cells) {
		if (cells == null || cells.Count == 0) {
			Debug.LogWarning("cells list is empty or null.");
			return Vector2.zero;
		}

		float totalX = 0;
		float totalY = 0;

		foreach (RectTransform cell in cells) {
			Vector3 relativePosition = cell.anchoredPosition;

			totalX += relativePosition.x;
			totalY += relativePosition.y;
		}

		float centerX = totalX / cells.Count;
		float centerY = totalY / cells.Count;

		return new Vector2(centerX, centerY);
	}
}