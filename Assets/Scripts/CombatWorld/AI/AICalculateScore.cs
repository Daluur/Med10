using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombatWorld;
using CombatWorld.AI;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AICalculateScore : Singleton<AICalculateScore> {

	struct AIUnitTasks {

		private Unit unit;
		private AIUnit aiUnit;
	}

	private Dictionary<Unit, AIUnit> aiUnits = new Dictionary<Unit, AIUnit>();

	private List<Unit> toRemoveAfterTurn = new List<Unit>();

	private ItemDatabase dataBase;

	private Pathfinding pathfinding = new Pathfinding();

	private Stack<Unit> dependentTasks = new Stack<Unit>();

	public int[] unitsToSummon = new int[4];

	private List<Unit> playerUnits = new List<Unit>();
	private Team aiTeam = Team.AI;
	private List<Node> aiTowers = new List<Node>();
	private List<Node> playerTowers = new List<Node>();

	public int summonPoints = 3;

	private int offensiveFactor = 0, defensiveFactor = 0;

	public int triggerForDefensiveSpawns = 3, triggerForOffensiveSpawns = 3;

	private void Start() {
		aiTowers = GameController.instance.GetTowersForTeam(aiTeam);
		playerTowers = GameController.instance.GetTowersForTeam(Team.Player);
		dataBase = new ItemDatabase();
	}


	private void Update() {
		if (Input.GetKeyDown(KeyCode.U)) {
			SpawnUnits();
		}

		//if (Input.GetKeyDown(KeyCode.W)) {
		//	StartCoroutine(WaitForActions());
/*foreach (var unit in aiUnits) {
				unit.Value.possibleTasks.AddRange(MoveToEnemyTower(unit.Key));
				CalculateScore(unit.Key, unit.Value.possibleTasks);
				unit.Value.PerformTask(PickHighestScore(unit.Value.possibleTasks));

//foreach (var task in unit.Value.possibleTasks) {

					//if(task!=null)
						//Debug.Log(task.endNode);
				//}

			}*/
	//	}
	}

	public void DoAITurn() {
		StartCoroutine(WaitForActions());
	}

	private IEnumerator WaitForActions() {
		offensiveFactor = OffensiveFactorCalculation();
		defensiveFactor = DefensiveFactorCalculation();
		Debug.Log("Turn started with offensive: " + offensiveFactor + " and defensive: " + defensiveFactor + " and: " + summonPoints);
		foreach (var unit in aiUnits) {
			Debug.Log("we are now at offensive: " + offensiveFactor + " and defensive: " + defensiveFactor);
			while (GameController.instance.WaitingForAction()) {
				yield return new WaitForSeconds(0.1f);
			}
			if (unit.Key.GetHealth() < 1)
				continue;
			if(!unit.Key.CanMove())
				continue;
			unit.Value.possibleTasks.AddRange(InRangeOfAttack(unit.Key));
			unit.Value.possibleTasks.AddRange(MoveToEnemyTower(unit.Key));
			unit.Value.possibleTasks.AddRange(MoveAndAttack(unit.Key));
			unit.Value.possibleTasks.AddRange(DefendOwnTower(unit.Key));
			unit.Value.possibleTasks.AddRange(CanBlockPathToOwnTower(unit.Key));
			unit.Value.possibleTasks.Add(Stay(unit.Key));
			unit.Value.possibleTasks.AddRange(MoveFromSpawn(unit.Key));
			unit.Value.possibleTasks.AddRange(MoveBlock(unit.Key));
			CalculateScore(unit.Key, unit.Value.possibleTasks);
			unit.Value.PerformTask(PickHighestScore(unit.Value.possibleTasks));

		}
		SpawnUnits();
		Debug.Log("AI turn ended, summon points left: " + summonPoints);
		GameController.instance.EndTurn();
		RemoveDeadUnits();
		yield return null;
	}

	private AITask PickHighestScore(List<AITask> tasks) {
		float score = -20f;
		AITask toChoose = null;
		foreach (var task in tasks) {
			if (task.score > score) {
				toChoose = task;
				score = task.score;
			}
		}
		if (toChoose == null) {
			Debug.Log(tasks.Count);
			foreach (var task in tasks) {
				Debug.Log(task.task);
			}
		}
		if(toChoose != null)
			Debug.Log("Chose: " + toChoose.task + " with end: "+toChoose.endNode+" with score: "+toChoose.score);
		return toChoose;
	}

	private void CalculateScore(Unit unit, List<AITask> tasks) {
		var distance = pathfinding.GetDistanceToNode(unit.GetNode(), unit.GetNode());
		var towers = GameController.instance.GetTowersForTeam(Team.AI);
		foreach (var task in tasks) {
			switch (task.task) {
				case PossibleTasks.MoveOffensive:
					if (distance < pathfinding.GetDistanceToNode(unit.GetNode(), task.endNode)) {
						distance = pathfinding.GetDistanceToNode(unit.GetNode(), task.endNode);
						task.score = distance + (offensiveFactor*2 - defensiveFactor);
					}
					break;
				case PossibleTasks.Attack:
					Debug.Log(unit.name +" can attack");
					if (task.toAttack.HasTower())
						task.score = 10;
					else if (task.toAttack.HasUnit()) {
						var enemyUnit = task.toAttack.GetUnit();
						if (unit.GetAttackValue() >= enemyUnit.GetHealth()) {
							task.score = 10;
						}
						else if (enemyUnit.GetAttackValue() >= unit.GetHealth()) {
							task.score = 1;
						}
						if (IsUnitNextToAITower(unit)) {
							task.score += 20;
						}
						if (IsUnitNextToAITower(enemyUnit)) {
							Debug.Log("enemy unit next to tower");
							task.score += 20;
						}
					}
					break;
				case PossibleTasks.MoveAttack:
					if (task.toAttack.HasTower())
						task.score = 11 + (offensiveFactor*2 - defensiveFactor);
					else if (task.toAttack.HasUnit()) {
						var enemyUnit = task.toAttack.GetUnit();
						if (enemyUnit.GetAttackValue() > 3) {
							task.score = 1;
						}
						else if (enemyUnit.GetAttackValue() > unit.GetHealth()) {
							task.score = -2;
						}
						else if (enemyUnit.GetAttackValue() < unit.GetHealth()) {
							task.score = 3;
						}
						else if (enemyUnit.GetHealth() < unit.GetAttackValue()) {
							task.score = 8;
						}
					}
					break;
				case PossibleTasks.MoveAttackDefensive:
					task.score = 25 + (defensiveFactor*2 - offensiveFactor);
					break;
				case PossibleTasks.MoveDefensive:
					var myTowers = GameController.instance.GetTowersForTeam(Team.AI);
					task.score = 2;
					foreach (var tower in myTowers) {
						if (tower.GetOccupant().GetHealth() < 10) {
							foreach (var node in tower.neighbours) {
								if (node.HasOccupant() && node.GetOccupant().GetTeam() != Team.AI) {
									task.score = 20 + (defensiveFactor*2 - offensiveFactor);
								}
							}
						}
					}
					break;
				case PossibleTasks.MoveBlock:
					task.score = 5 + pathfinding.GetDistanceToNode(unit.GetNode(), task.endNode) + (defensiveFactor*2 - offensiveFactor);;
					break;
				case PossibleTasks.Stay:
					if (unit.GetNode())
						if (AmIBlockingTowerHits(unit)) {
							task.score = 7 - offensiveFactor*2;
						}
						else {
							task.score = -3;
						}
					foreach (var node in GameController.instance.GetAISummonNodes()) {
						if ((Node) node == unit.GetNode()) {
							task.score -= 10;
						}
					}
					break;
				case PossibleTasks.MoveFromSpawn:
					task.score = 2;
					break;
				default:
					Debug.Log("This task has not been implemented yet");
					break;
			}
		}
	}


	private int OffensiveFactorCalculation() {
		var enemyTowers = GameController.instance.GetTowersForTeam(Team.Player);
		int hpOfTowers = 0;
		int turnsToReach = 0;
		int toReturn = 0;
		foreach (var tower in enemyTowers) {
			hpOfTowers += tower.GetOccupant().GetHealth();
			foreach (var unit in aiUnits.Keys) {
				turnsToReach = TurnsToReachNode(unit, ShortestPath(unit, tower.neighbours));
			}
		}

		if (aiUnits.Count > 0) {
			var avgTurns = turnsToReach / aiUnits.Count;
			if(avgTurns>0)
				toReturn = hpOfTowers / avgTurns;
		}
		toReturn = 3;
		return toReturn;
	}

	private int DefensiveFactorCalculation() {
		var towers = GameController.instance.GetTowersForTeam(Team.AI);
		var hpOfTowers = 0;
		var toReturn = 0;
		var enemiesNextToTowers = 0;
		var atkNextToTowers = 0;
		var turnsToKillTowers = 0;
		foreach (var tower in towers) {
			hpOfTowers += tower.GetOccupant().GetHealth();
			foreach (var neighbor in tower.neighbours) {
				if (neighbor.HasUnit() && neighbor.GetUnit().GetTeam() != Team.AI) {
					enemiesNextToTowers++;
					atkNextToTowers += neighbor.GetUnit().GetAttackValue();
				}
			}
		}
		if (atkNextToTowers != 0) {
			Debug.Log(atkNextToTowers + " attackvalue next to towers");
			turnsToKillTowers = hpOfTowers / atkNextToTowers;
		}
		Debug.Log("turns to kill towers" + turnsToKillTowers);
		toReturn = turnsToKillTowers;
		return toReturn;
	}


	private bool IsUnitNextToAITower(Unit unit) {
		foreach (var neighbor in unit.GetNode().neighbours) {
			if (neighbor.HasOccupant() && neighbor.HasTower() && neighbor.GetOccupant().GetTeam() == unit.GetTeam()) {
				Debug.Log(unit.name + " is next to ai tower");
				return true;
			}
		}
		return false;
	}

	private List<AITask> InRangeOfAttack(Unit unit) {
		List<AITask> attacksFromCurrPos = new List<AITask>();
		foreach (var neighbour in unit.GetNode().neighbours) {
			if (neighbour.HasOccupant() && neighbour.GetOccupant().GetTeam() != Team.AI) {
				attacksFromCurrPos.Add(new AITask(0,PossibleTasks.Attack, unit.GetNode(), neighbour));
			}
		}
		return attacksFromCurrPos;
	}

	private List<AITask> MoveAndAttack(Unit unit) {
		List<AITask> moveAndAttack = new List<AITask>();
		var nodesToAttackWithinReach = pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance()+1);
		var nodesWithinReach = pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance());
		foreach (var node in nodesToAttackWithinReach) {
			if (node.HasOccupant() && node.GetOccupant().GetTeam() != Team.AI && node.GetOccupant().GetHealth() > 0) {
				foreach (var neighbour in node.neighbours) {
					if (nodesWithinReach.Contains(neighbour)&&!neighbour.Equals(unit.GetNode())&&!neighbour.HasOccupant()) {
						//if((node.HasUnit() && node.GetUnit().GetHealth() > 0) || (node.HasTower() && node.GetOccupant().GetHealth() > 0))
							moveAndAttack.Add(new AITask(0,PossibleTasks.MoveAttack, neighbour, node));
					}
				}
			}
		}
		return moveAndAttack;
	}

	private List<AITask> MoveToFriendlyTower(Unit unit) {
		List<AITask> tasks = new List<AITask>();
		var nodesToMoveTo = MoveTowardTower(unit, Team.AI);
		foreach (var node in nodesToMoveTo) {
			if (unit.GetNode() == node) {
				tasks.Add(new AITask(0, PossibleTasks.Stay, node));
			}
			else {
				tasks.Add(new AITask(0, PossibleTasks.MoveDefensive, node));
			}
		}
		return tasks;
	}

	private List<AITask> CanBlockPathToOwnTower(Unit unit) {
		List<AITask> tasks = new List<AITask>();
		List<Node> enemyReachableNodes = new List<Node>();
		var towers = GameController.instance.GetTowersForTeam(Team.AI);
		foreach (var tower in towers) {
			foreach (var neighbor in tower.neighbours) {
				foreach (var neighborsNeighbor in neighbor.neighbours) {
					if (neighborsNeighbor.HasOccupant() && neighborsNeighbor.GetOccupant().GetTeam() != Team.AI) {
						enemyReachableNodes = pathfinding.GetAllNodesWithinDistanceWithhoutOccupants(neighborsNeighbor.GetUnit().GetNode(), neighborsNeighbor.GetUnit().GetMoveDistance());
					}
				}
			}
		}
		List<Node> unitReachableNodes = new List<Node>();
		unitReachableNodes = pathfinding.GetAllNodesWithinDistanceWithhoutOccupants(unit.GetNode(), unit.GetMoveDistance());
		foreach (var node in unitReachableNodes) {
			if (enemyReachableNodes.Contains(node)) {
				foreach (var neighbor in node.neighbours) {
					if (neighbor.HasTower()) {
						tasks.Add(new AITask(0, PossibleTasks.MoveBlock, node));
					}
				}
			}
		}
		return tasks;
	}

	private List<AITask> MoveFromSpawn(Unit unit) {
		List<AITask> tasks = new List<AITask>();
		var moveTo = pathfinding.GetAllNodesWithinDistanceWithhoutOccupants(unit.GetNode(), unit.GetMoveDistance());
		foreach (var node in moveTo) {
			if(node.GetType() != typeof(SummonNode))
				tasks.Add(new AITask(0, PossibleTasks.MoveFromSpawn, node));
		}
		return tasks;
	}

	private AITask Stay(IEntity unit) {
		return new AITask(0, PossibleTasks.Stay, unit.GetNode());
	}

	private List<AITask> MoveBlock(Unit unit) {
		var allUnits = GameController.instance.GetAllUnits();
		var towers = GameController.instance.GetTowersForTeam(Team.AI);
		List<AITask> tasks = new List<AITask>();
		foreach (var units in allUnits) {
			if (!units.gameObject.GetComponent<Unit>() || units.GetComponent<IEntity>().GetTeam()==Team.AI) {
				continue;
			}
			var unitComp = units.GetComponent<Unit>();
			var node = EnemyShortestPathToTower(unitComp);
			if (TurnsToReachNode(unitComp, node) <= 1 ) {
				if(pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance()).Contains(node))
				tasks.Add(new AITask(0, PossibleTasks.MoveBlock, node));
			}
		}
		return tasks;
	}

	private bool AmIBlockingTowerHits(Unit unit) {
		//if (!IsUnitNextToOwnTower(unit)) {
		//	return false;
		//}

		var allUnits = GameController.instance.GetAllUnits();
		var towers = GameController.instance.GetTowersForTeam(Team.AI);
		foreach (var enemyUnit in allUnits) {
			if (!enemyUnit.GetComponent<Unit>() || enemyUnit.GetComponent<IEntity>().GetTeam() == Team.AI) {
				continue;
			}
			var unitComp = enemyUnit.GetComponent<Unit>();
			var enemyNodesWithinDistance = pathfinding.GetAllReachableNodes(unitComp.GetNode(), unitComp.GetMoveDistance());
			var enemyNodesThatCanBeReached = pathfinding.GetAllNodesWithinDistance(unitComp.GetNode(), unitComp.GetMoveDistance());
			List<Node> towerAttackingNodesWithinDistance = new List<Node>();
			foreach (var tower in towers) {
				foreach (var neighbor in tower.neighbours) {
					if (enemyNodesWithinDistance.Contains(neighbor)) {
						towerAttackingNodesWithinDistance.Add(neighbor);
					}
				}
			}
			foreach (var node in towerAttackingNodesWithinDistance) {
				if (enemyNodesThatCanBeReached.Contains(node) && !towerAttackingNodesWithinDistance.Contains(unit.GetNode())) {
					return false;
				}
			}
		}
		return true;
	}

	private Node EnemyShortestPathToTower(Unit unit) {
		var towers = GameController.instance.GetTowersForTeam(Team.AI);
		Node shortestPath = null;
		List<Node> towerNeighbors = new List<Node>();
 		foreach (var tower in towers) {
			towerNeighbors.AddRange(tower.neighbours);
		}
		return ShortestPath(unit, towerNeighbors);
	}

	private int TurnsToReachNode(Unit unit, Node node) {
		var path = pathfinding.GetPathFromTo(unit.GetNode(), node);
		if(path!=null)
			return Mathf.CeilToInt(path.Count / unit.GetMoveDistance());
		return 20;
	}

	private Node ShortestPath(Unit unit, List<Node> nodes) {
		var distance = 30;
		var prevDistance = 30;
		Node toReturn = null;
		foreach (var node in nodes) {
			distance = pathfinding.GetDistanceToNode(unit.GetNode(), node);
			if (distance > 0 && distance < prevDistance) {
				prevDistance = distance;
				toReturn = node;
			}
		}
		return toReturn;
	}


	private List<AITask> DefendOwnTower(Unit unit) {
		List<AITask> tasks = new List<AITask>();
		var towers = GameController.instance.GetTowersForTeam(Team.AI);
		foreach (var tower in towers) {
			foreach (var neighbor in tower.neighbours) {
				if (neighbor.HasOccupant() && neighbor.GetOccupant().GetTeam() != Team.AI && neighbor.GetOccupant().GetHealth() > 0) {
					foreach (var neighborsNeighbor in neighbor.neighbours) {
						var path = pathfinding.GetPathFromTo(unit.GetNode(), neighborsNeighbor);
						if (path != null && path.Count <= unit.GetMoveDistance()) {
							tasks.Add(new AITask(0, PossibleTasks.MoveAttackDefensive, neighborsNeighbor, neighbor));
						}
					}
				}
			}
		}
		return tasks;
	}

	private Node MoveAndAttackNode(Unit unit) {


	List<AITask> moveAndAttack = new List<AITask>();
	var nodesToAttackWithinReach = pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance()+1);
	var nodesWithinReach = pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance());
	foreach (var node in nodesToAttackWithinReach) {
		if (node.HasOccupant() && node.GetOccupant().GetTeam() != Team.AI) {
			foreach (var neighbour in node.neighbours) {
				if (nodesWithinReach.Contains(neighbour)) {
					moveAndAttack.Add(new AITask(0,PossibleTasks.MoveAttack, neighbour, node));
				}
			}
		}
	}
		return null;
	}

	private List<AITask> MoveToEnemyTower(Unit unit) {
		List<AITask> tasks = new List<AITask>();
		var nodesToMoveTo = MoveTowardTower(unit, Team.Player);
		foreach (var node in nodesToMoveTo) {
			if (unit.GetNode() == node) {
				//tasks.Add(new AITask(0, PossibleTasks.Stay, node));
			}
			else {
				tasks.Add(new AITask(0, PossibleTasks.MoveOffensive, node));
			}
		}
		return tasks;
	}


	private List<Node> MoveTowardTower(Unit unit, Team team) {
		Node towerToMoveTo;
		var towers = GameController.instance.GetTowersForTeam(team);
		List<Node> nodeToBeMovedTo = new List<Node>();
		foreach (var tower in towers) {
			foreach (var neighbour in tower.neighbours) {
				if(!neighbour.HasOccupant()) {
					var node = PathTowardTowerNode(unit, neighbour);
					if(node != null)
						nodeToBeMovedTo.AddRange(node);
				}
			}
		}
		return nodeToBeMovedTo;
	}

	private List<Node> PathTowardTowerNode(Unit unit, Node neighbor) {
		List<Node> endNode = new List<Node>();
		var path = pathfinding.GetPathFromTo(unit.GetNode(), neighbor);
		if (path == null)
			return null;
		if (path.Count > unit.GetMoveDistance()) {
			endNode = GetPathThisTurn(path, unit.GetMoveDistance(), unit);
		}
		else {
			endNode.Add(neighbor);
		}
		return endNode;
	}


	private List<Node> GetPathThisTurn(List<Node> nodes, int moveDistance, Unit unit) {
		List<Node> endNode = new List<Node>();
		endNode.Add(unit.GetNode());
		//Debug.Log(endNode);
		foreach (var node in nodes) {
			var distance = pathfinding.GetDistanceToNode(unit.GetNode(), node);
			if (distance <= moveDistance) {
				endNode.Add(node);
			}
		}
		return endNode;
	}

	private void EvaluateTasksAllUnits() {
		foreach (var unit in aiUnits) {
			unit.Value.MyTasks();
			var task = EvaluateTasks(unit.Value);
			unit.Value.SetTaskToDo(task);
			PerformTask(unit.Value, unit.Value.taskToDo);
		}
	}

	public void DoCalculations() {


	}

	private void EvaluateTasksForUnit(AIUnit unit) {
		unit.MyTasks();
		var task = EvaluateTasks(unit);
		unit.SetTaskToDo(task);
		PerformTask(unit, unit.taskToDo);
	}

	private void SpawnUnits() {
		var summonNodes = GameController.instance.GetAISummonNodes();
		var type = EvaluateUnitToSpawn();
		foreach (var node in summonNodes) {
			if (node.HasOccupant() && node.GetOccupant().GetTeam() == aiTeam) {
				continue;
			}
			if(type.Count>0){
				SpawnUnit(node, type[0]);
				summonPoints -= dataBase.FetchItemByID(type[0]).SummonCost;
				type.RemoveAt(0);
			}
		}
		type.Clear();
	}

	private void SpawnUnit(SummonNode node, int type) {
		AddAIUnit(SummonHandler.instance.SummonAIUnitByID(node, type));
	}

	private List<int> EvaluateUnitToSpawn() {
		return BestToSummon(GameController.instance.GetAllUnits());
	}

	private List<int> BestToSummon(GameObject[] gos) {
		var unitToSummon = EnemyUnitAnalysis(gos);
		return unitToSummon;
	}

	private List<int> EnemyUnitAnalysis(GameObject[] gos) {

		var highHpFactor = 0;
		var highMoveFactor = 0;
		var highDmgFactor = 0;

		if (defensiveFactor > triggerForDefensiveSpawns && offensiveFactor<defensiveFactor) {
			highHpFactor = 3;
			highDmgFactor = 3;
		}
		else if (offensiveFactor > triggerForOffensiveSpawns && defensiveFactor < offensiveFactor) {
			highMoveFactor = 5;
			highDmgFactor = 2;
		}


		List<CombatData> myUnits = new List<CombatData>();
		for(int i=0;i<unitsToSummon.Length;i++){
			myUnits.Add(new CombatData(dataBase.FetchItemByID(unitsToSummon[i])));
		}

		int[] amountOfTypes = new int[5];
		ElementalTypes[] mostOfType = new ElementalTypes[5];

		var typeWithMost = 0;

		foreach (var go in gos) {
			if (go.GetComponent<Unit>().GetTeam() != aiTeam) {
				var unit = go.GetComponent<Unit>();
				switch (unit.GetElementalType()) {
					case ElementalTypes.NONE:
						mostOfType[0] = ElementalTypes.NONE;
						amountOfTypes[0]++;
						break;
					case ElementalTypes.Nature:
						mostOfType[1] = ElementalTypes.Nature;
						amountOfTypes[1]++;
						break;
					case ElementalTypes.Fire:
						mostOfType[2] = ElementalTypes.Fire;
						amountOfTypes[2]++;
						break;
					case ElementalTypes.Lightning:
						mostOfType[3] = ElementalTypes.Lightning;
						amountOfTypes[3]++;
						break;
					case ElementalTypes.Water:
						mostOfType[4] = ElementalTypes.Water;
						amountOfTypes[4]++;
						break;
					default:
						break;

				}

				var most = 0;
				for (int i = 0; i < amountOfTypes.Length; i++) {
					if (amountOfTypes[i] > most) {
						most = amountOfTypes[i];
						typeWithMost = i;
					}
				}
			}
		}
		return BestSummonScore(myUnits, highHpFactor, highMoveFactor, highDmgFactor, mostOfType[typeWithMost]);
	}

	private List<int> BestSummonScore(List<CombatData> units, int highHPFactor, int highMoveFactor, int highDMGFactor, ElementalTypes mostOfType) {
		List<int> index = new List<int>();
		int ind = -1;
		int score = 0;
		int bestScore = -1;
		int calculatedSummonPoints = summonPoints;
		int iterator = 0;
		int maxIterations = 10;
		while (calculatedSummonPoints > 0) {
			if (iterator >= maxIterations) {
				break;
			}
			for(int i=0; i<units.Count;i++) {
				if (units[i].cost <= calculatedSummonPoints && calculatedSummonPoints - units[i].cost >= 0) {
					score = Mathf.RoundToInt((highHPFactor * units[i].healthValue + highMoveFactor * units[i].moveDistance + highDMGFactor * units[i].attackValue)*typeModifier(units[i].type,mostOfType));
					if (score > bestScore) {
						bestScore = score;
						ind = i;
					}
				}
			}
			if(ind!=-1){
				calculatedSummonPoints -= units[ind].cost;
				Debug.Log("Wanting to summon: " + units[ind].model.name + " for: " + units[ind].cost + " " + calculatedSummonPoints);
				index.Add(ind);
			}
			iterator++;
		}
		return index;
	}


	private float typeModifier(ElementalTypes attacker, ElementalTypes defender) {
		if (attacker == ElementalTypes.Fire && defender == ElementalTypes.Water) {
			return 0.5f;
		}
		if (attacker == ElementalTypes.Lightning && defender == ElementalTypes.Water) {
			return 2f;
		}
		if (attacker == ElementalTypes.Nature && defender == ElementalTypes.Fire) {
			return 0.5f;
		}
		if (attacker == ElementalTypes.Water && defender == ElementalTypes.Fire) {
			return 2f;
		}
		if (attacker == ElementalTypes.Fire && defender == ElementalTypes.Nature) {
			return 2f;
		}
		if (attacker == ElementalTypes.Lightning && defender == ElementalTypes.Nature) {
			return 0.5f;
		}
		if (attacker == ElementalTypes.Nature && defender == ElementalTypes.Lightning) {
			return 2f;
		}
		if (attacker == ElementalTypes.Water && defender == ElementalTypes.Fire) {
			return 2f;
		}

		return 1;
	}

	private void SummonUnit() {

	}

	private IEnumerator WaitForCompletedTask(AIUnit unit) {
		while (!GameController.instance.WaitingForAction()) {
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void PerformTask(AIUnit unit, AITask task) {
		Debug.Log("Here");
		unit.PerformTask(task);
	}

	public void AddAIUnit(Unit unit) {
		aiUnits.Add(unit, new AIUnit(unit, pathfinding));
	}

	public void RemoveAIUnit(Unit unit) {
		toRemoveAfterTurn.Add(unit);
	}

	private void RemoveDeadUnits() {
		foreach (var deadUnit in toRemoveAfterTurn) {
			aiUnits.Remove(deadUnit);
		}
	}

	public void SetTower(Tower tower) {
		//aiTower = tower;
	}

	public void AddPlayerUnit(Unit unit) {
		playerUnits.Add(unit);
	}

	private PossibleTasks EvaluateTasks(AIUnit unit) {
		if (unit.FindTaskByName(PossibleTasks.MoveOffensive).task == PossibleTasks.MoveOffensive) {
			return PossibleTasks.MoveOffensive;
		}
		return PossibleTasks.MoveOffensive;
	}

	public void GiveSummonPoints(int points) {
		summonPoints += points;
	}

	enum OverAllTasks {
		SpawnUnits = 1,
		Defend = 2,
		Attack = 3,
	}

	//Possible tasks
	public enum PossibleTasks{
		Stay,
		MoveOffensive,
		MoveDefensive,
		MoveBlock,
		MoveAttack,
		MoveAttackDefensive,
		Attack,
		MoveFromSpawn,
		TurnStone,
		TurnShadow
	}

}
