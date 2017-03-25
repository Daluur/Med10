using System.Collections;
using CombatWorld.Map;
using CombatWorld.Units;
using UnityEngine;

public class AITask {

	public AITask(float score, AICalculateScore.PossibleTasks task, Node endNode) {
		this.score = score;
		this.task = task;
		this.endNode = endNode;
	}

	public AITask(float score, AICalculateScore.PossibleTasks task, Node endNode, int turnsToReachTower, Node tower) {
		this.score = score;
		this.task = task;
		this.endNode = endNode;
		this.turnsToTower = turnsToReachTower;
		this.towerToMoveTo = tower;
	}

	public AITask(float score, AICalculateScore.PossibleTasks task, int blockedTowerNodes, Node endNode, Node tower) {
		this.score = score;
		this.task = task;
		this.endNode = endNode;
		this.blockedTowerNodes = blockedTowerNodes;
		this.towerToMoveTo = tower;
	}

	public AITask(float score, AICalculateScore.PossibleTasks task, Node endNode, Node toAttack) {
		this.score = score;
		this.task = task;
		this.endNode = endNode;
		this.toAttack = toAttack;
	}

	public AITask(float score, AICalculateScore.PossibleTasks task, Node endNode, Node toAttack, Node towerToMoveTo, int turnsToTower) {
		this.score = score;
		this.task = task;
		this.endNode = endNode;
		this.toAttack = toAttack;
		this.towerToMoveTo = towerToMoveTo;
		this.turnsToTower = turnsToTower;
	}



	public float score;
	public AICalculateScore.PossibleTasks task;
	public Node endNode;
	public Node toAttack;
	public int blockedTowerNodes = 0;
	public Node towerToMoveTo;
	public int turnsToTower = 10;
}
