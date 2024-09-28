using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StorageRowItem : MonoBehaviour {
	[SerializeField] private ItemData item;

	[Header("UI Elements")]
	[SerializeField] private TMP_Text nameText;
	[SerializeField] private TMP_Text descText;
	[SerializeField] private TMP_Text amountText;
	[SerializeField] private Image itemIcon;

	private int amount;

	public UnityEvent<ItemData> OnClick;

	public void Initialize(ItemData item) {
		this.item = item;

		nameText.SetText(item.InventoryProperties.itemName);
		descText.SetText(item.InventoryProperties.itemDescription);
		itemIcon.sprite = item.InventoryProperties.inventorySprite;
		amount = 1;
	}

	public void AddAmount() {
		amount++;
	}

	public void ReduceAmount() {
		amount--;
	}

	public void OnClick_ItemRow() {
		OnClick.Invoke(item);
	}
}
