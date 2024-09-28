using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject {
	[SerializeField] public string playerName;

	[SerializeField] private List<ItemData> items;
	
	public List<ItemData> Items { get { return items; } }
}
