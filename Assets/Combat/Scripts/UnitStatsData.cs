using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Stats Data", menuName = "Combat/Unit Stats")]
public class UnitStatsData : ScriptableObject {
	public float hitPoints;
	public float manaPoints;
	public float speed;
	public float attack;
	public float attackSpeed;
	public float hpRegen;
	public float mpRegen;
	public float defense;
}
