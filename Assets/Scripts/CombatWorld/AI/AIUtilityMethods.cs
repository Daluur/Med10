using System.Collections.Generic;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;

namespace CombatWorld.AI {
	public class SubscriptionToTower {

		public SubscriptionToTower(Node node) {
			towerNode = node;
			amountFocusingThisTower = 0;
			amountFocusingThisTowerDefensive = 0;
		}

		public Node towerNode;
		public int amountFocusingThisTower;
		public int amountFocusingThisTowerDefensive;

		public void AddFocus() {
			amountFocusingThisTower++;
		}

		public void AddDefensiveFocus() {
			amountFocusingThisTowerDefensive++;
		}

		public void ResetFocus() {
			amountFocusingThisTower = 0;
			amountFocusingThisTowerDefensive = 0;
		}
	}

	public class AIUtilityMethods {

		private static Pathfinding pathfinding = new Pathfinding();
		private static List<SubscriptionToTower> towersToSubscribe = new List<SubscriptionToTower>();

		public static Node ShortestPath(Unit unit, List<Node> nodes) {
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
		public static bool IsUnitNextToAITower(Unit unit) {
			foreach (var neighbor in unit.GetNode().neighbours) {
				if (neighbor.HasOccupant() && neighbor.HasTower() && neighbor.GetOccupant().GetTeam() == unit.GetTeam()) {
					//Debug.Log(unit.name + " is next to ai tower");
					return true;
				}
			}
			return false;
		}
		public static int TurnsToReachNode(Unit unit, Node node) {
			var path = unit.GetShadow() ? pathfinding.GetPathFromToWithoutOccupants(unit.GetNode(), node) : pathfinding.GetPathFromTo(unit.GetNode(), node);
			if(path!=null && unit.GetMoveDistance()!=0)
				return Mathf.CeilToInt(path.Count / unit.GetMoveDistance());
			return 20;
		}

		public static float TypeModifier(ElementalTypes attacker, ElementalTypes defender) {
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
			if (attacker == ElementalTypes.Water && defender == ElementalTypes.Lightning) {
				return 0.5f;
			}

			return 1;
		}
		public static List<Node> GetPathThisTurn(List<Node> nodes, int moveDistance, Unit unit) {
			List<Node> endNode = new List<Node>();
			endNode.Add(unit.GetNode());
			foreach (var node in nodes) {
				//Debug.Log(node);
				var distance = pathfinding.GetDistanceToNode(unit.GetNode(), node);
				if (distance <= moveDistance) {

					endNode.Add(node);
				}
			}

			return endNode;
		}
		public static SubscriptionToTower FindSubscribedTower(Node towerNode) {
			if (towerNode == null) {
				return null;
			}
			SubscriptionToTower toReturn = null;
			foreach (var tower in towersToSubscribe) {
				if (tower.towerNode.Equals(towerNode)) {
					toReturn = tower;
					break;
				}
			}
			return toReturn;
		}

		public static void FillSubscriptionTowers() {
			var aiTowers = GameController.instance.GetTowersForTeam(Team.AI);
			var playerTowers = GameController.instance.GetTowersForTeam(Team.Player);
			foreach (var tower in aiTowers) {
				towersToSubscribe.Add(new SubscriptionToTower(tower));
			}
			foreach (var tower in playerTowers) {
				towersToSubscribe.Add(new SubscriptionToTower(tower));
			}
		}

		public static List<Node> PathTowardTowerNode(Unit unit, Node neighbor, out int turnsToTower) {
			List<Node> endNode = new List<Node>();
			var path = pathfinding.GetPathFromToWithoutOccupants(unit.GetNode(), neighbor);
			turnsToTower = 10;
			if (path == null)
				return null;

			if (path.Count > unit.GetMoveDistance()) {
				endNode = GetPathThisTurn(path, unit.GetMoveDistance(), unit);
				turnsToTower = Mathf.RoundToInt(path.Count / unit.GetMoveDistance());
			}
			else if(pathfinding.GetPathFromTo(unit.GetNode(), neighbor)!=null && pathfinding.GetPathFromTo(unit.GetNode(), neighbor).Count <= unit.GetMoveDistance()) {
				endNode.Add(neighbor);
				turnsToTower = 1;
			}

			return endNode;
		}

		public static List<TowerTask> MoveTowardTower(Unit unit, Team team) {
			var towers = GameController.instance.GetTowersForTeam(team);
			List<TowerTask> nodeToBeMovedTo = new List<TowerTask>();
			TowerTask tmp;
			foreach (var tower in towers) {
				tmp = new TowerTask(tower, new List<Node>());
				foreach (var neighbour in tower.neighbours) {
					if(!neighbour.HasOccupant()) {
						var node = PathTowardTowerNode(unit, neighbour, out tmp.turnsToTower);
						if(node != null)
							tmp.toMoveTo.AddRange(node);
					}
				}
				nodeToBeMovedTo.Add(tmp);
			}
			return nodeToBeMovedTo;
		}

		public static void ResetTowerFocus() {
			foreach (var tower in towersToSubscribe) {
				tower.ResetFocus();
			}
		}
	}
}
