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

	private static Queue<AIUnit> tasksToComplete = new Queue<AIUnit>();

	private static bool performingAction = false;

	private static Coroutine waiting;
	private static bool isRunning;

	public static void DoAIAction(AIUnit unit, AITask task) {
		unit.SetTaskToDo(task);
		tasksToComplete.Enqueue(unit);

		if(!isRunning){
			waiting = unit.myUnit.StartCoroutine(WaitForAction(unit));
		}
	}


	private static void DoAction(AIUnit unit) {
		var task = unit.taskToDo;
		switch (task.task) {
			case AICalculateScore.PossibleTasks.Attack:
				Attack(task.toAttack, unit.myUnit);
				break;
			case AICalculateScore.PossibleTasks.MoveAttack:
				MoveToAndAttack(task.endNode, task.toAttack, unit.myUnit);
				break;
			case AICalculateScore.PossibleTasks.MoveAttackDefensive:
				MoveToAndAttack(task.endNode, task.toAttack, unit.myUnit);
				break;
			case AICalculateScore.PossibleTasks.MoveDefensive:
				MoveTo(task.endNode, unit.myUnit);
				break;
			case AICalculateScore.PossibleTasks.MoveBlock:
				MoveTo(task.endNode, unit.myUnit);
				break;
			case AICalculateScore.PossibleTasks.MoveOffensive:
				MoveTo(task.endNode, unit.myUnit);
				break;
			case AICalculateScore.PossibleTasks.Stay:
				break;
			case AICalculateScore.PossibleTasks.MoveFromSpawn:
				MoveTo(task.endNode, unit.myUnit);
				break;
			default:
				Debug.LogError("Not implemented yet for this type of action");
				break;

		}
		unit.ClearTasks();

	}

	//TODO: Make it evaluate after each action, not before to ensure movement does not try to stack on eachother


	private static IEnumerator WaitForAction(AIUnit unit) {
		isRunning = true;
		while(tasksToComplete.Count>0){
			var unitDoAction = tasksToComplete.Dequeue();
			if (unitDoAction.taskToDo != null) {
				while(GameController.instance.WaitingForAction()){
					yield return new WaitForSeconds(0.1f);
				}

				DoAction(unitDoAction);
			}
			else {
				Debug.Log("Task was null");
			}
		}
		isRunning = false;
		yield return null;
	}


	private static void MoveTo(Node moveTo, Unit unit) {
		Debug.Log("moving to: " + moveTo);
		unit.Move(pathFinding.GetPathFromTo(unit.GetNode(), moveTo));
	}

	private static void MoveToAndAttack(Node moveTo, Node toAttack, Unit unit) {
		performingAction = true;
		Debug.Log("Move and attack");
		unit.Move(pathFinding.GetPathFromTo(unit.GetNode(), moveTo), true);
		unit.StartCoroutine(MoveAndAttackWait(unit, toAttack));
	}

	private static void Attack(Node toAttack, Unit unit) {
		Debug.Log("Attack");
		unit.Attack(toAttack.GetOccupant());
	}

	private static IEnumerator MoveAndAttackWait(Unit unit, Node toAttack) {
		while (unit.doingAction) {
			yield return new WaitForSeconds(0.1f);
		}
		unit.Attack(toAttack.GetOccupant());
		performingAction = false;
		yield return null;
	}

	public static bool GetPerformingAction() {
		return performingAction;
	}
}
