using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Combat/Weapon Data")]
public class WeaponData : ScriptableObject {
    public ProjectileData[] projectiles;
	public float rof = 1f; // rate of fire
}
