using System.Collections;
using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;
using UnityEngine;

namespace CombatWorld.AI {
	public class AITaskImplementations {

		private static Pathfinding pathFinding = new Pathfinding();

		private static Queue<AIUnit> tasksToComplete = new Queue<AIUnit>();

		private static bool performingAction = false;

		private static bool isRunning;

		public static bool isStoneEncounter;

		public static void DoAIAction(AIUnit unit, AITask task) {
			unit.SetTaskToDo(task);
			tasksToComplete.Enqueue(unit);

			if(!isRunning){
				unit.myUnit.StartCoroutine(WaitForAction(unit));
			}
		}


		private static void DoAction(AIUnit unit) {
			var task = unit.taskToDo;
			switch (task.task) {
				case PossibleTasks.Attack:
					Attack(task.toAttack, unit.myUnit);
					break;
				case PossibleTasks.MoveAttack:
					MoveToAndAttack(task.endNode, task.toAttack, unit.myUnit);
					break;
				case PossibleTasks.MoveAttackDefensive:
					MoveToAndAttack(task.endNode, task.toAttack, unit.myUnit);
					break;
				case PossibleTasks.MoveDefensive:
					MoveTo(task.endNode, unit.myUnit);
					break;
				case PossibleTasks.MoveBlock:
					MoveTo(task.endNode, unit.myUnit);
					//if (unit.myUnit.IsStoneUnit())
					//	unit.myUnit.StartCoroutine(TurnToStone(unit.myUnit));
					break;
				case PossibleTasks.MoveOffensive:
					MoveTo(task.endNode, unit.myUnit);
					break;
				case PossibleTasks.Stay:
					break;
				case PossibleTasks.MoveFromSpawn:
					MoveTo(task.endNode, unit.myUnit);
					break;
				default:
					Debug.LogError("Not implemented yet for this type of action");
					break;

			}
			if (unit.myUnit.IsStoneUnit() && isStoneEncounter && (task.task != PossibleTasks.MoveAttack && task.task != PossibleTasks.Attack && task.task != PossibleTasks.MoveAttackDefensive)) {
				var shouldTurn = true;
				foreach (var neighbor in task.endNode.neighbours) {
					if (neighbor.HasTower() || task.endNode.GetType() == typeof(SummonNode)) {
						shouldTurn = false;
						break;
					}
				}
				if(shouldTurn)
					unit.myUnit.StartCoroutine(TurnToStone(unit.myUnit));
			}
			unit.ClearTasks();

		}

		public static void IsStoneEncounter(DeckData deck) {
			if (deck.type1 == "Stone" && deck.type2 == "") {
				//Debug.Log("Stone encounter engaged");
				isStoneEncounter = true;
			}
			else {
				isStoneEncounter = false;
			}
		}

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
//			Debug.Log("moving to: " + moveTo);
			var path = unit.GetShadow() ? pathFinding.GetPathFromToWithoutOccupants(unit.GetNode(), moveTo) : pathFinding.GetPathFromTo(unit.GetNode(), moveTo);
			if (path!=null) {
				unit.Move(path);
			}
		}

		private static void MoveToAndAttack(Node moveTo, Node toAttack, Unit unit) {
			performingAction = true;
//			Debug.Log("Move and attack");
			var path = unit.GetShadow() ? pathFinding.GetPathFromToWithoutOccupants(unit.GetNode(), moveTo) : pathFinding.GetPathFromTo(unit.GetNode(), moveTo);
			unit.Move(path, false);
			unit.StartCoroutine(MoveAndAttackWait(unit, toAttack));
		}

		private static void Attack(Node toAttack, Unit unit) {
		//	Debug.Log("Attack");
			unit.Attack(toAttack.GetOccupant());
		}

		private static IEnumerator MoveAndAttackWait(Unit unit, Node toAttack) {
			while (GameController.instance.WaitingForAction()) {
				yield return new WaitForSeconds(0.1f);
			}
			unit.Attack(toAttack.GetOccupant());
			performingAction = false;
			yield return null;
		}

		private static IEnumerator TurnToStone(Unit unit) {
			while (GameController.instance.WaitingForAction()) {
				yield return new WaitForSeconds(0.1f);
			}
			unit.TurnToStone();
			yield return null;
		}

		public static bool GetPerformingAction() {
			return performingAction;
		}
	}
}