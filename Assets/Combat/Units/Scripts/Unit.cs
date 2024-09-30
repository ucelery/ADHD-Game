using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Inventory;
using Utilities.Units;
using static UnityEngine.GraphicsBuffer;

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
	private UnitState state;
	private Unit target;

	private void OnEnable() {
		UnitPooling.Instance.Events.OnSpawn.AddListener(OnUnitSpawnDespawn);
		UnitPooling.Instance.Events.OnDespawn.AddListener(OnUnitSpawnDespawn);
	}

	private void OnDisable() {
		UnitPooling.Instance.Events.OnSpawn.RemoveListener(OnUnitSpawnDespawn);
		UnitPooling.Instance.Events.OnDespawn.RemoveListener(OnUnitSpawnDespawn);
	}

	private void Start() {
		Initialize(unit);
	}

	public void Initialize(UnitData unitData) {
		this.unit = unitData;
		this.inventory = unitData.inventory;

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
		HandleBehaviour();
	}

	private void OnUnitSpawnDespawn(Unit unit) {
		LookForTargets();
	}

	private void HandleBehaviour() {
		switch (state) {
			case UnitState.Idle:
				break;
			case UnitState.Patrolling:
				break;
			case UnitState.InCombat:
				InCombatHandler();
				break;
			case UnitState.Dead:
				break;
		}
	}

	private void InCombatHandler() {
		if (target == null) return;

		// Activate all target towards this target
		HandleItemActivation(target);
	}

	private void HandleItemActivation(Unit target) {
		foreach (ItemTracker item in item_tracker) {
			if (item.CanActivate()) {
				events.OnShoot?.Invoke(item.ItemData);
				ActivateItem(item, TargetDirection(target));
			}
		}
	}

	private void ActivateItem(ItemTracker item, Vector2 direction) {
		if (item.ItemData is WeaponData && state == UnitState.InCombat) {
			ProjectilePooling.Instance.Shoot(item.ItemData as WeaponData, this, direction);
		}
	}

	private float GetShootDelay(WeaponData weapon) {
		return 1.0f / (_base.attackSpeed * weapon.rof * (1 + _growth.attackSpeed * levelHandler.Level));
	}

	private void Despawn() {
		gameObject.SetActive(false);
		events.OnDespawn?.Invoke(this);
	}

	private void LookForTargets() {
		UnitType target_filter = this.unit.target;
		Dictionary<UnitType, List<Unit>> targets = UnitPooling.Instance.ActiveUnits;

		if (!targets.ContainsKey(target_filter)) return;

		if (targets.Count > 0) {
			// Has Targets
			this.target = targets[target_filter].First();
			ChangeState(UnitState.InCombat);
		}
	}

	private Vector2 TargetDirection(Unit target) {
		return (target.transform.position - transform.position).normalized;
	}

	private void ChangeState(UnitState new_state) {
		state = new_state;
	}
}
