using System.Collections;
using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using Overworld;
using UnityEngine;

namespace CombatWorld.AI {
	public class TowerTask {
		public TowerTask(Node tower, List<Node> nodes) {
			towerNode = tower;
			toMoveTo = nodes;
		}

		public Node towerNode;
		public List<Node> toMoveTo;
		public int turnsToTower = 10;
	}

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

	public class AICalculateScore : Singleton<AICalculateScore> {

		private Dictionary<Unit, AIUnit> aiUnits = new Dictionary<Unit, AIUnit>();

		private List<Unit> toRemoveAfterTurn = new List<Unit>();

		private Pathfinding pathfinding = new Pathfinding();

		public int[] unitsToSummon = new int[4];

		private List<Unit> playerUnits = new List<Unit>();

		public int summonPoints { get; protected set; }

		public int offensiveFactor = 0, defensiveFactor = 0;

		public int triggerForDefensiveSpawns = 3, triggerForOffensiveSpawns = 3;

		private void Start() {
			summonPoints = DamageConstants.STARTSUMMONPOINTS - DamageConstants.SUMMONPOINTSPERTURN;
			if (SceneHandler.instance != null) {
				unitsToSummon = SceneHandler.instance.GetDeck().unitIDs;
				AITaskImplementations.IsStoneEncounter(SceneHandler.instance.GetDeck());
				DataGathering.Instance.AILastDeck(new List<int>(unitsToSummon));
			}
			AIUtilityMethods.FillSubscriptionTowers();
		}

		public void DoAITurn() {
			AIUtilityMethods.FillSubscriptionTowers();
			StartCoroutine(WaitForActions());
		}

		private IEnumerator WaitForActions() {
			offensiveFactor = OffensiveFactorCalculation();
			defensiveFactor = DefensiveFactorCalculation();
//			Debug.Log("Turn started with offensive: " + offensiveFactor + " and defensive: " + defensiveFactor + " and: " + summonPoints);
			foreach (var unit in aiUnits) {
//				Debug.Log("we are now at offensive: " + offensiveFactor + " and defensive: " + defensiveFactor);
				while (GameController.instance.WaitingForAction()) {
					yield return new WaitForSeconds(0.1f);
				}
				if(GameController.instance.gameFinished)
					yield break;
				if (unit.Key.GetHealth() < 1)
					continue;
				if(!unit.Key.CanMove())
					continue;
				unit.Value.possibleTasks.AddRange(AIAttackTasks.InRangeOfAttack(unit.Key));
				unit.Value.possibleTasks.AddRange(AIOffensiveTasks.MoveToEnemyTower(unit.Key));
				unit.Value.possibleTasks.AddRange(AIAttackTasks.MoveAndAttack(unit.Key));
				unit.Value.possibleTasks.AddRange(AIDefensiveTask.DefendOwnTower(unit.Key));
				unit.Value.possibleTasks.AddRange(AIDefensiveTask.CanBlockPathToOwnTower(unit.Key));
				unit.Value.possibleTasks.Add(AIDefensiveTask.Stay(unit.Key));
				unit.Value.possibleTasks.AddRange(AIDefensiveTask.MoveFromSpawn(unit.Key));
				unit.Value.possibleTasks.AddRange(AIDefensiveTask.MoveBlock(unit.Key));
				CalculateScore(unit.Key, unit.Value.possibleTasks);
				unit.Value.PerformTask(PickHighestScore(unit.Value.possibleTasks));

				offensiveFactor = OffensiveFactorCalculation();
				defensiveFactor = DefensiveFactorCalculation();

			}
			while (GameController.instance.WaitingForAction()) {
				yield return new WaitForSeconds(0.2f);
			}
			StartCoroutine(AISummon.SpawnUnits(unitsToSummon));
			while (AISummon.summoning) {
				yield return new WaitForSeconds(0.2f);
			}
			//Debug.Log("AI turn ended, summon points left: " + summonPoints);
			AIUtilityMethods.ResetTowerFocus();
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

			if (toChoose.towerToMoveTo != null) {
				var subscribedTower = AIUtilityMethods.FindSubscribedTower(toChoose.towerToMoveTo);
				subscribedTower.AddFocus();
			}

			if(toChoose != null)
				Debug.Log("Chose: " + toChoose.task + " with end: "+toChoose.endNode+" with score: "+toChoose.score);
				return toChoose;
		}

