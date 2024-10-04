using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
	public UnitData UnitData {  get { return unit; } }

	private List<ItemTracker> item_tracker = new();
	private WeaponData last_weapon_placed;
	private UnitState state;
	private Unit target;
	private UnitBehaviour behaviour;
	private List<Vector2> patrolPoints;

	private void OnEnable() {
		UnitPooling.Instance.Events.OnSpawn.AddListener(OnUnitSpawnDespawn);
		UnitPooling.Instance.Events.OnDespawn.AddListener(OnUnitSpawnDespawn);

		TickSpeed.BehaviourTickEvent += BehaviourTick;
	}

	private void OnDisable() {
		UnitPooling.Instance.Events.OnSpawn.RemoveListener(OnUnitSpawnDespawn);
		UnitPooling.Instance.Events.OnDespawn.RemoveListener(OnUnitSpawnDespawn);

		TickSpeed.BehaviourTickEvent -= BehaviourTick;
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

		this.behaviour = last_weapon_placed.behaviour;
	}

	public void InitializeInventory() {
		foreach (ItemData item in inventory) {
			if (item is WeaponData) {
				item_tracker.Add(new ItemTracker(item, GetShootDelay(item as WeaponData)));
				last_weapon_placed = (WeaponData)item;
			}
		}
	}

	public void TakeDamage(Damage damage) {
		// Debug.Log($"This unit took damage by {damage.origin}");
	}

	private void Update() {
		HandleBehaviour();
	}

	private void BehaviourTick() {
		switch (state) {
			case UnitState.Idle:
				break;
			case UnitState.Patrolling:
				break;
			case UnitState.TargetDetected:
				break;
			case UnitState.InRange:
				break;
			case UnitState.InCombat:
				break;
			case UnitState.Dead:
				break;
		}
	}

	private void OnUnitSpawnDespawn(Unit unit) {
		this.target = LookForTargets();

		if (this.target != null)
			ChangeState(UnitState.InCombat);
	}

	private void HandleBehaviour() {
		switch (state) {
			case UnitState.Idle:
				break;
			case UnitState.Patrolling:
				break;
			case UnitState.TargetDetected:
				break;
			case UnitState.InRange:
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
			WeaponData weapon = (WeaponData)item.ItemData;
			Damage weapon_damage = new Damage(this, weapon);

			ProjectilePooling.Instance.Shoot(item.ItemData as WeaponData, weapon_damage, direction);
		}
	}

	private float GetShootDelay(WeaponData weapon) {
		return 1.0f / (_base.attackSpeed * weapon.rof * (1 + _growth.attackSpeed * levelHandler.Level));
	}

	private void Despawn() {
		gameObject.SetActive(false);
		events.OnDespawn?.Invoke(this);
	}

	private Unit LookForTargets() {
		UnitType target_filter = this.unit.target;
		Dictionary<UnitType, List<Unit>> targets = UnitPooling.Instance.ActiveUnits;

		if (!targets.ContainsKey(target_filter)) return null;

		if (targets.Count > 0) {
			// Has Targets
			return targets[target_filter].First();
		}

		return null;
	}

	private Vector2 TargetDirection(Unit target) {
		return (target.transform.position - transform.position).normalized;
	}

	private void ChangeState(UnitState new_state) {
		state = new_state;
	}

	private void OnDrawGizmosSelected() {
		if (unit != null) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, unit.hitboxRadius);
		}
	}
}
