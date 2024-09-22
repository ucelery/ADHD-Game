using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItem : Interactable {
	[Header("Inventory Properties")]
	[SerializeField] private Transform[] cells;
	private Dictionary<Transform, InteractLock> cellBind = new();

	private float rotation = 0;

	protected override void Start() {
		foreach (Transform cell in cells) {
			cellBind.Add(cell, null);
		}
	}

	public void ToggleRotate() {
		switch (rotation) {
			case 0f:
				rotation = 90f;
				break;
			case 90f:
				rotation = 180f;
				break;
			case 180f:
				rotation = 270f;
				break;
			case 270f:
				rotation = 0f;
				break;
		}

		transform.rotation = Quaternion.Euler(0, 0, rotation);

		foreach (InteractLock il in lockPoints) {
			BindInteractLock(il);
		}

		UpdateHovers();
	}

	private void ToggleHover(InteractLock interactLock, bool flag) {
		if (interactLock.gameObject.GetComponent<SpriteRenderer>()) {
			interactLock.gameObject.GetComponent<SpriteRenderer>().color = flag ? Color.white : Color.clear;
		}
	}

	public override void Dragging() {
		base.Dragging();

		foreach (InteractLock il in lockPoints) {
			BindInteractLock(il);
		}

		UpdateHovers();
	}

	public override void DragEnd() {
		// Check if all cells are binded
		bool canDrop = cellBind.Values.All(il => il != null);
		base.HandleDrop();
	}

	private void UpdateHovers() {
		// Update hovers
		foreach (InteractLock il in lockPoints) {
			if (cellBind.Values.Contains(il)) {
				ToggleHover(il, true);
				continue;
			}

			ToggleHover(il, false);
		}
	}

	private void BindInteractLock(InteractLock interactLock) {
		Transform closest_cell = GetClosestCell(interactLock.transform.position);
		
		// If there is nothing binding that cell, assign that
		if (cellBind[closest_cell] == null) {
			cellBind[closest_cell] = interactLock;

		} 
		
		// If there is check which interact lock is the closest
		else {
			float current_dis = Vector2.Distance(closest_cell.position, cellBind[closest_cell].transform.position);
			float incoming_dis = Vector2.Distance(closest_cell.position, interactLock.transform.position);

			// If incoming interact lock is closer, replace
			if (incoming_dis < current_dis) {

				cellBind[closest_cell] = interactLock;

				List<Transform> nullBind = new();
				foreach (KeyValuePair<Transform, InteractLock> cell in cellBind) {
					if (cell.Value == null || cell.Key == closest_cell) continue;
					
					if (cell.Value == interactLock) {
						nullBind.Add(cell.Key);
					}
				}

				foreach (Transform cell in nullBind) {
					cellBind[cell] = null;
				}
			}
		}
	}

	private Transform GetClosestCell(Vector3 position) {
		Transform closestCell = cells[0];
		foreach (Transform cell in cells) {
			// Get closest cell
			float closest_dis = Vector2.Distance(closestCell.position, position);
			float current_dis = Vector2.Distance(cell.position, position);
			if (current_dis < closest_dis)
				closestCell = cell;
		}

		return closestCell;
	}

	protected override void OnTriggerExit2D(Collider2D collision) {
		base.OnTriggerExit2D(collision);

		if (collision.gameObject.GetComponent<SpriteRenderer>()) {
			collision.gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
		}
	}

	private void OnDrawGizmosSelected() {
		foreach (KeyValuePair<Transform, InteractLock> cell in cellBind) {
			if (cell.Value == null) continue;
			Gizmos.DrawLine(cell.Key.position, cell.Value.transform.position);
		}
	}
}
