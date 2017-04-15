﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using CombatWorld.AI;
using Overworld;
using UnityEngine.SceneManagement;

namespace CombatWorld {
	public class GameController : Singleton<GameController> {
		public GameObject winLosePanel;
		public Text winLoseText;
		public Button endTurnButton;
		public Animator endButtonAnim;

		List<GameObject> maps = new List<GameObject>();

		List<Node> allNodes = new List<Node>();
		List<SummonNode> playerSummonNodes = new List<SummonNode>();
		List<SummonNode> AISummonNodes = new List<SummonNode>();
		public Team currentTeam;

		Pathfinding pathfinding;

		Unit selectedUnit;

		int AITowersremaining = 0;
		int PlayerTowersRemaining = 0;

		bool waitingForAction = false;
		List<Unit> performingAction = new List<Unit>();

		public static bool playerVSPlayer = true;

		public Text TurnIndicator;
		public Text summonPointTurnNumber;
		public Animator summonPointTurnAnim;

		void Start() {
			if (GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER)) {
				StartCoroutine(FadeIn());
			}
			AudioHandler.instance.StartCWBGMusic();
			maps.AddRange(Resources.LoadAll<GameObject>("Art/3D/Maps"));
			pathfinding = new Pathfinding();
			SpawnMap();
			StartGame();
			summonPointTurnNumber.text = DamageConstants.SUMMONPOINTSPERTURN.ToString();
		}

		IEnumerator FadeIn() {
			yield return new WaitForSeconds(1f);
			while (FadingLoadingScreen.instance.isFading) {
				yield return new WaitForSeconds(0.3f);
			}
			FadingLoadingScreen.instance.StartFadeIn();
		}

