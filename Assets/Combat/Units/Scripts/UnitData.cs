using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Units;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Combat/Units/Unit Data")]
public class UnitData : ScriptableObject {
	[Header("Unit Properties")]
	public string nickname;
	public Sprite inGameSprite;
	public UnitType type;
	public UnitRace race;
	public UnitType target;
	public float hitboxRadius = 0.5f;

	[Header("Unit Stats")]
	public UnitStatsData baseStats;
	public UnitStatsData growthStats;

	[Header("Inventory"), Tooltip("Only assign if the unit is non-playable or for testing")]
	public List<ItemData> inventory;

	public Unit LookForTargets() {
		Dictionary<UnitType, List<Unit>> targets = UnitPooling.Instance.ActiveUnits;

		if (!targets.ContainsKey(target)) return null;

		if (targets.Count > 0) {
			// Has Targets
			return targets[target].First();
		}

		return null;
	}
}
