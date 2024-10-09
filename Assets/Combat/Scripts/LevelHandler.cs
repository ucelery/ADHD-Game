using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Units;

/// <summary>
/// This script handles xp and xp growth, please only increase LOG_BASE variable above 1.5f;
/// increasing the variable increases the amount of xp required for the next one.
/// </summary>
[System.Serializable]
public class LevelHandler {
	private const float LOG_BASE = 1.5f;

	[SerializeField] private int level;
	public int Level { get { return level; } }

	[SerializeField] private float currentXP;
	[SerializeField] private int baseXP = 100;
	[SerializeField] private int requiredXP;

	[Header("Events")]
	public LevelEvents events;

	public LevelHandler() {
		events.OnLevelUp = new();
		events.OnExperienceGain = new();
	}

	public void GainXP(int amount) {
		currentXP += amount;

		// Check if the player has enough XP to level up
		if (currentXP >= requiredXP) {
			LevelUp();
		}

		events.OnExperienceGain?.Invoke();
	}

	private void LevelUp() {
		level++;
		currentXP -= requiredXP;
		requiredXP = CalculateXPRequired(level);

		events.OnLevelUp?.Invoke();
	}

	private int CalculateXPRequired(int level) {
		return Mathf.RoundToInt(baseXP * Mathf.Log(level + 1, LOG_BASE));
	}
}