		void SpawnMap() {
			//Get correct map based on scenehandler info.
			MapTypes type = MapTypes.ANY;
			if (SceneHandler.instance != null) {
				type = SceneHandler.instance.GetMapType();
			}
			if (type != MapTypes.ANY) {
				maps.RemoveAll(g => g.GetComponent<MapInfo>().type != type);
			}
			GameObject go = Instantiate(maps[Random.Range(0, maps.Count)], transform.position, Quaternion.identity, transform) as GameObject;
			go.transform.position = go.transform.position - new Vector3(go.GetComponent<MapInfo>().mapLength, 0, 0);
			CombatCameraController.instance.setBoundary(new Vector2(-go.GetComponent<MapInfo>().mapLength, go.GetComponent<MapInfo>().mapLength));
			Debug.Log("Loaded map: " + go.GetComponent<MapInfo>().Name);
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

		public void TryEndTurn() {
			if (waitingForAction) {
				return;
			}
			if (playerVSPlayer) {
				EndTurn();
				return;
			}
			if(currentTeam != Team.Player) {
				return;
			}
			EndTurn();
		}

		public void EndTurn() {
			StartCoroutine(prepEndTurn());
		}

		IEnumerator prepEndTurn() {
			yield return new WaitUntil(() => !waitingForAction);
			switch (currentTeam) {
				case Team.Player:
					//CombatCameraController.instance.StartAICAM();
					endButtonAnim.SetTrigger("MoreMoves");
					AITurn();
					endTurnButton.interactable = false;
					ResetAllNodes();
					currentTeam = Team.AI;
					AICalculateScore.instance.GiveSummonPoints(DamageConstants.SUMMONPOINTSPERTURN);
					CheckWinLose();
					StartTurn();
					AICalculateScore.instance.DoAITurn();
					if (playerVSPlayer) {
						SummonHandler.instance.SetPlayerTurn(Team.AI);
						SummonHandler.instance.GivePoints(0);
						endTurnButton.interactable = true;
						ResetAllNodes();
						SelectTeamNodes();
					}
					break;
				case Team.AI:
					PlayerTurn();
					//CombatCameraController.instance.EndAICAM();
					CombatCameraController.instance.PlayerTurnsCam(GettAllUnitsOfTeam(Team.Player));
					currentTeam = Team.Player;
					if (playerVSPlayer) {
						SummonHandler.instance.SetPlayerTurn(Team.Player);
					}
					SummonHandler.instance.GivePoints(DamageConstants.SUMMONPOINTSPERTURN);
					CheckWinLose();
					StartTurn();
					ResetAllNodes();
					SelectTeamNodes();
					endTurnButton.interactable = true;
					break;
				default:
					break;
			}
		}

		void StartTurn() {
			foreach (Node node in allNodes) {
				if (node.HasUnit() && node.GetOccupant().GetTeam() == currentTeam){
					node.GetUnit().NewTurn();
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
				HighlightSelectableUnits();
				if (selectedUnit.CanMove()) {
					if (selectedUnit.IsShadowUnit()) {
						HighlightMoveableNodes(pathfinding.GetAllReachableNodes(selectedUnit.GetNode(), selectedUnit.GetMoveDistance()));
					}
					else {
						HighlightMoveableNodes(pathfinding.GetAllNodesWithinDistance(selectedUnit.GetNode(), selectedUnit.GetMoveDistance()));
					}
					if (selectedUnit.IsStoneUnit()) {
						selectedUnit.GetNode().SetState(HighlightState.SelfClick);
					}
				}

				if (selectedUnit.CanAttack()) {
					HighlightAttackableNodes();
					if (selectedUnit.IsStoneUnit()) {
						selectedUnit.GetNode().SetState(HighlightState.SelfClick);
					}
				}
			}
		}

		void HighlightSelectableUnits() {
			foreach (Node node in allNodes) {
				if (node.HasUnit() && node.GetOccupant().GetTeam() == Team.Player && currentTeam == Team.Player) {
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
				else if(playerVSPlayer && currentTeam == Team.AI && node.HasUnit() && node.GetOccupant().GetTeam() == Team.AI) {
					if (node.GetUnit().CanMove()) {
						node.SetState(HighlightState.Selectable);
					}
					else if (node.GetUnit().CanAttack()) {
						node.SetState(HighlightState.NoMoreMoves);
					}
					else {
						node.SetState(HighlightState.NotMoveable);
					}
				}
			}
		}

		void HighlightMoveableNodes(List<Node> reacheableNodes) {
			HighlightSelectableUnits();
			foreach (Node node in reacheableNodes) {
				if (node.HasOccupant()) {
					if(node.HasUnit() && node.GetUnit().GetTeam() != selectedUnit.GetTeam()) {
						node.SetState(HighlightState.NotMoveable);
					}
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
						node.SetState(HighlightState.Summon);
					}
					else {
						node.SetState(HighlightState.NotMoveable);
					}
				}
			}
			else if(playerVSPlayer) {
				foreach (SummonNode node in AISummonNodes) {
					if (!node.HasOccupant()) {
						node.SetState(HighlightState.Summon);
					}
					else {
						node.SetState(HighlightState.NotMoveable);
					}
				}
			}
		}

		public void SummonNodeClickHandler(SummonNode node) {
			SummonHandler.instance.SummonUnit(node);
			CheckPlayerHasMoves();
		}

		public void MoveUnit(Node node) {
			if(selectedUnit.GetTeam() == Team.Player) {
				movingPlayerUnit = true;
			}
			selectedUnit.Move(pathfinding.GetPathTo(node));
		}

		public void NodeGotSelfClick() {
			selectedUnit.TurnToStone();
		}

		public void GotInput() {
			ResetAllNodes();
			SelectTeamNodes();
		}

		bool movingPlayerUnit = false;

		public void UnitMadeAction() {
			if (!movingPlayerUnit || (selectedUnit != null && !selectedUnit.GetNode().HasAttackableNeighbour())) {
				selectedUnit = null;
			}
			movingPlayerUnit = false;
			waitingForAction = false;
			if (currentTeam == Team.Player || playerVSPlayer) {
				SelectTeamNodes();
				endTurnButton.interactable = true;
				CheckPlayerHasMoves();
			}
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
			StartCoroutine(Unload());
		}

		private IEnumerator Unload() {
			FadingLoadingScreen.instance.StartFadeOut();
			while (FadingLoadingScreen.instance.isFading) {
				yield return new WaitForSeconds(0.1f);
			}
			SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		}

		public bool WaitingForAction() {
			return performingAction.Count > 0;
		}

		public void AddWaitForUnit(Unit unit) {
			endTurnButton.interactable = false;
			if (performingAction.Contains(unit)) {
				Debug.LogWarning("Was already performing an action " + unit);
			}
			else {
				performingAction.Add(unit);
			}
			waitingForAction = true;
		}

		public void PerformedAction(Unit unit) {
			if (performingAction.Contains(unit)) {
				performingAction.Remove(unit);
			}
			if(performingAction.Count == 0) {
				UnitMadeAction();
			}
		}

		public List<Unit> GetAllUnits() {
			List<Unit> units = new List<Unit>();
			foreach (Node node in allNodes) {
				if (node.HasUnit()) {
					units.Add(node.GetUnit());
				}
			}
			return units;
		}

		public List<Unit> GettAllUnitsOfTeam(Team team) {
			List<Unit> units = new List<Unit>();
			foreach (Node node in allNodes) {
				if(node.HasUnit() && node.GetUnit().GetTeam() == team) {
					units.Add(node.GetUnit());
				}
			}
			return units;
		}

		private void PlayerTurn() {
			TurnIndicator.text = "Your turn";
			GiveTurnSummonPoints();
			StartCoroutine(HideText());	
		}

		private void AITurn() {
			TurnIndicator.text = "Enemy turn";
			StartCoroutine(HideText());
		}

		private IEnumerator HideText() {
			TurnIndicator.gameObject.SetActive(true);
			yield return new WaitForSeconds(1);
			TurnIndicator.gameObject.SetActive(false);
		}

		private void GiveTurnSummonPoints() {
			summonPointTurnNumber.text = DamageConstants.SUMMONPOINTSPERTURN.ToString();
			summonPointTurnAnim.SetTrigger("TurnPoints");
		}

		private void CheckPlayerHasMoves() {
			if(SummonHandler.instance.HasPointsToSummon()){
				foreach (Node node in playerSummonNodes) {
					if (!node.HasOccupant()) {
						return;
					}
				}
			}
			foreach (Node node in allNodes) {
				if(node.HasUnit() && node.GetUnit().GetTeam() == Team.Player && (node.GetUnit().CanMove() || (node.GetUnit().CanAttack() && node.HasAttackableNeighbour()))) {
					return;
				}
			}
			endButtonAnim.SetTrigger("NoMoreMoves");
		}

		#region SummonPoints

		public void UnitDied(Team team, Node node) {
			if(team == Team.AI) {
				SummonHandler.instance.GivePoints(DamageConstants.SUMMONPOINTSPERKILL);
				AICalculateScore.instance.RemoveAIUnit(node.GetUnit());
			}
			else {
				AICalculateScore.instance.GiveSummonPoints(DamageConstants.SUMMONPOINTSPERKILL);
			}
			node.ResetState();
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
				SummonHandler.instance.GivePoints(DamageConstants.SUMMONPOINTSPERTOWERKILL);
			}
			else {
				PlayerTowersRemaining--;
				if(PlayerTowersRemaining == 0) {
					Lost();
					return;
				}
				AICalculateScore.instance.GiveSummonPoints(DamageConstants.SUMMONPOINTSPERTOWERKILL);
			}
		}

		public List<Node> GetTowersForTeam(Team team) {
			List<Node> toReturn = new List<Node>();
			foreach (Node node in allNodes) {
				if(node.HasOccupant() && !node.HasUnit() && node.GetOccupant().GetTeam() == team) {
					toReturn.Add(node);
				}
			}
			return toReturn;
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
				if (!AllSummonNodesOccupied() && SummonHandler.instance.HasPointsToSummon(AIController.instance.summonPoints)) {
					return;
				}
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
			AudioHandler.instance.PlayWinSound();
			SceneHandler.instance.Won();
		}

		void Lost() {
			winLoseText.text = "YOU LOST!";
			winLosePanel.SetActive(true);
			AudioHandler.instance.PlayLoseSound();
			SceneHandler.instance.Lost();
		}

		public void GiveUp() {
			Lost();
		}

		#endregion

		#region AI THINGS

		public List<SummonNode> GetAISummonNodes() {
			return AISummonNodes;
		}

		public List<SummonNode> GetPlayerSummonNodes() {
			return playerSummonNodes;
		}

		#endregion
	}
}