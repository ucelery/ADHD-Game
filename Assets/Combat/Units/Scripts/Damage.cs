using UnityEngine;
using static UnityEngine.UI.Image;

[System.Serializable]
public class Damage {
	public Unit origin;
	public WeaponData weapon;

	public Damage(Unit origin, WeaponData weapon) {
		this.origin = origin;
		this.weapon = weapon;
	}

	public float CalculateDamage() {
		float rand_weap_dmg = Random.Range(weapon.minDamage, weapon.maxDamage);
		float attack_multi = (Mathf.Pow(origin.CurrentStats.Attack, 1.2f) / 100) + 1;

		return rand_weap_dmg * attack_multi;
	}
}