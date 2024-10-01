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
		Demon = 1 << 12
	}

	[System.Serializable]
	public struct UnitEvents {
		public UnityEvent<Unit> OnDespawn;
		public UnityEvent<ItemData> OnShoot;
	}

	[System.Serializable] 
	public struct PoolingEvents {
		public UnityEvent<Unit> OnSpawn;
		public UnityEvent<Unit> OnDespawn;
	}

	public enum UnitState {
		Idle, Patrolling, InCombat, Dead
	}

	[System.Serializable]
	public class Damage {
		public Unit origin;

		public Damage(Unit origin) {
			this.origin = origin;
		}

		public float CalculateDamage() {
			// Implement damage formula here
			return 0f;
		}
	}
}