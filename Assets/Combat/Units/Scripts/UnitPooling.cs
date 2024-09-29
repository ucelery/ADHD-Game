using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using Utilities.Units;

public class UnitPooling : MonoBehaviour {
	[Header("UI Elements")]
	[SerializeField] private GameObject unitPrefab;
	[SerializeField] private Transform container;

	[SerializeField] private List<UnitData> unitsToSpawn;

	private Queue<Unit> unitPool = new();
	private Dictionary<UnitType, List<Unit>> active_units = new();

	private static UnitPooling _instance;

	public static UnitPooling Instance {
		get {
			if (_instance == null) {
				// Optionally log an error if the instance isn't found
				Debug.LogError("UnitPooling instance is null! Ensure there is one in the scene.");
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

	private void Start() {
		foreach (UnitData unit in unitsToSpawn) {
			SpawnUnit(unit);
		}
	}

	public void SpawnUnit(UnitData new_unit) {
		Unit unit = null;

		// Recycle Projectiles if there are unused projectiles
		if (unitPool.Count > 0) {
			unit = unitPool.Dequeue();
			unit.gameObject.SetActive(true);
		} else {
			// if there are not enough in the pool, make more
			GameObject unit_go = Instantiate(unitPrefab, container);
			unit = unit_go.GetComponent<Unit>();
		}

		unit.Initialize(new_unit);

		if (!active_units.ContainsKey(new_unit.type))
			active_units.Add(new_unit.type, new List<Unit>());

		active_units[new_unit.type].Add(unit);

		unit.Events.OnDespawn.AddListener((unit) => {
			// When this projectile despawns add it back to queue and reset listeners
			unitPool.Enqueue(unit);
			active_units[new_unit.type].Remove(unit);
			unit.Events.OnDespawn.RemoveAllListeners();
		});
	}
}
