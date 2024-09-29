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

	public ItemData Item { get { return item; } }

	public UnityEvent<ItemData> OnClick;

	public void Initialize(ItemData item) {
		this.item = item;

		nameText.SetText(item.InventoryProperties.itemName);
		descText.SetText(item.InventoryProperties.itemDescription);
		itemIcon.sprite = item.InventoryProperties.inventorySprite;
		amount = 1;

		amountText.SetText($"{amount}x");
	}

	public void AddAmount() {
		amount++;
		amountText.SetText($"{amount}x");
	}

	public void ReduceAmount() {
		amount--;
		amountText.SetText($"{amount}x");
	}

	public void ResetAmount() {
		amount = 0;
		amountText.SetText($"{amount}x");
	}

	public void OnClick_ItemRow() {
		OnClick.Invoke(item);
	}
}
