using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// General Grid Generator
/// </summary>
public class GridManager : MonoBehaviour {
	[Header("Grid Properties")]
	[SerializeField] private Vector2 cellSize = Vector2.one;
	[SerializeField] private Vector2Int gridSize;

	private Dictionary<Vector2Int, ItemData> cells = new();

	public Vector2 CellSize { get { return cellSize; } }

	private void Start() {
		InitializeCells(gridSize);
	}

	private void Update() {
		// Check if point is in cell
		//Vector2Int? cell = GetCellAtPosition(test_point.position);
		//if (cell != null) {
		//	Debug.Log($"Pos is in cell {cell}");
		//}
	}

	private void InitializeCells(Vector2Int grid_size) {
		// Generate the coordinates of the grid
		int totalCells = grid_size.x * grid_size.y;
		for (int i = 0; i < totalCells; i++) {
			int x = i % grid_size.x;    
			int y = i / grid_size.x;    
			
			Vector2Int position = new Vector2Int(x, y);
			cells[position] = null; 

			Debug.Log($"{x}, {y}");
		}
	}

	public Vector2Int? GetCellAtPosition(Vector2 pos) {
		foreach (Vector2Int cell in cells.Keys) {
			Vector2 final_cell_pos = new Vector2(cell.x, cell.y) + (Vector2)transform.position;

			float x_cell_size = (cellSize.x / 2);
			float y_cell_size = (cellSize.y / 2);

			bool is_within_x = pos.x > (final_cell_pos.x - x_cell_size) && pos.x < (final_cell_pos.x + x_cell_size);
			bool is_within_y = pos.y > (final_cell_pos.y - y_cell_size) && pos.y < (final_cell_pos.y + y_cell_size);
			if (is_within_x & is_within_y) {
				return cell;
			}
		}

		return null;
	}

	private void OnDrawGizmos() {
		foreach (Vector2Int cell in cells.Keys) {
			Vector2 center_cell = new Vector2(cell.x, cell.y) + (Vector2)transform.position;

			Gizmos.DrawWireCube(center_cell, cellSize);

			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;
			Handles.Label(center_cell, cell.ToString(), style);
		}
	}
}