using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.Interactable;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

/// <summary>
/// Make sure that this interactable is under a UIGridManager for dragging to work properly.
/// Handles dragging and dropping of UI elements.
/// </summary>
public class UIInteractable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

	private Canvas canvas;
	protected RectTransform rectTransform;

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>();
	}

	public virtual void OnBeginDrag(PointerEventData eventData) { }

	public virtual void OnDrag(PointerEventData eventData) {
		rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
	}

	public virtual void OnEndDrag(PointerEventData eventData) { }

	public virtual void OnPointerDown(PointerEventData eventData) { }
}