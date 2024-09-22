using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItem : Interactable {
	[Header("Inventory Properties")]
	[SerializeField] private ItemData itemData;

	private Dictionary<Vector2, InteractLock> cellBind = new();
	private List<BoxCollider2D> currentColliders = new();

	private float rotation = 0;

	protected override void Start() {
		Initialize(itemData);
	}

	public void Initialize(ItemData itemData) {
		this.itemData = itemData;

		// Add / Edit Colliders for the items
		int higher = currentColliders.Count > itemData.BoxColliderData.Length ? currentColliders.Count : itemData.BoxColliderData.Length;
		cellBind = new();
		for (int i = 0; i < higher; i++) {
			// Check if theres not enough colliders / cells
			if (currentColliders.Count <= i) {
				BoxCollider2D new_collider = gameObject.AddComponent<BoxCollider2D>();
				currentColliders.Add(new_collider);
				new_collider.usedByComposite = true;
			}

			BoxCollider2D col_item = currentColliders[i];
			col_item.offset = itemData.BoxColliderData[i].offset;
			col_item.size = itemData.BoxColliderData[i].size;
			cellBind.Add(col_item.offset, null);

			// If theres a surplus in BoxColliders, deactivate them
			if (i >= currentColliders.Count) {
				currentColliders[i].enabled = false;
			}
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

		Quaternion quat = Quaternion.Euler(0, 0, rotation);

		transform.rotation = quat;
		Dictionary<Vector2, InteractLock> newCellBind = new();
		foreach (Vector2 point in cellBind.Keys) {
			Vector2 newPoint = RotatePoint90(point);
			newCellBind.Add(newPoint, null);

			Debug.Log(newPoint);
		}

		cellBind = newCellBind;

		HandleIndicators();
	}

	public Vector2 RotatePoint90(Vector2 point) {
		return new Vector2(point.y, -point.x);
	}

	private void ToggleHover(InteractLock interactLock, bool flag) {
		if (interactLock.gameObject.GetComponent<SpriteRenderer>()) {
			interactLock.gameObject.GetComponent<SpriteRenderer>().color = flag ? Color.white : Color.clear;
		}
	}

	public override void Dragging() {
		base.Dragging();

		HandleIndicators();
	}

	public override void DragEnd() {
		// Check if all cells are binded
		bool canDrop = cellBind.Values.All(il => il != null);
		base.HandleDrop();
	}

	private void HandleIndicators() {
		// O(n2)
		foreach (InteractLock il in lockPoints) {
			BindInteractLock(il);
		}

		foreach (InteractLock il in lockPoints) {
			if (cellBind.Values.Contains(il)) {
				ToggleHover(il, true);
				continue;
			}

			ToggleHover(il, false);
		}
	}

	private void BindInteractLock(InteractLock interactLock) {
		Vector2 closest_cell = GetClosestCell(interactLock.transform.position);
		
		// If there is nothing binding that cell, assign that
		if (cellBind[closest_cell] == null) {
			cellBind[closest_cell] = interactLock;
		} 
		
		// If there is check which interact lock is the closest
		else {
			float current_dis = Vector2.Distance((Vector2)transform.position + closest_cell, cellBind[closest_cell].transform.position);
			float incoming_dis = Vector2.Distance((Vector2)transform.position + closest_cell, interactLock.transform.position);

			// If incoming interact lock is closer, replace
			if (incoming_dis < current_dis) {

				cellBind[closest_cell] = interactLock;

				List<Vector2> nullBind = new();
				foreach (KeyValuePair<Vector2, InteractLock> cell in cellBind) {
					if (cell.Value == null || cell.Key == closest_cell) continue;
					
					if (cell.Value == interactLock) {
						nullBind.Add(cell.Key);
					}
				}

				foreach (Vector2 cell in nullBind) {
					cellBind[cell] = null;
				}
			}
		}
	}

	private Vector2 GetClosestCell(Vector2 position) {
		Vector2 closestCell = currentColliders[0].offset;
		foreach (Vector2 cellPos in cellBind.Keys) {
			// Get closest cell
			float closest_dis = Vector2.Distance((Vector2)transform.position + closestCell, position);
			float current_dis = Vector2.Distance((Vector2)transform.position + cellPos, position);
			if (current_dis < closest_dis)
				closestCell = cellPos;
		}

		return closestCell;
	}

	protected override void OnTriggerEnter2D(Collider2D collision) {
		base.OnTriggerEnter2D(collision);

		HandleIndicators();
	}

	protected override void OnTriggerExit2D(Collider2D collision) {
		base.OnTriggerExit2D(collision);

		if (collision.gameObject.GetComponent<SpriteRenderer>()) {
			collision.gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
		}
	}

	private void OnDrawGizmosSelected() {
		foreach (KeyValuePair<Vector2, InteractLock> cell in cellBind) {
			if (cell.Value == null) continue;
			Gizmos.DrawLine((Vector2)transform.position + cell.Key, cell.Value.transform.position);
		}
	}
}
