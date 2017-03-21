using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld.AI {
	public class AIController : Singleton<AIController> {

		public GameObject unitToSummon;

		public CombatData data;

		List<Unit> units = new List<Unit>();

		Team team = Team.AI;

		Pathfinding pathfinding = new Pathfinding();

		int summonPoints = 2;

		public void MyTurn() {
			CheckForDeadUnits();
			StartCoroutine(SummonUnit());
			
		}

		IEnumerator SummonUnit() {
			List<SummonNode> nodes = GameController.instance.GetAISummonNodes();
			foreach (Node node in nodes) {
				if (!node.HasOccupant()) {
					GameObject unit = Instantiate(unitToSummon, node.transform.position, Quaternion.identity) as GameObject;
					unit.GetComponent<Unit>().SpawnEntity(node, team, data);
					units.Add(unit.GetComponent<Unit>());
					break;
				}
			}
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(AttackWithUnits());
		}

		IEnumerator AttackWithUnits() {
			foreach (Unit unit in units) {
				while (GameController.instance.WaitingForAction()) {
					yield return new WaitForSeconds(0.1f);
				}
				if (!unit.CanAttack()) {
					continue;
				}
				foreach (Node neighbour in unit.GetNode().GetNeighbours()) {
					if (neighbour.HasOccupant() && neighbour.GetOccupant().GetTeam() != team) {
						unit.Attack(neighbour.GetOccupant());
						break;
					}
				}
			}
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(MoveUnits());
		}

		IEnumerator MoveUnits() {
			foreach (Unit unit in units) {
				while (GameController.instance.WaitingForAction()) {
					yield return new WaitForSeconds(0.1f);
				}
				if (unit.CanMove()) {
					var possibleNodes = pathfinding.GetAllNodesWithinDistance(unit.GetNode(), unit.GetMoveDistance());
					for (int i = possibleNodes.Count-1; i > 0; i--) {
						if (possibleNodes[i].HasOccupant()) {
							possibleNodes.RemoveAt(i);
						}
					}
					if (possibleNodes.Count > 0) {

						unit.Move(pathfinding.GetPathTo(possibleNodes[Random.Range(0, possibleNodes.Count)]));
					}
				}
			}
			GameController.instance.EndTurn();
		}

		void CheckForDeadUnits() {
			for (int i = units.Count-1; i >= 0; i--) {
				if (units[i] == null) {
					units.RemoveAt(i);
				}
			}
		}

		public void GiveSummonPoints(int amount) {
			summonPoints += amount;
		}
	}
}