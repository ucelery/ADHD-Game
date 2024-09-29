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
}
