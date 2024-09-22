using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class InventoryInput : MonoBehaviour {
	private Camera mainCamera;
	protected enum InteractableState { Idle, Dragging, Clicked }

	private Dictionary<int, InteractableState> states = new();
	private Vector2 startTapPos;
	private InventoryItem targetInteractable;

	[SerializeField] private float dragThreshold = 0.1f;
	[SerializeField] private LayerMask layerMask;

	private void Awake() {
		mainCamera = Camera.main;
	}

	public void OnClick_Select(EnhancedTouch.Finger finger) {
		Debug.Log($"[{finger.index}] Click");

		ChangeState(finger.index, InteractableState.Clicked);

		Ray ray = mainCamera.ScreenPointToRay(finger.screenPosition);
		RaycastHit2D raycast = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layerMask);

		targetInteractable = raycast.collider?.gameObject?.GetComponent<InventoryItem>();
		startTapPos = mainCamera.ScreenToWorldPoint(finger.screenPosition);
		if (!raycast.collider || targetInteractable == null) return;

		targetInteractable.Click();
	}
	
	public void OnMove_Drag(EnhancedTouch.Finger finger) {
		// Debug.Log($"[{finger.index}] Dragging");

		ChangeState(finger.index, InteractableState.Dragging);

		// Handle Dragging / Moving
		Vector2 move_pos = mainCamera.ScreenToWorldPoint(finger.screenPosition);
		if (Vector2.Distance(startTapPos, move_pos) > dragThreshold && targetInteractable != null) {
			targetInteractable.DragStart();
			targetInteractable.transform.position = new Vector2(move_pos.x, move_pos.y + .5f);
		}

		if (targetInteractable != null)
			targetInteractable.Dragging();
	}
	
	public void OnClick_Rotate(EnhancedTouch.Finger finger) {
		Debug.Log(targetInteractable == null);

		if (targetInteractable == null)
			return;

		ChangeState(finger.index, InteractableState.Clicked);

		Debug.Log($"[{finger.index}] Rotate");
		
		targetInteractable.ToggleRotate();
	}

	public void OnRelease_Release(EnhancedTouch.Finger finger) {
		Debug.Log($"[{finger.index}] Release");

		if (states[finger.index] == InteractableState.Dragging && targetInteractable != null) {
			targetInteractable.DragEnd();
			Debug.Log("Drag End");
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
