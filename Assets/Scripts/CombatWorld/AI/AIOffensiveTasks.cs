using System.Collections.Generic;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;

namespace CombatWorld.AI {
	public class AIOffensiveTasks : MonoBehaviour {
		public static List<AITask> MoveToEnemyTower(Unit unit) {
			List<AITask> tasks = new List<AITask>();
			var nodesToMoveTo = AIUtilityMethods.MoveTowardTower(unit, Team.Player);
			foreach (var tower in nodesToMoveTo) {
				foreach (var node in tower.toMoveTo) {
					if (unit.GetNode() == node) {
					}
					else {
						tasks.Add(new AITask(0, PossibleTasks.MoveOffensive, node, null, tower.towerNode, tower.turnsToTower));
					}
				}
			}
			return tasks;
		}

		public static float StandingOnSpawn(Unit unit) {
			var enemySpawn = GameController.instance.GetPlayerSummonNodes();
			var factor = 1f;
			var canStay = false;
			foreach (var node in enemySpawn) {
				if (node.HasUnit()) {
					factor += 3f;
				}
				if (node.HasUnit() && node.GetUnit() == unit) {
					canStay = true;
				}
			}
			return canStay ? factor : 1f;
		}
	}
}