using System;
using UnityEngine;
using Utilities.Inventory;

public class ItemData : ScriptableObject {
	[Header("Item Data")]
	public string itemId;
	public InventoryProperties InventoryProperties;
	public ItemType type;

	public virtual void Activate() { }
}
