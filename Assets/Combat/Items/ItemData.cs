using UnityEngine;
using Utilities.Inventory;

public class ItemData : ScriptableObject {
	[Header("Item Data")]
	public InventoryProperties InventoryProperties;
	public ItemType type;

	public virtual void Activate() {

	}
}
