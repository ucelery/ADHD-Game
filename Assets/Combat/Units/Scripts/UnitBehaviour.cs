using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : ScriptableObject {
	[SerializeField] private List<Movement> patrol;

	public struct Movement {
		public Vector2 point;
		public float speed;
	}
}
