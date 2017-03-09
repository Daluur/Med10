using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using CombatWorld.AI;

namespace CombatWorld {
	public class GameController : Singleton<GameController> {
		public GameObject winLosePanel;
		public Text winLoseText;

		List<Node> allNodes = new List<Node>();
		List<SummonNode> playerSummonNodes = new List<SummonNode>();
		List<SummonNode> AISummonNodes = new List<SummonNode>();
		Team currentTeam;

		Pathfinding pathfinding;

		Unit selectedUnit;

		int AITowersremaining = 0;
		int PlayerTowersRemaining = 0;

		bool waitingForAction = false;

		void Start() {
			pathfinding = new Pathfinding();
			StartGame();
		}

		void StartGame() {
			currentTeam = Team.Player;
			ResetAllNodes();
			SelectTeamNodes();
		}

		#region setup

		public void AddTower(Team team) {
			if (team == Team.AI) {
				AITowersremaining++;
			}
			else {
				PlayerTowersRemaining++;
			}
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

		#endregion

		public void EndTurn() {
			switch (currentTeam) {
				case Team.Player:
					ResetAllNodes();
					currentTeam = Team.AI;
					AIController.instance.GiveSummonPoints(2);
					CheckWinLose();
					StartTurn();
					AIController.instance.MyTurn();
					break;
				case Team.AI:
					currentTeam = Team.Player;
					SummonHandler.instance.GivePoints(2);
					CheckWinLose();
					StartTurn();
					SelectTeamNodes();
					break;
				default:
					break;
			}
		}

		void StartTurn() {
			foreach (Node node in allNodes) {
				if (node.HasUnit() && node.GetOccupant().GetTeam() == currentTeam){
					node.GetUnit().newTurn();
				}
			}
		}

		void ResetAllNodes() {
			foreach (Node node in allNodes) {
				node.ResetState();
			}
		}

		void SelectTeamNodes() {
			if (waitingForAction) {
				return;
			}
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
				if (node.HasUnit() && node.GetOccupant().GetTeam() == Team.Player) {
					if (node.GetUnit().CanMove()) {
						node.SetState(HighlightState.Selectable);
					}
					else if(node.GetUnit().CanAttack()) {
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
			waitingForAction = false;
			if (currentTeam == Team.Player) {
				SelectTeamNodes();
			}
		}

		public void SetSelectedUnit(Unit unit) {
			selectedUnit = unit;
		}

		public Unit GetSelectedUnit() {
			if(selectedUnit == null) {
				var i = 1;
			}
			return selectedUnit;
		}

		public void ClickedNothing() {
			selectedUnit = null;
			ResetAllNodes();
			SelectTeamNodes();
		}

		public void WaitForAction() {
			waitingForAction = true;
		}

		public bool WaitingForAction() {
			return waitingForAction;
		}

		#region SummonPoints

		public void UnitDied(Team team) {
			if(team == Team.AI) {
				SummonHandler.instance.GivePoints(2);
			}
			else {
				AIController.instance.GiveSummonPoints(2);
			}
		}

		#endregion

		#region Towers

		public void DestroyTower(Team team) {
			if(team == Team.AI) {
				AITowersremaining--;
				if(AITowersremaining == 0) {
					Won();
					return;
				}
				SummonHandler.instance.GivePoints(2);
			}
			else {
				PlayerTowersRemaining--;
				if(PlayerTowersRemaining == 0) {
					Lost();
					return;
				}
				AIController.instance.GiveSummonPoints(2);
			}
		}

		#endregion

		#region WinLose

		void CheckWinLose() {
			if(currentTeam == Team.AI) {
				if (UnitFromTeamAlive()) {
					return;
				}
				if (!AllSummonNodesOccupied()) {
					return;
				}
				//TODO check for AI summonpoints.
				Won();
			}
			else {
				if (UnitFromTeamAlive()) {
					return;
				}
				if (!AllSummonNodesOccupied() && SummonHandler.instance.HasPointsToSummon()) {
					return;
				}
				Lost();
			}
		}

		bool UnitFromTeamAlive() {
			foreach (Node node in allNodes) {
				if(node.HasUnit() && node.GetUnit().GetTeam() == currentTeam) {
					return true;
				}
			}
			return false;
		}

		bool AllSummonNodesOccupied() {
			if(currentTeam == Team.AI) {
				foreach (Node node in AISummonNodes) {
					if (!node.HasUnit()) {
						return false;
					}
				}
				return true;
			}
			else {
				foreach (Node node in playerSummonNodes) {
					if (!node.HasUnit()) {
						return false;
					}
				}
				return true;
			}
		}

		void Won() {
			winLoseText.text = "YOU WON!";
			winLosePanel.SetActive(true);
		}

		void Lost() {
			winLoseText.text = "YOU LOST!";
			winLosePanel.SetActive(true);
		}

		#endregion

		#region AI THINGS

		public List<SummonNode> GetAISummonNodes() {
			return AISummonNodes;
		}

		#endregion
	}
}