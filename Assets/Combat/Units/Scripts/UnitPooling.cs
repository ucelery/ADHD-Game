using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using Utilities.Units;
using UnityEngine.Events;

public class UnitPooling : MonoBehaviour {
	[Header("UI Elements")]
	[SerializeField] private GameObject unitPrefab;
	[SerializeField] private Transform container;

	private Queue<Unit> unitPool = new();
	private Dictionary<UnitType, List<Unit>> active_units = new();

	private static UnitPooling _instance;

	public Dictionary<UnitType, List<Unit>> ActiveUnits { get { return active_units; } }
	public PoolingEvents Events;
	public static UnitPooling Instance {
		get {
			if (_instance == null) {
				// Optionally log an error if the instance isn't found
				Debug.LogError("UnitPooling instance is null! Ensure there is one in the scene.");
			}

			return _instance;
		}
	}

	private void Awake() {
		if (_instance == null) {
			_instance = this;
		} else if (_instance != this) {
			Destroy(gameObject);
		}

		Events.OnSpawn = new();
		Events.OnDespawn = new();
	}

	public Unit SpawnUnit(UnitData new_unit, Vector2 spawnPosition) {
		Unit unit = null;

		// Recycle Projectiles if there are unused projectiles
		if (unitPool.Count > 0) {
			unit = unitPool.Dequeue();
			unit.gameObject.SetActive(true);
		} else {
			// if there are not enough in the pool, make more
			GameObject unit_go = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, container);
			unit = unit_go.GetComponent<Unit>();
		}

		unit.Initialize(new_unit);

		if (!active_units.ContainsKey(new_unit.type)) {
			active_units.Add(new_unit.type, new List<Unit>());
		}

		active_units[new_unit.type].Add(unit);

		unit.Events.OnDespawn.AddListener((unit) => {
			// When this projectile despawns add it back to queue and reset listeners
			unitPool.Enqueue(unit);
			active_units[new_unit.type].Remove(unit);
			Events.OnDespawn?.Invoke(unit);

			unit.Events.OnDespawn.RemoveAllListeners();
		});

		Events.OnSpawn?.Invoke(unit);

		return unit;
	}
}
