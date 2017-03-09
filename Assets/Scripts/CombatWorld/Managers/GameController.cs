using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using CombatWorld.AI;
using UnityEngine.SceneManagement;

namespace CombatWorld {
	public class GameController : Singleton<GameController> {
		List<Node> allNodes = new List<Node>();
		List<SummonNode> playerSummonNodes = new List<SummonNode>();
		List<SummonNode> AISummonNodes = new List<SummonNode>();
		Team currentTeam;

		Pathfinding pathfinding;

		Unit selectedUnit;

		void Start() {
			pathfinding = new Pathfinding();
			StartGame();
		}

		void StartGame() {
			currentTeam = Team.Player;
			ResetAllNodes();
			SelectTeamNodes();
		}

		public void AddNode(Node node) {
			allNodes.Add(node);
		}

		public void AddTeamNode(SummonNode node, Team team) {
			if (team == Team.Player) {
				playerSummonNodes.Add(node);
			}
			else {
				AISummonNodes.Add(node);
			}
		}

		public void EndTurn() {
			switch (currentTeam) {
				case Team.Player:
					currentTeam = Team.AI;
					StartTurn();
					AIController.instance.MyTurn();
					break;
				case Team.AI:
					currentTeam = Team.Player;
					StartTurn();
					ResetAllNodes();
					SelectTeamNodes();
					break;
				default:
					break;
			}
		}

		void StartTurn() {
			foreach (Node node in allNodes) {
				if (node.HasOccupant() && node.GetOccupant().GetTeam() == currentTeam){
					node.GetOccupant().newTurn();
				}
			}
		}

		void ResetAllNodes() {
			foreach (Node node in allNodes) {
				node.ResetState();
			}
		}

		void SelectTeamNodes() {
			if (selectedUnit == null) {
				HighlightSelectableUnits();
			}
			else {
				if (selectedUnit.CanMove()) {
					HighlightMoveableNodes(pathfinding.GetAllNodesWithinDistance(selectedUnit.GetNode(), selectedUnit.GetMoveDistance()));
				}
				if (selectedUnit.CanAttack()) {
					HighlightAttackableNodes();
				}
			}
		}

		void HighlightSelectableUnits() {
			foreach (Node node in allNodes) {
				if (node.HasOccupant() && node.GetOccupant().GetTeam() == currentTeam) {
					if (node.GetOccupant().CanMove()) {
						node.SetState(HighlightState.Selectable);
					}
					else if(node.GetOccupant().CanAttack()) {
						node.SetState(HighlightState.NoMoreMoves);
					}
					else {
						node.SetState(HighlightState.NotMoveable);
					}
				}
			}
		}

		void HighlightMoveableNodes(List<Node> reacheableNodes) {
			foreach (Node node in reacheableNodes) {
				if (node.HasOccupant()) {
					node.SetState(HighlightState.NotMoveable);
				}
				else {
					node.SetState(HighlightState.Moveable);
				}
			}
		}

		void HighlightAttackableNodes() {
			foreach (Node node in selectedUnit.GetNode().GetNeighbours()) {
				if(node.HasOccupant() && node.GetOccupant().GetTeam() != currentTeam) {
					node.SetState(HighlightState.Attackable);
				}
			}
			selectedUnit.GetNode().SetState(HighlightState.NotMoveable);
		}

		public void HighlightSummonNodes() {
			selectedUnit = null;
			ResetAllNodes();
			if (currentTeam == Team.Player) {
				foreach (SummonNode node in playerSummonNodes) {
					if (!node.HasOccupant()) {
						node.SetState(HighlightState.Moveable);
					}
					else {
						node.SetState(HighlightState.NotMoveable);
					}
				}
			}
		}

		public void SummonNodeClickHandler(SummonNode node) {
			if (selectedUnit != null) {
				selectedUnit.Move(node);
			}
			else {
				SummonHandler.instance.SummonUnit(node);
			}
		}

		public void GotInput() {
			ResetAllNodes();
			SelectTeamNodes();
		}

		public void UnitMadeAction() {
			selectedUnit = null;
		}

		public void SetSelectedUnit(Unit unit) {
			selectedUnit = unit;
		}

		public Unit GetSelectedUnit() {
			return selectedUnit;
		}

		public void ClickedNothing() {
			selectedUnit = null;
			ResetAllNodes();
			SelectTeamNodes();
		}

		public void Forfeit() {
			SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		}

		#region AI THINGS

		public List<SummonNode> GetAISummonNodes() {
			return AISummonNodes;
		}

		#endregion
	}
}