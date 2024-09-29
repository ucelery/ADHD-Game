using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageManager : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] private GameObject itemRowPrefab;
	[SerializeField] private GameObject inventoryItemPrefab;
	[SerializeField] private Transform rowContainer;

	[Header("Storage Properties")]
	[SerializeField] private PlayerData playerData;

	private Dictionary<ItemData, StorageRowItem> storageRowItems = new();
	private List<InventoryItem> inventoryItems = new();

	public void ListItems(List<ItemData> items) {
		// List Items
		int highest = storageRowItems.Count > items.Count ? storageRowItems.Count : items.Count;
		foreach (ItemData item in items) {
			// check if there is an item in the dictionary, if there is just add the amount
			// - else make one
			if (storageRowItems.ContainsKey(item)) {
				storageRowItems[item].gameObject.SetActive(true);
				storageRowItems[item].AddAmount();
			} else {
				StorageRowItem new_row = Instantiate(itemRowPrefab, rowContainer).GetComponent<StorageRowItem>();
				new_row.OnClick.AddListener(OnItemSelect);
				storageRowItems.Add(item, new_row);
				new_row.Initialize(item);
			}
		}
	}

	public void ResetItems() {
		foreach (KeyValuePair<ItemData, StorageRowItem> item in storageRowItems) {
			item.Value.ResetAmount();
			item.Value.gameObject.SetActive(false);
		}
	}

	public void OnItemSelect(ItemData item) {
		
	}
}
