using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;
//ddfgh
namespace CombatWorld.AI {
	public class AIScoringMethods  {
		private static Pathfinding pathfinding = new Pathfinding();

		public static bool AmIBlockingTowerHits(Unit unit) {
			var allUnits = GameController.instance.GettAllUnitsOfTeam(Team.Player);
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

		public static int IsAttackeeCloseToTower(Unit unit, Node enemyUnit) {
			var towers = GameController.instance.GetTowersForTeam(Team.Player);
			foreach (var tower in towers) {
				if (tower.neighbours.Contains(enemyUnit)) {
					return 4;
				}
				foreach (var neighbor in tower.neighbours) {
					if (neighbor.neighbours.Contains(enemyUnit)) {
						return 4;
					}
				}
			}
			return 1;
		}

		public static float AttackCalculation(Unit unit, Node enemy) {
			var enemyUnit = enemy.GetUnit();
			var killingFactor = 20;
			var dyingFactor = 2;
			var unitATKEnemyTypeModifier = AIUtilityMethods.TypeModifier(unit.GetElementalType(), enemyUnit.GetElementalType());
			var enemyUnitATKunitTypeModifier = AIUtilityMethods.TypeModifier(enemyUnit.GetElementalType(), unit.GetElementalType());
			Debug.Log("Unit: " + unitATKEnemyTypeModifier + "Enemy: " + enemyUnitATKunitTypeModifier);
			return ( ( ( unit.GetAttackValue() * unitATKEnemyTypeModifier ) / enemyUnit.GetHealth() ) * killingFactor -
			         ( (enemyUnit.GetAttackValue() * enemyUnitATKunitTypeModifier ) / unit.GetHealth() ) * dyingFactor ) *
			       IsAttackeeCloseToTower(unit, enemy);

		}
	}
}