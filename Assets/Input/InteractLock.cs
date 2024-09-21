using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class InteractLock : MonoBehaviour {
	protected Interactable snappedInteractable;
	public Interactable SnappedInteractable { get { return snappedInteractable; } }

	[SerializeField] private Transform point;
	[SerializeField] private InteractLockEvents events;

	[System.Serializable]
	public struct InteractLockEvents {
		public UnityEvent OnAttach;
		public UnityEvent OnDetach;
	}

	public bool IsOccupied() {
		return snappedInteractable != null;
	}

	public virtual void Attach(Interactable interactable) {
		snappedInteractable = interactable;

		// Place Object in the spawnPoint
		interactable.UpdateSnapPosition(this);

		events.OnAttach?.Invoke();
	}

	public virtual void Detach() {
		events.OnDetach?.Invoke();

		snappedInteractable = null;
	}

	public Vector2 GetSnapPoint() {
		return point.position;
	}
}