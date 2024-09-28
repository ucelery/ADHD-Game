using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] private GameObject itemRowPrefab;
	[SerializeField] private Transform rowContainer;

	[Header("Storage Properties")]
	private List<StorageRowItem> storageRowItems = new();

	public void ListItems(List<ItemData> items) {
		// List Items
		int highest = storageRowItems.Count > items.Count ? storageRowItems.Count : items.Count;
		for (int i = 0; i < highest; i++) {
			StorageRowItem row = null;
			// Check if theres enough rows
			if (i > storageRowItems.Count - 1) {
				row = Instantiate(itemRowPrefab, rowContainer).GetComponent<StorageRowItem>();
				storageRowItems.Add(row);
			}

			row = storageRowItems[i];

			if (i < items.Count) {
				row.Initialize(items[i]);
			} else {
				// Disable excess rows
				row.gameObject.SetActive(false);
			}
		}
	}
}
