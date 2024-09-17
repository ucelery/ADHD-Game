using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
	[SerializeField] private UnitStatsData _base;
	[SerializeField] private UnitStatsData _growth;
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private WeaponData weapon;
	[SerializeField] private LevelHandler levelHandler;

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

		// Simulate gaining XP (for testing purposes)
		if (Input.GetKeyDown(KeyCode.X)) {
			levelHandler.GainXP(50);  // Add 50 XP when pressing "X"
		}
	}

	void ShootProjectile(Vector3 shootDirection) {
		foreach (ProjectileData proj in weapon.projectiles) {
			GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
			Projectile projectileComp = projectile.GetComponent<Projectile>();
			projectileComp.Initialize(proj, shootDirection.normalized);
		}

		fireDelay = 1.0f / (_base.attackSpeed * weapon.rof * (1 + _growth.attackSpeed * levelHandler.Level));

		Debug.Log($"1.0f / Base: {_base.attackSpeed}; RoF: {weapon.rof}; AtkSpd: {(_growth.attackSpeed * levelHandler.Level)}; {fireDelay}");
	}
}
