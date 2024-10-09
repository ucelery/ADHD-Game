using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Units;

/// <summary>
/// Handles all status effects changes, stats changes etc
/// </summary>
[System.Serializable]
public class UnitStatsHandler {
	[SerializeField] private UnitStatsData _base;
	[SerializeField] private UnitStatsData _growth;
	[SerializeField] private LevelHandler levelHandler;

	private CurrentStats currentStats;

	public float MaxHP { get { return _base.hitPoints + (_growth.hitPoints * levelHandler.Level); } }
	public float MaxMP { get { return _base.manaPoints + (_growth.manaPoints * levelHandler.Level); } }
	public float HP { get { return currentStats.hitPoints; } }
	public float MP { get { return currentStats.manaPoints; } }
	public float Speed { get { return _base.speed + (_growth.speed * levelHandler.Level); } }
	public float Defense { get { return _base.defense + (_growth.speed * levelHandler.Level); } }
	public float AttackSpeed { get { return _base.attackSpeed * (1 + _growth.attackSpeed * levelHandler.Level); } }
	public float Attack { get { return _base.attack * (1 + _growth.attack * levelHandler.Level); } }

	public void Initialize(UnitData unitData) {
		_base = unitData.baseStats;
		_growth = unitData.growthStats;

		currentStats.hitPoints = MaxHP;
		currentStats.manaPoints = MaxMP;
	}

	public float GetShootDelay(WeaponData weapon) {
		return 1.0f / AttackSpeed * weapon.rof;
	}

	public void HandleDamage(Damage incoming_dmg) {
		float total_damage = incoming_dmg.CalculateDamage() - Mathf.Pow(Defense, 1.1f);
		if (total_damage < 0)
			total_damage = 1;

		Debug.Log($"Total: {total_damage}; Incoming Damage: {incoming_dmg.CalculateDamage()}; Defense: {Mathf.Pow(Defense, 1.2f)}");

		currentStats.hitPoints -= total_damage;
	}
}
