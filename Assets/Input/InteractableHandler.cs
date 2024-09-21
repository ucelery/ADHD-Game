using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class InteractableHandler : MonoBehaviour {
	[SerializeField] private Events events;

	[System.Serializable]
	private struct Events {
		public List<UnityEvent<EnhancedTouch.Finger>> OnPress;
		public List<UnityEvent<EnhancedTouch.Finger>> OnMove;
		public List<UnityEvent<EnhancedTouch.Finger>> OnRelease;
	}

	private void OnEnable() {
		EnhancedTouch.TouchSimulation.Enable();
		EnhancedTouch.EnhancedTouchSupport.Enable();
	}

	private void OnDisable() {
		EnhancedTouch.TouchSimulation.Disable();
		EnhancedTouch.EnhancedTouchSupport.Disable();
	}

	private void Start() {
		EnhancedTouch.Touch.onFingerDown += OnInput_Press;
		EnhancedTouch.Touch.onFingerMove += OnInput_Move;
		EnhancedTouch.Touch.onFingerUp += OnInput_Release;
	}

	public void OnInput_Press(EnhancedTouch.Finger finger) {
		if (finger.index < events.OnPress.Count)
			events.OnPress[finger.index]?.Invoke(finger);
	}

	public void OnInput_Move(EnhancedTouch.Finger finger) {
		if (finger.index < events.OnMove.Count)
			events.OnMove[finger.index]?.Invoke(finger);
	}

	public void OnInput_Release(EnhancedTouch.Finger finger) {
		if (finger.index < events.OnRelease.Count)
			events.OnRelease[finger.index]?.Invoke(finger);
	}
}