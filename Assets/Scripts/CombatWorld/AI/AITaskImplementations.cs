using System.Collections;
using System.Collections.Generic;
using System.IO;
using CombatWorld;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;

public class AITaskImplementations {

	private static Pathfinding pathFinding = new Pathfinding();

	public static void DoAIAction(AIUnit unit, AITask task) {
		switch (task.task) {
			case AICalculateScore.PossibleTasks.Attack:
				break;
			case AICalculateScore.PossibleTasks.MoveAttack:
				MoveToAndAttack(task.endNode, task.toAttack, unit.myUnit);
				break;
			case AICalculateScore.PossibleTasks.MoveDefensive:
				MoveTo(task.endNode, unit.myUnit);
				break;
			case AICalculateScore.PossibleTasks.MoveFromSpawn:
				break;
			case AICalculateScore.PossibleTasks.MoveOffensive:
				MoveTo(task.endNode, unit.myUnit);
				break;
			case AICalculateScore.PossibleTasks.Stay:
				break;
			case AICalculateScore.PossibleTasks.TurnShadow:
				break;
			case AICalculateScore.PossibleTasks.TurnStone:
				break;
			default:
				Debug.LogError("Not implemented yet for this type of action");
				break;

		}

	}

	private static void MoveTo(Node moveTo, Unit unit) {
		Debug.Log("moving to: " + moveTo);
		unit.Move(pathFinding.GetPathFromTo(unit.GetNode(), moveTo));
	}

	private static void MoveToAndAttack(Node moveTo, Node toAttack, Unit unit) {
		unit.Move(pathFinding.GetPathFromTo(unit.GetNode(), moveTo));
		if (toAttack.HasOccupant() && toAttack.GetOccupant().GetTeam() != Team.AI) {
			unit.Attack(toAttack.GetOccupant());
		}
	}
}
