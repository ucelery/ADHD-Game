using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.Inventory;

public class InventoryItem : UIGridObject {
    [Header("Inventory Item")]
    [SerializeField] private ItemData item;
	[SerializeField] private GameObject inventoryCellPrefab;

	[Header("UI Elements")]
	[SerializeField] private RectTransform spriteHolder;
	[SerializeField] private RectTransform itemSprite;
	[SerializeField] private Transform cellsContainer;

	private float rotation = 0;

	public ItemData Item { get { return item; } }

	protected override void Initialize(UIGridManager grid) {
		foreach (BoxComponent col in item.InventoryProperties.boxColliderData) {
			GameObject new_ui_element = Instantiate(inventoryCellPrefab, cellsContainer);
			RectTransform rect = new_ui_element.GetComponent<RectTransform>();
			rect.anchoredPosition = col.offset * grid.CellSize;
			rect.sizeDelta = grid.CellSize;
			itemSprite.localEulerAngles = new Vector3(0, 0, item.InventoryProperties.rotation);
			itemSprite.GetComponent<Image>().sprite = item.InventoryProperties.inventorySprite;

			cells.Add(rect);
		}

		base.Initialize(grid);
	}

	public override void OnBeginDrag(PointerEventData eventData) {
		if (CanDrag()) {
			base.OnBeginDrag(eventData);
		}
	}

	protected override bool CanDrag() {
		return base.CanDrag();
	}

	protected override bool CanOccupy(List<RectTransform> cells_to_occupy) {
		// Item Filtering - Checks all cells that we're about to occupy
		// - If current item has a filter check if occupying cell is part of it
		// - If it is allow occupying and vise versa
		if (item.InventoryProperties.cellFilter != ItemType.Nothing) {
			int valid_cells = 0;
			foreach (RectTransform cell in cells_to_occupy) {
				// Get all the UIGridObjects in that cell
				List<UIGridObject> objects = grid.Cells[cell];
				foreach (UIGridObject grid_obj in objects) {
					InventoryItem item_holder = grid_obj as InventoryItem;
					if (item_holder == null) continue;

					ItemType filter = item.InventoryProperties.cellFilter;
					ItemType target_type = item_holder.Item.type;
					if (filter.HasFlag(target_type))
						valid_cells++;
				}
			}

			if (valid_cells >= cells_to_occupy.Count) 
				return true;

			return false;
		}

		// Default Grid Checking
		if (!base.CanOccupy(cells_to_occupy)) return false;

		return true;
	}

	public override void ToggleRotate() {
		base.ToggleRotate();

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

		spriteHolder.rotation = quat;
		spriteHolder.anchoredPosition = grid.GetCenter(cells);
	}
}