		private void CalculateScore(Unit unit, List<AITask> tasks) {
			var distance = pathfinding.GetDistanceToNode(unit.GetNode(), unit.GetNode());
			foreach (var task in tasks) {
				switch (task.task) {
					case PossibleTasks.MoveOffensive:
						distance = pathfinding.GetDistanceToNode(unit.GetNode(), task.endNode);
						if (distance > unit.GetMoveDistance() || task.endNode == unit.GetNode()) {
						continue;
						}
						var subscribedTower = AIUtilityMethods.FindSubscribedTower(task.towerToMoveTo);
						task.score = ( distance + ( offensiveFactor * 2 - defensiveFactor ) -
									   ( ( subscribedTower != null ) ? subscribedTower.amountFocusingThisTower : 0 ) * 3 ) *
									 ( 4 / task.turnsToTower );
						break;
					case PossibleTasks.Attack:
						if (task.toAttack.HasTower())
							task.score = 10 * 100;
						else if (task.toAttack.HasUnit()) {

							task.score = AIScoringMethods.AttackCalculation(unit, task.toAttack);
							if (AIUtilityMethods.IsUnitNextToAITower(task.toAttack.GetUnit()))
								task.score *= 1000;
						}
						break;
					case PossibleTasks.MoveAttack:
						distance = pathfinding.GetDistanceToNode(unit.GetNode(), task.endNode);
						if(distance > unit.GetMoveDistance())
							continue;
						if (task.toAttack.HasTower())
							task.score = 11 + ( offensiveFactor * 2 - defensiveFactor ) * 1000000;
						else if (task.toAttack.HasUnit()) {
							task.score = AIScoringMethods.AttackCalculation(unit, task.toAttack) + distance;
						}
						break;
					case PossibleTasks.MoveAttackDefensive:
						task.score = 25 + ( defensiveFactor * 2 - offensiveFactor ) *
									 AIUtilityMethods.FindSubscribedTower(task.towerToMoveTo).amountFocusingThisTower * 4;
						break;
					case PossibleTasks.MoveDefensive:
						distance = pathfinding.GetDistanceToNode(unit.GetNode(), task.endNode);
						if(distance > unit.GetMoveDistance())
							continue;
						var myTowers = GameController.instance.GetTowersForTeam(Team.AI);
						task.score = 2;
						foreach (var tower in myTowers) {
							if (tower.GetOccupant().GetHealth() < 10) {
								foreach (var node in tower.neighbours) {
									if (node.HasOccupant() && node.GetOccupant().GetTeam() != Team.AI) {
										task.score = 20 + ( defensiveFactor * 2 - offensiveFactor );
									}
								}
							}
						}
						break;
					case PossibleTasks.MoveBlock:
						distance = pathfinding.GetDistanceToNode(unit.GetNode(), task.endNode);
						if(distance > unit.GetMoveDistance())
							continue;
						task.score = 5 + distance +
									 ( defensiveFactor * 2 - offensiveFactor ) *
									 ( 4 / ( task.turnsToTower > 0 ? task.turnsToTower : 4 ) ) * AIUtilityMethods.FindSubscribedTower(task.towerToMoveTo)
										 .amountFocusingThisTower;
						break;
					case PossibleTasks.Stay:
						float onOwnSpawnPoint = 1;
						foreach (var node in GameController.instance.GetAISummonNodes()) {
							if ((Node) node == unit.GetNode()) {
								onOwnSpawnPoint = -10;
							}
						}
						var blockingTowerHitsFactor = AIScoringMethods.AmIBlockingTowerHits(unit) ? 4 : 1;
						if (AIOffensiveTasks.StandingOnSpawn(unit) > 1) {
							task.score = ( offensiveFactor ) * AIOffensiveTasks.StandingOnSpawn(unit);
						}
						else {
							task.score = ( ( 7 + blockingTowerHitsFactor ) - ( offensiveFactor * 2 ) + onOwnSpawnPoint );
						}
						break;
					case PossibleTasks.MoveFromSpawn:
						task.score = 2 + task.endNode.gameObject.transform.position.z;
						break;
					default:
						//Debug.Log("This task has not been implemented yet");
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
					turnsToReach = AIUtilityMethods.TurnsToReachNode(unit, AIUtilityMethods.ShortestPath(unit, tower.neighbours));
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
			var focusingTower = 0;
			foreach (var tower in towers) {
				focusingTower += AIUtilityMethods.FindSubscribedTower(tower).amountFocusingThisTower;
				hpOfTowers += tower.GetOccupant().GetHealth();
				foreach (var neighbor in tower.neighbours) {
					if (neighbor.HasUnit() && neighbor.GetUnit().GetTeam() != Team.AI) {
						enemiesNextToTowers++;
						atkNextToTowers += neighbor.GetUnit().GetAttackValue();
					}
				}
			}
			if (atkNextToTowers != 0) {
				//Debug.Log(atkNextToTowers + " attackvalue next to towers");
				turnsToKillTowers = hpOfTowers / atkNextToTowers;
			}
			//Debug.Log("turns to kill towers" + turnsToKillTowers);
			toReturn = turnsToKillTowers;
			toReturn += focusingTower*5;
			return toReturn;
		}

		public void AddAIUnit(Unit unit) {
			aiUnits.Add(unit, new AIUnit(unit, pathfinding));
		}

		public void WithdrawSummonPoints(int amount) {
			summonPoints -= amount;
		}

		public void SetSummonPoints(int value) {
			summonPoints = value;
		}

		public void RemoveAIUnit(Unit unit) {
			toRemoveAfterTurn.Add(unit);
		}

		private void RemoveDeadUnits() {
			foreach (var deadUnit in toRemoveAfterTurn) {
				aiUnits.Remove(deadUnit);
			}
		}

		public void AddPlayerUnit(Unit unit) {
			playerUnits.Add(unit);
		}

		public void GiveSummonPoints(int points) {
			summonPoints += points;
		}
	}
}