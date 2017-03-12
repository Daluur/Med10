using System.Collections;
using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;

public class AIUnit : AITask {

	//private List<AICalculateScore.PossibleTasks> possibleTasks = new List<AICalculateScore.PossibleTasks>();
	private Dictionary<AICalculateScore.PossibleTasks, Node> possibleTasks = new Dictionary<AICalculateScore.PossibleTasks, Node>();
	private Pathfinding pathFinding = new Pathfinding();
	private List<Node> possibleMoveTo = new List<Node>();
	private Node moveTo;
	private List<AICalculateScore.PossibleTasks> waitingTasks = new List<AICalculateScore.PossibleTasks>();
	public bool taskCompleted = false;

	public AICalculateScore AICalcScore;

	private void Start() {
		if (AICalcScore == null) {
			AICalcScore = GameObject.Find("EventSystem").GetComponent<AICalculateScore>();
			AICalcScore.AddAIUnit(this);
			//Debug.LogError("You need to add AICalcScore on AI units");
		}
	}

	public void MyTasks() {
		possibleMoveTo = pathFinding.GetAllNodesWithinDistance(currentNode, this.GetMoveDistance());
		OffensiveMove();
		DefensiveMove();

	}

	private void OffensiveMove() {
		//foreach (var node in possibleMoveTo) {
			if(!possibleTasks.ContainsKey(AICalculateScore.PossibleTasks.MoveOffensive)){
				possibleTasks.Add(AICalculateScore.PossibleTasks.MoveOffensive, possibleMoveTo[1]);
			}
		//}
		if(CanMove()&&CanAttack()){
			foreach (var node in possibleMoveTo) {
				if (node.HasOccupant()) {
					if (node.GetOccupant().GetTeam() != Team.AI) {
						possibleTasks.Add(AICalculateScore.PossibleTasks.Attack, node);
						continue;
					}
				}
				else if (!node.HasOccupant()) {
					foreach (var neighborNode in node.neighbours) {
						if (neighborNode.HasOccupant()) {
							if (neighborNode.GetOccupant().GetTeam() != Team.AI) {
								moveTo = node;
								possibleTasks.Add(AICalculateScore.PossibleTasks.MoveAttack, neighborNode);
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
		if (CanMove()) {
			foreach (var node in possibleMoveTo) {
				//TODO: Get any enemies that can attack now, also see if what I move to is in range of any of the other teams units on the next turn to see if they can attack me?
				//TODO: Where on the map is a node, I want to move closer to tower to defend, but no way of finding it ATM
			}
		}
	}

	public void PerformTask(AICalculateScore.PossibleTasks task) {
		DoTask(task);
	}

	//TODO: Implement task completion.
	private void DoTask(AICalculateScore.PossibleTasks task) {
		Node node;
		Debug.Log(task.ToString());
		switch (task) {
			case AICalculateScore.PossibleTasks.MoveAttack:
				possibleTasks.TryGetValue(task, out node);
				AITaskImplementations.MoveToAndAttack(moveTo, node, node.GetUnit());
				break;
			case AICalculateScore.PossibleTasks.MoveOffensive:
				Debug.Log("Got it");
				possibleTasks.TryGetValue(task, out node);
				AITaskImplementations.MoveTo(node, this);
				break;
		}
		Debug.Log(this.gameObject.name + " wants to do: " + task.ToString());
		taskCompleted = true;
	}

	public Dictionary<AICalculateScore.PossibleTasks, Node> GetTasks() {
		return possibleTasks;
	}

}
