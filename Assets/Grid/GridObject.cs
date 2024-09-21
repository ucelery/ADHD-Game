using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {
	public static List<GridObject> gridObjects = new();

	protected virtual void OnEnable() {
		if (!gridObjects.Contains(this))
			gridObjects.Add(this);
	}

	protected virtual void OnDisable() {
		if (gridObjects.Contains(this))
			gridObjects.Remove(this);
	}

	public static List<Vector2Int> GetObjectsPosition() {
		List<Vector2Int> list = new();
		foreach (GridObject obj in gridObjects) {
			list.Add(obj.GetGridPosition());
		}

		return list;
	}

	public Vector2Int GetGridPosition() {
		return Vector2Int.RoundToInt(transform.position);
	}
}