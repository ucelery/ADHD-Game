using System.Collections.Generic;
using UnityEngine;
using Utilities.Units;

[CreateAssetMenu(fileName = "Unit Behaviour", menuName = "Combat/Units/Unit Behaviour")]
public class UnitBehaviour : ScriptableObject {
	[Header("Target Priority")]
	public UnitType[] type;
	public UnitRace[] race;

	[Header("Movement")]
	public Movement[] combatMovePattern;
	public Movement[] patrolMovePattern;

	public struct Movement {
		public bool instant;
		public Vector2 point;
		public float speed;
	}
}
