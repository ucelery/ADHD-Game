using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
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
	}

	[SerializeField]
	private InteractableProperties interactableProperties;

	[SerializeField]
	private InteractableEvents interactableEvents;

	public InteractableProperties Props { get { return interactableProperties; } }

	private List<InteractLock> lockPoints = new();
	private InteractLock currentInteractLock;
	private Vector3 previousPosition;

	protected virtual void Start() {
		previousPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}

	public virtual void Click() {
		interactableEvents.OnClick.Invoke();
	}

	public virtual void DragStart() {
		interactableEvents.OnDragStart.Invoke();
	}

	public virtual void DragEnd() {
		interactableEvents.OnDragEnd.Invoke();

		HandleDrop();
	}

	private void HandleDrop() {
		// Drop
		float closestDistance = Mathf.Infinity;
		InteractLock closestPoint = null;
		foreach (InteractLock lp in lockPoints) {

			if (lp.IsOccupied()) continue;

			float distance = Vector2.Distance(transform.position, lp.transform.position);
			if (distance < closestDistance) {
				closestDistance = distance;
				closestPoint = lp;
			}
		}

		// Successful drag
		if (closestPoint != null) {
			if (currentInteractLock != null) {
				currentInteractLock.Detach();
				currentInteractLock = closestPoint;
			}

			currentInteractLock = closestPoint;
		}

		// Reattach to previous InteractLock / position
		if (currentInteractLock != null) {
			currentInteractLock.Attach(this);
		} else {
			transform.position = previousPosition;
		}
	}

	public void UpdateSnapPosition(InteractLock il) {
		currentInteractLock = il;

		previousPosition = il.GetSnapPoint();
		transform.position = il.GetSnapPoint();
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision) {
		InteractLock il = collision.gameObject.GetComponent<InteractLock>();
		Debug.Log($"Enter {il}");
		if (il != null && !lockPoints.Contains(il)) {
			lockPoints.Add(il);
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D collision) {
		InteractLock il = collision.gameObject.GetComponent<InteractLock>();
		Debug.Log($"Exit {il}");
		if (il != null && lockPoints.Contains(il)) {
			lockPoints.Remove(il);
		}
	}
}