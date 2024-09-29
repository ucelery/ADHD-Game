using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Inventory;
using Utilities.Units;

public class Unit : MonoBehaviour {
	[Header("Unit Properties")]
	[SerializeField] private UnitData unit;
	[SerializeField] private UnitStatsData _base;
	[SerializeField] private UnitStatsData _growth;
	[SerializeField] private List<ItemData> inventory;
	[SerializeField] private LevelHandler levelHandler;
	[SerializeField] private UnitEvents events;

	public UnitEvents Events { get { return events; } }

	private List<ItemTracker> item_tracker = new();

	private void Start() {
		Initialize(unit);
	}

	public void Initialize(UnitData unitData) {
		this.unit = unitData;

		events.OnShoot = new();
		events.OnDespawn = new();

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
				events.OnShoot?.Invoke(item.ItemData);
				if (item.ItemData is WeaponData) {
					ProjectilePooling.Instance.Shoot(item.ItemData as WeaponData, this, Vector2.one);
				}
			}
		}
	}

	private float GetShootDelay(WeaponData weapon) {
		return 1.0f / (_base.attackSpeed * weapon.rof * (1 + _growth.attackSpeed * levelHandler.Level));
	}

	private void Despawn() {
		gameObject.SetActive(false);
		events.OnDespawn?.Invoke(this);
	}
}
