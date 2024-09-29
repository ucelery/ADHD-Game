using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Inventory;

public class Unit : MonoBehaviour {
	[Header("Unit Properties")]
	[SerializeField] private UnitData unit;
	[SerializeField] private UnitStatsData _base;
	[SerializeField] private UnitStatsData _growth;
	[SerializeField] private List<ItemData> inventory;
	[SerializeField] private LevelHandler levelHandler;

	private List<ItemTracker> item_tracker = new();

	private void Awake() {
		InitializeInventory();
	}

	public void InitializeInventory() {
		foreach (ItemData item in inventory) {
			if (item is WeaponData)
				item_tracker.Add(new ItemTracker(item, GetShootDelay(item as WeaponData)));
		}
	}

	private void Update() {
		HandleItemActivation();
	}

	private void HandleItemActivation() {
		foreach (ItemTracker item in item_tracker) {
			if (item.CanActivate()) {
				if (item.ItemData is WeaponData) {
					ProjectilePooling.Instance.Shoot(item.ItemData as WeaponData, this, Vector2.one);
				}
			}
		}
	}

	private float GetShootDelay(WeaponData weapon) {
		return 1.0f / (_base.attackSpeed * weapon.rof * (1 + _growth.attackSpeed * levelHandler.Level));
	}
}
