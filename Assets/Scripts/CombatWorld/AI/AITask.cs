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
	public AITask(float score, AICalculateScore.PossibleTasks task, Node endNode, int blockedTowerNodes) {
		this.score = score;
		this.task = task;
		this.endNode = endNode;
		this.blockedTowerNodes = blockedTowerNodes;
	}

	public AITask(float score, AICalculateScore.PossibleTasks task, Node endNode, Node toAttack) {
		this.score = score;
		this.task = task;
		this.endNode = endNode;
		this.toAttack = toAttack;
	}



	public float score;
	public AICalculateScore.PossibleTasks task;
	public Node endNode;
	public Node toAttack;
	public int blockedTowerNodes = 0;
}
