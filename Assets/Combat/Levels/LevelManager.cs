using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Units;

public class LevelManager : MonoBehaviour {
	[SerializeField] private LevelData currentLevel;
	[SerializeField] private Spawner spawner;

	private void Start() {
		StartCoroutine(Spawn());
	}

	private IEnumerator Spawn() {
		float delay = 1f / currentLevel.spawnRate;
		yield return new WaitForSeconds(delay);

		// currentLevel.spawns[0].enemies for debugging
		SpawnData spawnData = currentLevel.spawns[0];

		switch(spawnData.spawnType) {
			case SpawnType.Constant:
				break;
			case SpawnType.Batches:
				break;
			case SpawnType.Group:
				break;
			case SpawnType.Circle:
				spawner.CircleSpawn(spawnData.enemy, spawnData.spawnAmount, 10f, transform.position);
				break;
		}

		Dictionary<UnitType, List<Unit>> activeUnits = UnitPooling.Instance.ActiveUnits;
		if (activeUnits[UnitType.Enemy].Count < spawnData.spawnCap) {
			StartCoroutine(Spawn());
		}
	}
}
