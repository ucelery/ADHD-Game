using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Interactable;

public class Interactable : MonoBehaviour {
	[Header("Interactable")]

	[SerializeField]
	private InteractableProperties interactableProperties;

	[SerializeField]
	private InteractableEvents interactableEvents;

	public InteractableProperties Props { get { return interactableProperties; } }

	protected List<InteractLock> lockPoints = new();
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

	public virtual void Dragging() {
		interactableEvents.OnDragging.Invoke();
	}

	public virtual void DragEnd() {
		interactableEvents.OnDragEnd.Invoke();

		HandleDrop();
	}

	protected virtual void HandleDrop() {
		// Drop
		InteractLock closestPoint = GetClosestLock(lockPoints, transform.position);

		// Successful drag
		if (closestPoint != null) {
			if (currentInteractLock != null) {
				currentInteractLock.Detach();
				currentInteractLock = closestPoint;
			}

			currentInteractLock = closestPoint;
		}

		InvalidDrop();
	}

	protected void InvalidDrop() {
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

	protected InteractLock GetClosestLock(List<InteractLock> locks, Vector3 position) {
		float closestDistance = Mathf.Infinity;
		InteractLock closestPoint = null;
		foreach (InteractLock lp in locks) {

			if (lp.IsOccupied()) continue;

			float distance = Vector2.Distance(position, lp.transform.position);
			if (distance < closestDistance) {
				closestDistance = distance;
				closestPoint = lp;
			}
		}

		return closestPoint;
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision) {
		InteractLock il = collision.gameObject.GetComponent<InteractLock>();
		if (il != null && !lockPoints.Contains(il)) {
			lockPoints.Add(il);
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D collision) {
		InteractLock il = collision.gameObject.GetComponent<InteractLock>();
		if (il != null && lockPoints.Contains(il)) {
			lockPoints.Remove(il);
		}
	}
}