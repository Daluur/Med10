using System.Collections;
using System.Collections.Generic;
using CombatWorld;
using CombatWorld.AI;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;

public class AICalculateScore : MonoBehaviour {

	private Dictionary<Unit, AIUnit> aiUnits = new Dictionary<Unit, AIUnit>();

	private Pathfinding pathfinding = new Pathfinding();

	public int unitToSummon;

	//private List<AIUnit> aiUnits = new List<AIUnit>();
	private List<Unit> playerUnits = new List<Unit>();
	private Team aiTeam = Team.AI;
	private List<Node> aiTowers = new List<Node>();
	private List<Node> playerTowers = new List<Node>();

	private void Start() {
		aiTowers = GameController.instance.GetTowersForTeam(aiTeam);
		playerTowers = GameController.instance.GetTowersForTeam(Team.Player);
	}


	private void Update() {
		if (Input.GetKeyDown(KeyCode.U)) {
			SpawnUnits();
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			EvaluateTasksAllUnits();
			foreach (var unit in aiUnits) {
				unit.Value.ClearTasks();
			}
		}
	}

	private void EvaluateTasksAllUnits() {
		foreach (var unit in aiUnits) {
			unit.Value.MyTasks();
			var task = EvaluateTasks(unit.Value);
			//unit.Value.SetTaskToDo(task);
			PerformTask(unit.Value, unit.Value.FindTaskByName(task));
		}
	}

	public void DoCalculations() {


	}

	private void EvaluateTasksForUnit(AIUnit unit) {
		unit.MyTasks();
		var task = EvaluateTasks(unit);
		PerformTask(unit, unit.FindTaskByName(task));
	}

	private void SpawnUnits() {
		var summonNodes = GameController.instance.GetAISummonNodes();
		var type = EvaluateUnitToSpawn();
		foreach (var node in summonNodes) {
			if (node.HasOccupant() && node.GetOccupant().GetTeam() == aiTeam) {
				var unit = node.GetUnit().GetComponent<Unit>();
				AIUnit aiUnit;
				aiUnits.TryGetValue(unit, out aiUnit);
				EvaluateTasksForUnit(aiUnit);
				unit.StartCoroutine(WaitForCompletedTask(aiUnit));
				SpawnUnit(node, type);
			}
			SpawnUnit(node, type);
			return;
		}
	}

	private void SpawnUnit(SummonNode node, int type) {
		AddAIUnit(SummonHandler.instance.SummonAIUnitByID(node, type));
	}

	private int EvaluateUnitToSpawn() {
		return BestToSummon(GameController.instance.GetAllUnits());
	}

	private int BestToSummon(GameObject[] gos) {
		int unitToSummon = EnemyUnitAnalysis(gos);
		return unitToSummon;
	}

	private int EnemyUnitAnalysis(GameObject[] gos) {
		var totalHealth = 0;
		int[] amountOfTypes = new int[5];
		var mostOfType = ElementalTypes.NONE;
		var versatile = false;

		foreach (var go in gos) {
			if (go.GetComponent<Unit>().GetTeam() != aiTeam) {
				var unit = go.GetComponent<Unit>();
				totalHealth += unit.GetHealth();
				switch (unit.GetType()) {
					case ElementalTypes.NONE:
						amountOfTypes[0]++;
						break;
					case ElementalTypes.Nature:
						amountOfTypes[1]++;
						break;
					case ElementalTypes.Fire:
						amountOfTypes[2]++;
						break;
					case ElementalTypes.Lightning:
						amountOfTypes[3]++;
						break;
					case ElementalTypes.Water:
						amountOfTypes[4]++;
						break;
					default:
						break;

				}
				if (unit.GetMoveDistance() > 1) {
					versatile = true;
				}
			}
		}

		if (totalHealth > 15) {
			return 2;
		}
		if (versatile) {
			return 1;
		}
		if (mostOfType != ElementalTypes.NONE) {
			return 0; //Implement proper summon types and spawning and such
		}

		return 0;
	}


	private void SummonUnit() {

	}

	private IEnumerator WaitForCompletedTask(AIUnit unit) {
		while (!unit.taskCompleted) {
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
		MoveAttack,
		Attack,
		MoveFromSpawn,
		TurnStone,
		TurnShadow
	}

}
