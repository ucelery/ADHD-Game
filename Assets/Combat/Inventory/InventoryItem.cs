using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Utilities.Inventory;

public class InventoryItem : GridObject {
    [Header("Inventory Properties")]
    [SerializeField] private ItemData item;
	[SerializeField] SpriteRenderer itemSprite;

	public ItemData Item { get { return item; } }

	protected override void Start() {
		Initialize(grid, item);
		Debug.Log(item.type);
	}

	public void Initialize(GridManager grid, ItemData item) {
		this.item = item;
		itemSprite.sprite = item.InventoryProperties.inventorySprite;

		foreach (BoxComponent collider in item.InventoryProperties.boxColliderData) {
			GameObject point = new GameObject("Point");
			point.transform.parent = transform;
			point.transform.localPosition = collider.offset;

			cells.Add(point.transform);
		}

		base.Initialize(grid);
	}

	protected override bool CanOccupy(List<Vector2Int> cells_to_occupy) {
		// Inventory items can only occupy if there is a backpack in that cell
		foreach (Vector2Int cell in cells_to_occupy) {
			// if item is not a bag and grid isnt occupied, return false
			if (!item.type.HasFlag(ItemType.Backpack) && grid.Cells[cell].Count < 1)
				return false;

			// Check if occupying grid cells have a backpack
			foreach (GridObject gridObj in grid.Cells[cell]) {
				if (gridObj is InventoryItem) {
					InventoryItem cell_item = (gridObj as InventoryItem);
					bool is_part_of_filter = item.InventoryProperties.cellFilter.HasFlag(cell_item.Item.type);
					Debug.Log($"[{cell}] [{item.InventoryProperties.cellFilter}].Has({cell_item.Item.type}); = {is_part_of_filter}");
					if (!is_part_of_filter) {
						Debug.Log($"Item Looking for {item.InventoryProperties.cellFilter} and got {cell_item.Item.type}");
						return false;
					}
				}
			}
		}

		return true;
	}
}
