using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Units;

[CreateAssetMenu(fileName = "Level Data", menuName = "Combat/Level Data")]
public class LevelData : ScriptableObject {
	public float spawnRate = 1f;
	public List<SpawnData> spawns;
}
