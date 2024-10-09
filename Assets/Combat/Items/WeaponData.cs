using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Units;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Combat/Weapon Data")]
public class WeaponData : ItemData {
	[Header("Weapon Properties")]
    public ProjectileData[] projectiles;
	public float rof = 1f; // rate of fire
	public float range = 7f;
	public UnitBehaviour behaviour;
}
