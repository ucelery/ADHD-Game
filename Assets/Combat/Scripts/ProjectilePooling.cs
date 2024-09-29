using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectilePooling : MonoBehaviour {
	[Header("UI Elements")]
	[SerializeField] private GameObject projectilePrefab;

	private Queue<Projectile> projectilePool = new();

	private static ProjectilePooling _instance;

	public static ProjectilePooling Instance {
		get {
			if (_instance == null) {
				// Optionally log an error if the instance isn't found
				Debug.LogError("ProjectilePooling instance is null! Ensure there is one in the scene.");
			}

			return _instance;
		}
	}

	protected virtual void Awake() {
		if (_instance == null) {
			_instance = this;
		} else if (_instance != this) {
			Destroy(gameObject);
		}
	}

	public void Shoot(WeaponData weapon, Unit origin, Vector2 shootDirection) {
		foreach (ProjectileData proj_data in weapon.projectiles) {
			Projectile projectile = null;

			// Recycle Projectiles if there are unused projectiles
			if (projectilePool.Count > 0) {
				projectile = projectilePool.Dequeue();
			} else {
				// if there are not enough in the pool, make more
				GameObject projectile_go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
				projectile = projectile_go.GetComponent<Projectile>();
			}

			projectile.Initialize(proj_data, origin, shootDirection.normalized);
			projectile.OnDespawn.AddListener(() => {
				// When this projectile despawns add it back to queue and reset listeners
				projectilePool.Enqueue(projectile);
				projectile.OnDespawn.RemoveAllListeners();
			});
		}
	}
}
