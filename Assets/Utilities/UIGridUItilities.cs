using UnityEngine;

namespace Utilities.UIGrid {
	public enum GridObjectStates {
		Unattached, Attached, Clicked, Dragging
	}

	public class UIGridUtility {
		public static Vector2 GetGridPosition(RectTransform child, RectTransform gridManager) {
			// Get the world position of the child RectTransform
			Vector3 childWorldPosition = child.position;

			// Convert the world position to the grandparent's local space
			Vector3 relativePosition = gridManager.InverseTransformPoint(childWorldPosition);

			return new Vector2(relativePosition.x, relativePosition.y);
		}

		public static Vector2 GetGridPosition(Vector2 child, RectTransform gridManager) {
			Vector3 childWorldPosition = child;

			Vector3 relativePosition = gridManager.InverseTransformPoint(childWorldPosition);

			return new Vector2(relativePosition.x, relativePosition.y);
		}
	}
}