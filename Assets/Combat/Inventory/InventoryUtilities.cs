using static ItemData;
using UnityEngine;

namespace Utilities.Inventory {
	[System.Flags]
	public enum ItemType {
		Nothing = 0,
		Everything = 1 << 0,
		Backpack = 1 << 1,
		Ability = 1 << 2,
		Weapon = 1 << 3,
	}

	[System.Serializable]
	public struct InventoryProperties {
		public string itemName;
		public string itemDescription;
		public Sprite inventorySprite;
		public Sprite dropSprite;
		public BoxComponent[] boxColliderData;
		public ItemType cellFilter; // Type of cell this item can occupy
		public float rotation;
	}

	[System.Serializable]
	public struct BoxComponent {
		public Vector2 offset;
		public Vector2 size;
	}
}