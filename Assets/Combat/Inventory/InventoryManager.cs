using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
	[SerializeField] private PlayerData playerData;

	[Header("UI Elements")]
	[SerializeField] private Transform itemContainer;
	[SerializeField] private StorageManager storageManager;

	private void OnEnable() {
		// List Items
		storageManager.ListItems(playerData.Items);
	}
}
