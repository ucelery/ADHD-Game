using System.Collections;
using System.Collections.Generic;
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

	[Header("Inventory"), Tooltip("Only assign if the unit is non-playable or for testing")]
	public List<ItemData> inventory;
}
