using UnityEngine;

[System.Serializable]
public class ItemTracker {
	private ItemData item;
	private float fireRate;
	private float lastFiredTime;

	public ItemData ItemData { get { return item; } }

	public ItemTracker(ItemData item, float delay) {
		this.item = item;
		fireRate = delay;
		lastFiredTime = -fireRate;
	}

	public bool CanActivate() {
		if (Time.time >= lastFiredTime + fireRate) {
			lastFiredTime = Time.time;
			return true;
		}

		return false;
	}
}
