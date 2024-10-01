using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Units;

public class Projectile : MonoBehaviour {
	public UnityEvent OnDespawn;

	[SerializeField]
	private ProjectileData projectileData;
	private Vector3 direction = Vector3.right; // Direction to shoot the projectile

	private Vector3 perpendicular; // Perpendicular to the direction (for wavy movement)
	private Vector3 startPosition; // Initial position of the projectile
	private float startTime;
	private float currentLifeSpan;

	[SerializeField]
	private Damage damage;

	private void OnEnable() {
		TickSpeed.HitTickEvent += CheckCollision;
	}

	private void OnDisable() {
		TickSpeed.HitTickEvent -= CheckCollision;
	}

	void Update() {
		if (projectileData == null) return;

		// Calculate the time-based wavy offset
		float waveOffset = Mathf.Sin((Time.time - startTime) * projectileData.frequency) * projectileData.amplitude;
		if (projectileData.isReversed)
			waveOffset = -waveOffset;

		// Move forward in the given direction
		Vector3 forwardMovement = direction * projectileData.speed * Time.deltaTime;

		// Calculate the new position
		// Move forward and add wavy movement
		Vector3 currentTrajectoryPosition = startPosition + direction * (Vector3.Dot(transform.position - startPosition, direction) + forwardMovement.magnitude);
		Vector3 wavyMovement = perpendicular * waveOffset;
		transform.position = currentTrajectoryPosition + wavyMovement;

		// Update lifespan
		currentLifeSpan -= Time.deltaTime;
		if (currentLifeSpan < 0) {
			Despawn();
		}
	}

	public void Initialize(ProjectileData projectileData, Damage damage, Vector2 direction) {
		this.damage = damage;
		this.projectileData = projectileData;
		this.direction = direction;
		this.currentLifeSpan = projectileData.lifespan;

		this.direction = Quaternion.Euler(0, 0, projectileData.directionOffset) * direction;
		this.direction.Normalize();

		perpendicular = new Vector3(-this.direction.y, this.direction.x, 0).normalized;

		startPosition = damage.origin.transform.position;
		transform.position = startPosition;
		startTime = 0;
	}

	private void Despawn() {
		gameObject.SetActive(false);
		OnDespawn?.Invoke();
	}

	private void HandleCollision(Unit other) {
		if (damage == null || other == null) return;

		// Damage target
		other.TakeDamage(damage);

		Despawn();
	}

	private void OnDrawGizmosSelected() {
		if (projectileData == null) return;

		Gizmos.DrawWireSphere(transform.position, projectileData.projectileRadius);
	}

	private void CheckCollision() {
		float proj_radius_sqr = projectileData.projectileRadius * projectileData.projectileRadius;

		List<Unit> targets = UnitPooling.Instance.ActiveUnits[damage.origin.UnitData.target];
		foreach (Unit enemy in targets) {
			// ignore if one of the targets is its owner
			if (enemy == damage.origin) continue;

			// Get the distance between two units
			float distanceSqr = (transform.position - enemy.transform.position).sqrMagnitude;
			float combinedRadius = projectileData.projectileRadius + enemy.UnitData.hitboxRadius;

			if (distanceSqr <= combinedRadius * combinedRadius) {
				// Handle collision
				HandleCollision(enemy);
			}
		}
	}

	private void OnDrawGizmos() {
		if (projectileData != null) {
			Gizmos.DrawWireSphere(transform.position, projectileData.projectileRadius);
		}
	}
}
