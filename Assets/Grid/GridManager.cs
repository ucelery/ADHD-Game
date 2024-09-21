using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour {
	[SerializeField] private GameObject cellObject;
	[SerializeField] private Tilemap cells;

	private List<Vector2Int> tiles;

	private void Awake() {
		tiles = new List<Vector2Int>();

		foreach (var pos in cells.cellBounds.allPositionsWithin) {
			if (cells.HasTile(pos)) {
				// Add 1 to y due to an offset error
				tiles.Add((Vector2Int)pos + new Vector2Int(0, 1));
			}
		}
	}

	private void Start() {
		foreach (Vector2Int tile in tiles) {
			Instantiate(cellObject, new Vector2(tile.x, tile.y), Quaternion.identity);
		}
	}

	public List<Vector2Int> GetAllCells(Tilemap tilemap) {
		List<Vector2Int> tileWorldLocations = new List<Vector2Int>();

		foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
			if (tilemap.HasTile(pos)) {
				// Add 1 to y due to an offset error
				tileWorldLocations.Add((Vector2Int)pos + new Vector2Int(0, 1));
			}
		}

		return tileWorldLocations;
	}

	public bool HasCell(Vector2Int target) {
		return GetAllCells(cells).Contains(target);
	}

	public bool CanOccupy(Vector2Int target) {
		List<Vector2Int> cell = GetAllCells(cells);

		return cell.Contains(target);
	}
}