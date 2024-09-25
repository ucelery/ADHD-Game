using System.Collections.Generic;
using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Utilities.Inventory;

public class InventoryInput : MonoBehaviour {
	private Camera mainCamera;
	protected enum InteractableState { Idle, Dragging, Clicked }

	private Dictionary<int, InteractableState> states = new();
	private Vector2 startTapPos;
	private GridObject targetInteractable;

	[SerializeField] private float dragThreshold = 0.1f;
	[SerializeField] private LayerMask layerMask;

	private void Awake() {
		mainCamera = Camera.main;
	}

	public void OnClick_Select(EnhancedTouch.Finger finger) {
		ChangeState(finger.index, InteractableState.Clicked);

		Ray ray = mainCamera.ScreenPointToRay(finger.screenPosition);
		RaycastHit2D raycast = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layerMask);

		targetInteractable = raycast.collider?.gameObject?.GetComponent<GridObject>();

		if (targetInteractable != null) {
			Debug.Log(targetInteractable.gameObject.name);
		}

		if (targetInteractable != null && 
			(targetInteractable as InventoryItem).Item.type == ItemType.Backpack && 
			(targetInteractable as InventoryItem).HasItemAbove()) {
			targetInteractable = null;
		}

		startTapPos = mainCamera.ScreenToWorldPoint(finger.screenPosition);
		if (!raycast.collider || targetInteractable == null) return;

		targetInteractable.Click();
	}
	
	public void OnMove_Drag(EnhancedTouch.Finger finger) {
		// Debug.Log($"[{finger.index}] Dragging");

		// Handle Dragging / Moving
		Vector2 move_pos = mainCamera.ScreenToWorldPoint(finger.screenPosition);
		if (Vector2.Distance(startTapPos, move_pos) > dragThreshold && targetInteractable != null && states[finger.index] != InteractableState.Dragging) {
			ChangeState(finger.index, InteractableState.Dragging);
			targetInteractable.DragStart();
		}

		if (targetInteractable != null && states[finger.index] == InteractableState.Dragging) {
			targetInteractable.transform.position = new Vector2(move_pos.x, move_pos.y + .5f);
			targetInteractable.Dragging();
		}
	}
	
	public void OnClick_Rotate(EnhancedTouch.Finger finger) {
		Debug.Log(targetInteractable == null);

		if (targetInteractable == null)
			return;

		ChangeState(finger.index, InteractableState.Clicked);
		
		targetInteractable.ToggleRotate();
	}

	public void OnRelease_Release(EnhancedTouch.Finger finger) {
		if (states[finger.index] == InteractableState.Dragging && targetInteractable != null) {
			targetInteractable.DragEnd();
		}

		ChangeState(finger.index, InteractableState.Idle);
	}

	private void ChangeState(int fingerIndex, InteractableState newState) {
		if (!states.ContainsKey(fingerIndex)) {
			states.Add(fingerIndex, newState);
		}

		states[fingerIndex] = newState;
	}
}
