using System.Collections;
using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;

public class AIUnit {

	public AIUnit(Unit unit, Pathfinding pathfinding) {
		myUnit = unit;
		this.pathFinding = pathfinding;
	}

	public List<AITask> possibleTasks = new List<AITask>();
	public AITask taskToDo;

	public Pathfinding pathFinding = new Pathfinding();

	private List<Node> possibleMoveTo = new List<Node>();

	public Unit myUnit;

	private Node moveTo;
	private List<AICalculateScore.PossibleTasks> waitingTasks = new List<AICalculateScore.PossibleTasks>();
	public bool taskCompleted = false;

	public AICalculateScore AICalcScore;

	public void MyTasks() {
		possibleMoveTo = pathFinding.GetAllNodesWithinDistance(myUnit.GetNode(), myUnit.GetMoveDistance());
		OffensiveMove();
		DefensiveMove();

	}

	private void OffensiveMove() {
		//foreach (var node in possibleMoveTo) {

		if(possibleMoveTo.Count > 0)
			foreach (var move in possibleMoveTo) {

				possibleTasks.Add(new AITask(0,AICalculateScore.PossibleTasks.MoveOffensive, move));
			}

		if(myUnit.CanMove()&&myUnit.CanAttack()){
			foreach (var node in possibleMoveTo) {
				if (node.HasOccupant()) {
					if (node.GetOccupant().GetTeam() != Team.AI) {
						possibleTasks.Add(new AITask(0,AICalculateScore.PossibleTasks.Attack, node));
					}
				}
				else if (!node.HasOccupant()) {
					foreach (var neighborNode in node.neighbours) {
						if (neighborNode.HasOccupant()) {
							if (neighborNode.GetOccupant().GetTeam() != Team.AI) {
								moveTo = node;
								possibleTasks.Add(new AITask(0,AICalculateScore.PossibleTasks.MoveAttack, neighborNode));
							}
						}
					}
				}


				//TODO: How to know if the node is a summon node?
				//if (GetNode()) {
				//}
			}
		}
	}

	private void DefensiveMove() {
		if (myUnit.CanMove()) {
			foreach (var node in possibleMoveTo) {
				//TODO: Get any enemies that can attack now, also see if what I move to is in range of any of the other teams units on the next turn to see if they can attack me?
				//TODO: Where on the map is a node, I want to move closer to tower to defend, but no way of finding it ATM
			}
		}
	}

	public void PerformTask(AITask task) {
		AITaskImplementations.DoAIAction(this, task);
		//DoTask(task.task);
	}

	//TODO: Implement task completion.
	private void DoTask(AICalculateScore.PossibleTasks task) {
		Node node;
		AITask taskToDo;
		Debug.Log(task.ToString());
		switch (task) {
			case AICalculateScore.PossibleTasks.MoveAttack:
				taskToDo = FindTaskByName(possibleTasks, AICalculateScore.PossibleTasks.MoveAttack);
				AITaskImplementations.DoAIAction(this, taskToDo);
				break;
			case AICalculateScore.PossibleTasks.MoveOffensive:
				Debug.Log("Got it");
				taskToDo = FindTaskByName(possibleTasks, AICalculateScore.PossibleTasks.MoveOffensive);
				AITaskImplementations.DoAIAction(this, taskToDo);
				break;
		}
		Debug.Log(myUnit.gameObject.name + " wants to do: " + task.ToString());
		taskCompleted = true;
	}

	public List<AITask> GetTasks() {
		return possibleTasks;
	}

	public AITask FindTaskByName(AICalculateScore.PossibleTasks inTask) {
		foreach (var task in possibleTasks) {
			if (task.task == inTask) {
				return task;
			}
		}
		Debug.LogError("Something went wrong should never reach here, the specified task does not exist in the possible tasks for this unit");
		return null;
	}

	private AITask FindTaskByName(List<AITask> possibleTasks, AICalculateScore.PossibleTasks inTask) {
		foreach (var task in possibleTasks) {
			if (task.task == inTask) {
				return task;
			}
		}
		Debug.LogError("Something went wrong should never reach here, the specified task does not exist in the possible tasks for this unit");
		return null;
	}

	public void ClearTasks() {
		possibleTasks.Clear();
		taskToDo = null;
	}

	public void SetTaskToDo(AITask task) {
		taskToDo = task;
	}

	public void SetTaskToDo(AICalculateScore.PossibleTasks task) {
		taskToDo = FindTaskByName(task);
	}

}
