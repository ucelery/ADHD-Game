using UnityEngine.Events;

namespace Utilities.Interactable {
	[System.Serializable]
	public struct InteractableProperties {
		public bool draggable;
		public bool clickable;
	}

	[System.Serializable]
	public struct InteractableEvents {
		public UnityEvent OnClick;
		public UnityEvent OnDragStart;
		public UnityEvent OnDragEnd;
		public UnityEvent OnDragging;
	}
}