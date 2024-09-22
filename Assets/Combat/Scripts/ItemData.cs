using UnityEngine;

public class ItemData : ScriptableObject {
	[Header("Item Data")]
	[SerializeField] private Sprite inventorySprite;
	[SerializeField] private Sprite dropSprite;
	[SerializeField] private BoxComponent[] boxColliderData;
	public BoxComponent[] BoxColliderData { get { return boxColliderData; } }

	[System.Serializable]
	public struct BoxComponent {
		public Vector2 offset;
		public Vector2 size;
	}
}
