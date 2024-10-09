using Unity.VisualScripting;
using UnityEngine;
using Utilities.Behaviour;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "Movement Data", menuName = "Combat/Units/Unit Behaviour/Movement")]
public class MovementData : ScriptableObject {
	public Movement[] movement;

	public Vector2 GetPosition(Unit self, Unit target, int index) {
		Vector2 pos = default;

		switch (movement[index].axis) {
			case MovementAxis.Self:
				pos = (Vector2)self.transform.position + movement[index].point;
				break;
			case MovementAxis.Target:
				pos = (Vector2)target.transform.position + movement[index].point;
				break;
			case MovementAxis.TargetBehind: {
					Vector2 behind = (Vector2)target.transform.position - target.Direction.normalized;
					pos = behind;
					break;
				}
			case MovementAxis.TargetFront: {
					Vector2 forward = (Vector2)target.transform.position + target.Direction.normalized;
					pos = forward;
					break;
				}
		}

		return pos;
	}
}
