using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private WeaponData weapon;
	[SerializeField] private float baseAttackSpeed;
	[SerializeField] private float attackSpeed;

	private float fireDelay;

	private void Start() {
		this.fireDelay = weapon.rof;
	}

	private void Update() {
		if (fireDelay > 0) {
			fireDelay -= Time.deltaTime;
		}

		if (Input.GetMouseButton(0)) {
			if (fireDelay <= 0) {
				ShootProjectile(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
		}
	}

	void ShootProjectile(Vector3 shootDirection) {
		foreach (ProjectileData proj in weapon.projectiles) {
			GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
			Projectile projectileComp = projectile.GetComponent<Projectile>();
			projectileComp.Initialize(proj, shootDirection.normalized);
		}

		fireDelay = 1.0f / (baseAttackSpeed * weapon.rof * attackSpeed);

		Debug.Log($"1.0f / Base: {baseAttackSpeed}; RoF: {weapon.rof}; AtkSpd: {attackSpeed}; {fireDelay}");
	}
}
