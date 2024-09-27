using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class InventoryInput : MonoBehaviour {
	[SerializeField] private GraphicRaycaster raycaster;

	private Camera mainCamera;

	private void Awake() {
		mainCamera = Camera.main;
	}

	public void Click(EnhancedTouch.Finger finger) {
		
	}

	public void StartDrag(EnhancedTouch.Finger finger) {

	}

	public void Drag(EnhancedTouch.Finger finger) {

	}

	public void Release(EnhancedTouch.Finger finger) {

	}
}
