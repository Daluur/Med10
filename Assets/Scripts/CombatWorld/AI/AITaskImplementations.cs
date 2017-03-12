using System.Collections;
using System.Collections.Generic;
using CombatWorld;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;

public class AITaskImplementations : MonoBehaviour {

	public static void MoveTo(Node node, Unit unit) {
		Debug.Log("moving to: " + node);
		unit.Move(node);
	}

	public static void MoveToAndAttack(Node moveTo, Node toAttack, Unit unit) {
		unit.Move(moveTo);
		if (toAttack.HasOccupant() && toAttack.GetOccupant().GetTeam() != Team.AI) {
			unit.Attack(toAttack.GetOccupant());
		}
	}
}
