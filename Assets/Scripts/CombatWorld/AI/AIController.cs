using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld.AI {
	public class AIController : Singleton<AIController> {

		public GameObject unitToSummon;

		List<Unit> units = new List<Unit>();

		Team team = Team.AI;

		Pathfinding pathfinding = new Pathfinding();

		public void MyTurn() {
			CheckForDeadUnits();
			SummonUnit();
			AttackWithUnits();
			MoveUnits();
			GameController.instance.EndTurn();
		}

		void SummonUnit() {
			List<SummonNode> nodes = GameController.instance.GetAISummonNodes();
			foreach (Node node in nodes) {
				if (!node.HasOccupant()) {
					GameObject unit = Instantiate(unitToSummon, node.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
					unit.GetComponent<Unit>().SpawnEntity(node, team);
					units.Add(unit.GetComponent<Unit>());
					return;
				}
			}
		}

		void MoveUnits() {
			foreach (Unit unit in units) {
				if (unit.CanMove()) {
					var possibleNodes = pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance());
					for (int i = possibleNodes.Count-1; i > 0; i--) {
						if (possibleNodes[i].HasOccupant()) {
							possibleNodes.RemoveAt(i);
						}
					}
					if (possibleNodes.Count > 0) {
						unit.Move(possibleNodes[Random.Range(0, possibleNodes.Count)]);
					}
				}
			}
		}

		void AttackWithUnits() {
			foreach (Unit unit in units) {
				foreach (Node neighbour in unit.GetNode().GetNeighbours()) {
					if(neighbour.HasOccupant() && neighbour.GetOccupant().GetTeam() != team) {
						unit.Attack(neighbour.GetOccupant());
						break;
					}
				}
			}
		}

		void CheckForDeadUnits() {
			for (int i = units.Count-1; i >= 0; i--) {
				if (units[i] == null) {
					units.RemoveAt(i);
				}
			}
		}
	}
}