using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Units;

[CreateAssetMenu(fileName = "Unit Behaviour", menuName = "Combat/Units/Unit Behaviour/Behaviour")]
public class UnitBehaviour : ScriptableObject {
	// Target Priority
	public bool canChangeTargets;
	public PriorityData priority;

	// Movement Behaviour
	public MovementData patrolMovement;
	public MovementData combatMovement;
}
