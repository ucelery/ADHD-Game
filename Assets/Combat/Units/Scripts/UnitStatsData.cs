using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Stats Data", menuName = "Combat/Unit Stats")]
public class UnitStatsData : ScriptableObject {
	public float hitPoints;
	public float hpRegen;
	public float manaPoints;
	public float mpRegen;
	public float defense;
	public float attack;
	public float attackSpeed;
	public float speed;

	[Range(0f, 1f)] public float dodgeChance;
	[Range(0f, 1f)] public float critChance;
	public float critDamage;
}
