using System.Collections.Generic;
using UnityEngine;
using Utilities.Units;

public class Spawner : MonoBehaviour {
	[SerializeField] private Transform allyContainer;
	[SerializeField] private Transform enemyContainer;

	[Header("Debug REMOVE LATER")]
	[SerializeField] private UnitData spawnUnit;

	private void Start() {
		UnitPooling.Instance.SpawnUnit(spawnUnit, allyContainer.position);
	}

	public void CircleSpawn(UnitData unitData, int amount, float radius, Vector2 center) {
		float angleStep = 360f / amount;

		for (int i = 0; i < amount; i++) {
			float angle = i * angleStep * Mathf.Deg2Rad;

			// Calculate the position of each object on the circle
			float x = Mathf.Cos(angle) * radius;
			float y = Mathf.Sin(angle) * radius;

			Vector2 spawnPosition = new Vector2(x, y) + center;

			UnitPooling.Instance.SpawnUnit(unitData, spawnPosition);
		}
	}
}
