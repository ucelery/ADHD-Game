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

	public void Initialize(UnitData unitData) {
		_base = unitData.baseStats;
		_growth = unitData.growthStats;

		currentStats.hitPoints = MaxHP;
		currentStats.manaPoints = MaxMP;
	}

	public float GetShootDelay(WeaponData weapon) {
		return 1.0f / (_base.attackSpeed * weapon.rof * (1 + _growth.attackSpeed * levelHandler.Level));
	}

	public void HandleDamaged() {

	}
}
