using UnityEngine;

namespace Utilities.Behaviour {
	public enum MovementAxis {
		Self, Target, TargetBehind, TargetFront
	}

	[System.Serializable]
	public struct Movement {
		public Vector2 point;
		public MovementAxis axis;
		public float delay; // delay between move points
		public bool isInstant;
	}
}