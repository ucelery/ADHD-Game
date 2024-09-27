using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.Interactable;

public class InventoryItem : UIInteractable {
	[Header("Interactable")]

	[SerializeField]
	private InteractableProperties interactableProperties;

	[SerializeField]
	private InteractableEvents interactableEvents;

	public InteractableProperties Props { get { return interactableProperties; } }

	private Vector3 previousPosition;
}
