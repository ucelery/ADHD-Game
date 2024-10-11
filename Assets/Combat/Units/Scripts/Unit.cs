using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Behaviour;
using Utilities.Units;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

public class Unit : MonoBehaviour {
	[Header("Unit Properties")]
	[SerializeField] private UnitData unit;
	[SerializeField] private List<ItemData> inventory;
	[SerializeField] private UnitStatsHandler unitStatsHandler;
	[SerializeField] private UnitEvents events;
	[SerializeField] private Transform pointIndicator;

	private List<ItemTracker> item_tracker = new();
	private WeaponData last_weapon_placed;
	private UnitState state;
	private Unit target;

	private WeaponData primaryWeapon; // This will be the basis of the unit's behaviour
	private int currentMoveIndex;
	private Vector2 nextPosition;
	private Vector2 direction;
	private bool canMove = true;
	private Coroutine moveRoutine = null;

	public UnitEvents Events { get { return events; } }
	public UnitData UnitData { get { return unit; } }
	public UnitStatsHandler CurrentStats { get { return unitStatsHandler; } }
	public Vector2 Direction { get { return direction; } }

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
		events.OnTakeDamage = new();

		unitStatsHandler.Initialize(unitData);
		InitializeInventory();

		// If its empty, then let the weapon be the behaviour
		this.primaryWeapon = last_weapon_placed;
	}

	public void InitializeInventory() {
		foreach (ItemData item in inventory) {
			if (item is WeaponData) {
				item_tracker.Add(new ItemTracker(item, unitStatsHandler.GetShootDelay(item as WeaponData)));
				last_weapon_placed = (WeaponData)item;
			}
		}
	}

	public void TakeDamage(Damage damage) {
		unitStatsHandler.HandleDamage(damage);

		// Debug.Log($"This unit took damage {unitStatsHandler.HP}/{unitStatsHandler.MaxHP}");

		events.OnTakeDamage?.Invoke();
	}

	private void FixedUpdate() {
		HandleStates();
	}

	private void BehaviourTick() {
		// All behaviour changes happens here
		switch (state) {
			case UnitState.Idle:
			case UnitState.Patrolling:
				// if detecting an enemy, change state
				this.target = unit.LookForTargets();
				if (this.target != null)
					ChangeState(UnitState.TargetDetected);
				break;
			case UnitState.TargetDetected:
				// if in combat range, switch to in range
				if (Vector2.Distance(transform.position, target.transform.position) <= primaryWeapon.range) {
					ChangeState(UnitState.InCombat);
				}
				break;
			case UnitState.InCombat:
				// Attack target

				// if the target is out of range, go back to Idle
				if (Vector2.Distance(transform.position, target.transform.position) > primaryWeapon.range) {
					ChangeState(UnitState.Idle);
				}

				// if the target is dead, loop back to idle
				if (this.target.state == UnitState.Dead) {
					ChangeState(UnitState.Idle);
				}
				break;
			case UnitState.Dead:
				// Death animation or something
				break;
		}
	}

	private void HandleStates() {
		if (state == UnitState.Dead) return;

		UnitBehaviour behaviour = primaryWeapon.behaviour;

		switch (state) {
			case UnitState.Idle:
				// Dont do anything
				break;
			case UnitState.Patrolling: {
					// follow the patrol movement
					MovementData move_props = behaviour.patrolMovement;
					HandleMovement(move_props);

					break;
				}
			case UnitState.TargetDetected: {
					// Handle combat movement
					MovementData move_props = behaviour.aggroMovement;
					HandleMovement(move_props);

					break;
				}
			case UnitState.InCombat: {
					// Activate all target towards this target
					HandleItemActivation(target);
					LookAtPos((Vector2)target.transform.position);

					MovementData move_props = behaviour.combatMovement;
					HandleMovement(move_props);

					break;
				}
			case UnitState.Dead:
				break;
		}
	}

	private void HandleMovement(MovementData movementData) {
		if (movementData.movement.Length < 1)
			return;

		if (moveRoutine == null)
			moveRoutine = StartCoroutine(MovementRoutine(movementData));

		LookAtPos(this.nextPosition);
	}

	private void LookAtPos(Vector2 pos) {
		direction = pos - (Vector2)transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		pointIndicator.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

	private void OnUnitSpawnDespawn(Unit unit) {
		// this.target = LookForTargets();
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

	private void Despawn() {
		gameObject.SetActive(false);
		events.OnDespawn?.Invoke(this);
	}

	private Vector2 TargetDirection(Unit target) {
		return (target.transform.position - transform.position).normalized;
	}

	private void ChangeState(UnitState new_state) {
		state = new_state;

		currentMoveIndex = 0; // Reset move index
	}

	private void OnDrawGizmosSelected() {
		if (unit != null) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, unit.hitboxRadius);
		}

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere((Vector2)transform.position + direction.normalized, 0.5f);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere((Vector2)transform.position - direction.normalized, 0.5f);
	}

	private IEnumerator MovementRoutine(MovementData movementData) {
		if (unit.type == UnitType.Ally) 
			Debug.Log("Start Moving");

		Movement current_move_data = movementData.movement[currentMoveIndex];
		if (current_move_data.isInstant)
			transform.position = this.nextPosition;
		else
			while (Vector2.Distance((Vector2)transform.position, this.nextPosition) > 0.013f)
				transform.position = Vector2.MoveTowards(transform.position, this.nextPosition, Time.deltaTime * CurrentStats.Speed);

		yield return new WaitUntil(() => Vector2.Distance((Vector2)transform.position, this.nextPosition) <= 0.013f);
		if (unit.type == UnitType.Ally) 
			Debug.Log("Change Move Pos");

		// Update Next Position
		if (movementData.isRandom)
			currentMoveIndex = UnityEngine.Random.Range(0, movementData.movement.Length);
		else {
			currentMoveIndex++;

			if (currentMoveIndex > movementData.movement.Length - 1)
				currentMoveIndex = 0;
		}

		yield return new WaitForSeconds(current_move_data.delay);
		if (unit.type == UnitType.Ally)
			Debug.Log("End Movement");

		// End Routine
		moveRoutine = null;
	}
}
