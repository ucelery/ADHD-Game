using UnityEngine;

public class Projectile : MonoBehaviour {
	private ProjectileData projectileData;
	public Vector3 direction = Vector3.right; // Direction to shoot the projectile

	private Vector3 perpendicular; // Perpendicular to the direction (for wavy movement)
	private Vector3 startPosition; // Initial position of the projectile
	private float startTime;
	private float currentLifeSpan;

	void Start() {
		// Normalize the direction to ensure uniform speed
		direction.Normalize();

		// Calculate the perpendicular vector for the wavy movement
		perpendicular = new Vector3(-direction.y, direction.x, 0).normalized;

		// Store the start position and start time
		startPosition = transform.position;
		startTime = Time.time;
	}

	void Update() {
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
			Destroy(gameObject);
		}
	}

	public void Initialize(ProjectileData projectileData, Vector2 direction) {
		this.projectileData = projectileData;
		this.direction = direction;
		this.currentLifeSpan = projectileData.lifespan;

		this.direction = Quaternion.Euler(0, 0, projectileData.directionOffset) * direction;
	}
}
