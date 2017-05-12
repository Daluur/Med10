using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld.AI {
	public class AIDefensiveTask  {
		private static Pathfinding pathfinding = new Pathfinding();

		public static List<AITask> MoveBlock(Unit unit) {
			var allUnits = GameController.instance.GettAllUnitsOfTeam(Team.Player);
			List<AITask> tasks = new List<AITask>();
			foreach (var units in allUnits) {
				if (!units.gameObject.GetComponent<Unit>() || units.GetComponent<IEntity>().GetTeam()==Team.AI) {
					continue;
				}
				var unitComp = units.GetComponent<Unit>();
				Node tower = null;
				var node = EnemyShortestPathToTower(unitComp, out tower);
				var turnsToTower = AIUtilityMethods.TurnsToReachNode(unitComp, node);
				if (turnsToTower <= 3 ) {
					if(pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance()).Contains(node))
						tasks.Add(new AITask(0, PossibleTasks.MoveBlock, node, turnsToTower, tower));
				}
			}
			return tasks;
		}
		private static Node EnemyShortestPathToTower(Unit unit, out Node towerToReturn) {
			var towers = GameController.instance.GetTowersForTeam(Team.AI);
			List<Node> towerNeighbors = new List<Node>();
			towerToReturn = null;
			foreach (var tower in towers) {
				towerNeighbors.AddRange(tower.neighbours);
			}
			var toReturn = AIUtilityMethods.ShortestPath(unit, towerNeighbors);
			foreach (var tower in towers) {
				if (tower.neighbours.Contains(toReturn)) {
					towerToReturn = tower;
				}
			}
			return toReturn;
		}

		public static List<AITask> CanBlockPathToOwnTower(Unit unit) {
			List<AITask> tasks = new List<AITask>();
			List<Node> enemyReachableNodes = new List<Node>();
			var towers = GameController.instance.GetTowersForTeam(Team.AI);
			foreach (var tower in towers) {
				foreach (var neighbor in tower.neighbours) {
					foreach (var neighborsNeighbor in neighbor.neighbours) {
						if (neighborsNeighbor.HasOccupant() && neighborsNeighbor.GetOccupant().GetTeam() != Team.AI) {
							var enemyUnit = neighborsNeighbor.GetUnit();
							enemyReachableNodes = pathfinding.GetAllNodesWithinDistanceWithhoutOccupants(enemyUnit.GetNode(), enemyUnit.GetMoveDistance());
							var task = PathBlocking(unit, enemyUnit, enemyReachableNodes, tower);
							if (task != null) {
								tasks.Add(task);
								AIUtilityMethods.FindSubscribedTower(tower).AddDefensiveFocus();
							}
						}
					}
				}
			}
			return tasks;
		}
		private static AITask PathBlocking(Unit unit, Unit enemyUnit, List<Node> reachableNodes, Node tower) {
			if (enemyUnit.GetShadow())
				return null;
			List<Node> unitReachableNodes = new List<Node>();
			unitReachableNodes = pathfinding.GetAllNodesWithinDistanceWithhoutOccupants(unit.GetNode(), unit.GetMoveDistance());
			var amountTowerNeighborsEnemyCanReach = tower.GetNeighbours().Count;
			var prevAmountTowerNeighborsEnemyCanReach = amountTowerNeighborsEnemyCanReach;
			Node bestNode = null;
			foreach (var node in unitReachableNodes) {
				if (reachableNodes.Contains(node)) {
					amountTowerNeighborsEnemyCanReach = tower.GetNeighbours().Count;
					var enemyUnitPath = pathfinding.GetReachableNodesForUnitAfterAMove(enemyUnit, node, unit.GetNode());
					foreach (var enemyNode in enemyUnitPath) {
						foreach (var enemyNeighbor in enemyNode.neighbours) {
							if (enemyNeighbor.HasTower()) {
								amountTowerNeighborsEnemyCanReach--;
							}
						}
					}
					if (amountTowerNeighborsEnemyCanReach < prevAmountTowerNeighborsEnemyCanReach) {
						prevAmountTowerNeighborsEnemyCanReach = amountTowerNeighborsEnemyCanReach;
						bestNode = node;
					}
				}
			}
			AITask task = null;
			if (amountTowerNeighborsEnemyCanReach < tower.GetNeighbours().Count && bestNode != null) {
				task = new AITask(0, PossibleTasks.MoveBlock, tower.GetNeighbours().Count - amountTowerNeighborsEnemyCanReach, bestNode, tower);
			}
			return task;
		}

		public static List<AITask> DefendOwnTower(Unit unit) {
			List<AITask> tasks = new List<AITask>();
			var towers = GameController.instance.GetTowersForTeam(Team.AI);
			foreach (var tower in towers) {
				if (AIUtilityMethods.FindSubscribedTower(tower).amountFocusingThisTower > 0) {
					foreach (var neighbor in tower.neighbours) {
						if (neighbor.HasOccupant() && neighbor.GetOccupant().GetTeam() != Team.AI &&
						    neighbor.GetOccupant().GetHealth() > 0) {
							foreach (var neighborsNeighbor in neighbor.neighbours) {
								var path = unit.GetShadow() ? pathfinding.GetPathFromToWithoutOccupants(unit.GetNode(), neighborsNeighbor) : pathfinding.GetPathFromTo(unit.GetNode(), neighborsNeighbor);
								if (path != null && path.Count <= unit.GetMoveDistance()) {
									tasks.Add(new AITask(0, PossibleTasks.MoveAttackDefensive, neighborsNeighbor, neighbor, tower, 1));
								}
							}
						}
						foreach (var neighborNeighbor in neighbor.neighbours) {
							if (neighbor.HasOccupant() && neighbor.GetOccupant().GetTeam() != Team.AI &&
							    neighbor.GetOccupant().GetHealth() > 0) {
								foreach (var nnn in neighborNeighbor.neighbours) {
									var path = unit.GetShadow() ? pathfinding.GetPathFromToWithoutOccupants(unit.GetNode(), nnn) : pathfinding.GetPathFromTo(unit.GetNode(), nnn);
									if (path != null && path.Count <= unit.GetMoveDistance()) {
										tasks.Add(new AITask(0, PossibleTasks.MoveAttackDefensive, neighborNeighbor, nnn, tower, 1));
									}
								}
							}
						}
					}
				}
			}
			return tasks;
		}

		public static AITask Stay(IEntity unit) {
			return new AITask(0, PossibleTasks.Stay, unit.GetNode());
		}

		public static List<AITask> MoveFromSpawn(Unit unit) {
			List<AITask> tasks = new List<AITask>();
			var moveTo = pathfinding.GetAllNodesWithinDistanceWithhoutOccupants(unit.GetNode(), unit.GetMoveDistance());
			foreach (var node in moveTo) {
				if(node.GetType() != typeof(SummonNode) && unit.GetNode().GetType() == typeof(SummonNode))
					tasks.Add(new AITask(0, PossibleTasks.MoveFromSpawn, node));
			}
			return tasks;
		}

	}
}