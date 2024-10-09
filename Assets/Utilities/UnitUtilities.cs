using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Units {
	public enum UnitType { Ally, Enemy, Neutral }

	[System.Flags]
	public enum UnitRace {
		Nothing = 0,
		Everything = 1 << 0,
		Human = 1 << 1,
		Goblin = 1 << 2,
		Orc = 1 << 3,
		Elf = 1 << 4,
		Dwarf = 1 << 5,
		Troll = 1 << 6,
		Undead = 1 << 7,
		Beastman = 1 << 8,
		Dragonkin = 1 << 9,
		Vampire = 1 << 10,
		Angel = 1 << 11,
		Demon = 1 << 12,
		Summon = 1 << 13,
	}

	[System.Serializable]
	public struct UnitEvents {
		public UnityEvent<Unit> OnDespawn;
		public UnityEvent<ItemData> OnShoot;
		public UnityEvent OnTakeDamage;
	}

	[System.Serializable] 
	public struct PoolingEvents {
		public UnityEvent<Unit> OnSpawn;
		public UnityEvent<Unit> OnDespawn;
	}

	public enum UnitState {
		Idle, Patrolling, TargetDetected, InCombat, Dead
	}

	public enum SpawnType {
		Constant, Batches, Group, Circle
	}

	[System.Serializable]
	public struct SpawnData {
		public int spawnCap;
		public int spawnAmount;
		[Tooltip("Delay for when the actual spawning begins")]
		public float spawnDelay;

		[Tooltip("Interval between spawns")]
		public float spawnInterval;
		public SpawnType spawnType;
		public UnitData enemy;
	}

	[System.Serializable]
	public struct CurrentStats {
		public float hitPoints;
		public float manaPoints;
	}

    [System.Serializable]
    public struct LevelEvents {
		public UnityEvent OnLevelUp;
		public UnityEvent OnExperienceGain;
	}
}