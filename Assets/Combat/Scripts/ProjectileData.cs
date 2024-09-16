using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "Combat/Projectile Data")]
public class ProjectileData : ScriptableObject {
	public bool isReversed;
	public float speed = 5f;
	public float frequency = 2f;
	public float amplitude = 0.5f;
	public float lifespan = 1.5f;
	public float directionOffset;
}
