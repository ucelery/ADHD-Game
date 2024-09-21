using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Interactable {
	private float rotation = 0;
	public void ToggleRotate() {
		switch (rotation) {
			case 0f:
				rotation = 90f;
				break;
			case 90f:
				rotation = 180f;
				break;
			case 180f:
				rotation = 270f;
				break;
			case 270f:
				rotation = 0f;
				break;
		}

		transform.rotation = Quaternion.Euler(0, 0, rotation);
	}
}
