using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld.AI {
	public class AIAttackTasks {

		private static Pathfinding pathfinding = new Pathfinding();

		public static List<AITask> InRangeOfAttack(Unit unit) {
			List<AITask> attacksFromCurrPos = new List<AITask>();
			foreach (var neighbour in unit.GetNode().neighbours) {
				if (neighbour.HasOccupant() && neighbour.GetOccupant().GetTeam() != Team.AI) {
					attacksFromCurrPos.Add(new AITask(0, PossibleTasks.Attack, unit.GetNode(), neighbour));
				}
			}
			return attacksFromCurrPos;
		}

		public static List<AITask> MoveAndAttack(Unit unit) {
			List<AITask> moveAndAttack = new List<AITask>();
			var nodesToAttackWithinReach = unit.GetShadow() ? pathfinding.GetAllReachableNodes(unit.GetNode(), unit.GetMoveDistance()+1) : pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance()+1);
			var nodesWithinReach = unit.GetShadow() ? pathfinding.GetAllReachableNodes(unit.GetNode(), unit.GetMoveDistance()) : pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance());
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
	}
}