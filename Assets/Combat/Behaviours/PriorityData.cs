using UnityEngine;
using Utilities.Behaviour;
using Utilities.Units;

[CreateAssetMenu(fileName = "Target Priority Data", menuName = "Combat/Units/Unit Behaviour/Target Priority")]
public class PriorityData : ScriptableObject {
	[SerializeField, Range(-1f, 100f)] 
	private float closestToHP = -1; // Percentage, -1 means exclude
	[SerializeField, Range(-1f, 100f)] 
	private float closestToMP = -1; // Percentage, -1 means exclude

	[SerializeField] private UnitType unitType;
	[SerializeField] private UnitRace unitRace;

	public Unit GetPriorityUnit() {
		Unit unit = null;

		// Get closest to HP
		if (closestToHP >= 0) {
			float current_lowest_diff = float.PositiveInfinity;
			foreach (Unit unit_item in UnitPooling.Instance.ActiveUnits[unitType]) {
				// Convert unit hp to percentage current / maxHp * 100
				float current_hp_percent = unit_item.CurrentStats.HP / unit_item.CurrentStats.MaxHP;

				if (Mathf.Abs(current_hp_percent) < current_lowest_diff) {
					current_lowest_diff = current_hp_percent;
					unit = unit_item;
				}
			}
		}

		// Get closest to MP
		else if (closestToMP >= 0) {
			float current_lowest_diff = float.PositiveInfinity;
			foreach (Unit unit_item in UnitPooling.Instance.ActiveUnits[unitType]) {
				// Convert unit hp to percentage current / maxHp * 100
				float current_mp_percent = unit_item.CurrentStats.MP / unit_item.CurrentStats.MaxMP;

				if (Mathf.Abs(current_mp_percent) < current_lowest_diff) {
					current_lowest_diff = current_mp_percent;
					unit = unit_item;
				}
			}
		}

		// Get the closest
		else {

		}

		return unit;
	}
}