using System.Collections;
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
		Team currentTeam;

		public bool gameFinished;

		Pathfinding pathfinding;

		Unit selectedUnit;

		int AITowersremaining = 0;
		int PlayerTowersRemaining = 0;

		bool waitingForAction = false;
		List<Unit> performingAction = new List<Unit>();

		public Text TurnIndicator;
		public Text summonPointTurnNumber;
		public Animator summonPointTurnAnim;

		InGameMenu menu;

		void Start() {
			menu = FindObjectOfType<InGameMenu>();
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

			//			Debug.Log("Loaded map: " + go.GetComponent<MapInfo>().Name);

			if (TutorialHandler.instance != null) {
				if (TutorialHandler.instance.combatFirstTurn) {
					//TutorialHandler.instance.ShowGoalAndSummon();

				}
			}
		}

		void StartGame() {
			gameFinished = false;
			DataGathering.Instance.StartNewCombat();
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
			if (waitingForAction || gameFinished) {
				return;
			}
			if(currentTeam != Team.Player) {
				return;
			}
			EndTurn();
		}

		public void TryEndAITurn() {
			if (currentTeam != Team.AI) {
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
					endButtonAnim.SetBool("MoreMoves",true);
					AITurn();
					DoBaordCalculations();
					CombatCameraController.instance.StartAICAM();
					endTurnButton.interactable = false;
					ResetAllNodes();
					currentTeam = Team.AI;
					AICalculateScore.instance.GiveSummonPoints(DamageConstants.SUMMONPOINTSPERTURN);
					CheckWinLose();
					StartTurn();
					TowerNodes();
					AICalculateScore.instance.DoAITurn();
					break;
				case Team.AI:
					PlayerTurn();
					CombatCameraController.instance.EndAICAM();
					CombatCameraController.instance.PlayerTurnsCam(GettAllUnitsOfTeam(Team.Player));
					currentTeam = Team.Player;
					SummonHandler.instance.GivePoints(DamageConstants.SUMMONPOINTSPERTURN);
					CheckWinLose();
					StartTurn();
					SelectTeamNodes();
					endTurnButton.interactable = true;
					TowerNodes();
					break;
				default:
					break;
			}
		}

		void DoBaordCalculations() {
			//Check if player has stood beside tower and did not attack it.
			//TODO: make a check, that it should not do this, if the player has killed a tower already.
			foreach (Node node in allNodes) {
				if(node.HasUnit() && node.GetUnit().GetTeam() == Team.Player) {
					if (node.GetUnit().CanAttack()) {
						foreach (Node nodeNeighbour in node.GetUnit().GetNode().GetNeighbours()) {
							if (nodeNeighbour.HasTower() && nodeNeighbour.GetOccupant().GetTeam() == Team.AI) {
								DataGathering.Instance.StoodBesideTowerAndDidNotAttack();
							}
						}
					}
				}
			}
		}

		public int GetAmountOfOccupiedPlayerSummonSpots() {
			int temp = playerSummonNodes.Count;
			foreach (SummonNode node in playerSummonNodes) {
				if (node.HasOccupant()) {
					temp--;
				}
			}
			return temp;
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
			if (waitingForAction || gameFinished) {
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
			DataGathering.Instance.DeselectUnit();
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
			TowerNodes();
			SummonHandler.instance.UpdateButtonsAndText();
		}
 
		void TowerNodes() {
			foreach (Node node in GetTowersForTeam(Team.AI)) {
				if (node.HasTower()) {
					node.GetTower().CanBeAttacked(currentTeam == Team.Player);
				}
			}
		}

		bool movingPlayerUnit = false;

		public void UnitMadeAction() {
			if (!movingPlayerUnit || (selectedUnit != null && !selectedUnit.GetNode().HasAttackableNeighbour())) {
				selectedUnit = null;
				DataGathering.Instance.DeselectUnit();
			}
			movingPlayerUnit = false;
			waitingForAction = false;
			if (currentTeam == Team.Player) {
				SelectTeamNodes();
				endTurnButton.interactable = true;
				CheckPlayerHasMoves();
			}
		}

		public void SetSelectedUnit(Unit unit) {
			selectedUnit = unit;
			DataGathering.Instance.SelectedUnit(unit);
		}

		public Unit GetSelectedUnit() {
			return selectedUnit;
		}

		public void ClickedNothing() {
			selectedUnit = null;
			DataGathering.Instance.DeselectUnit();
			SummonHandler.instance.UpdateButtonsAndText();
			ResetAllNodes();
			SelectTeamNodes();
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
			DynamicTut.instance.CheckForDynamicTuts();

			TurnIndicator.text = "Your turn";
			GiveTurnSummonPoints();
			StartCoroutine(HideText());

			if (TutorialHandler.instance != null) {
				if (TutorialHandler.instance.combatSecondTurn) {
					//TutorialHandler.instance.StartingTurnSecondTurn();
				}
			}
			if (TutorialHandler.instance != null) {
				if (TutorialHandler.instance.combatThirdTurn) {
					//TutorialHandler.instance.combatThirdTurn = false;
					//TutorialHandler.instance.StartingThirdTurn();
				}
			}
		}

		private void AITurn() {
			if (TutorialHandler.instance != null) {
				if (TutorialHandler.instance.combatSecondTurn) {
					//TutorialHandler.instance.combatSecondTurn = false;
					//TutorialHandler.instance.combatThirdTurn = true;
				}
			}

			if (TutorialHandler.instance != null) {
				if (TutorialHandler.instance.combatFirstTurn) {
					//TutorialHandler.instance.combatFirstTurn = false;
					//TutorialHandler.instance.combatSecondTurn = true;
					//TutorialHandler.instance.combatThirdTurn = false;
				}
			}

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
			endButtonAnim.SetBool("MoreMoves",false);
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

		public void Won() {
			gameFinished = true;
			ResetAllNodes();
			DataGathering.Instance.AddCombatTrade(new CombatTrades(){ initiator = Team.NONE, killHit = true});
			//DataToServer.SendData(this);
			won = true;
			winLoseText.text = "YOU WON!";
			winLosePanel.SetActive(true);
			AudioHandler.instance.PlayWinSound();
			SaveLoadHandler.Instance.Save(SaveLoadHandler.Instance.GetCheckpoint());
		}

		void Lost() {
			gameFinished = true;
			ResetAllNodes();
			DataGathering.Instance.AddCombatTrade(new CombatTrades(){ initiator = Team.NONE, killHit = false});
			//DataToServer.SendData(this);
			won = false;
			winLoseText.text = "YOU LOST!";
			winLosePanel.SetActive(true);
			AudioHandler.instance.PlayLoseSound();
			SaveLoadHandler.Instance.Save(SaveLoadHandler.Instance.GetCheckpoint());
		}

		public void GiveUp() {
			Lost();
		}

		public void Forfeit() {
			StartCoroutine(Unload());
			if (won) {
				SceneHandler.instance.Won();
			}
			else {
				SceneHandler.instance.Lost();
			}
		}

		bool won;

		#endregion

		#region AI THINGS

		public List<SummonNode> GetAISummonNodes() {
			return AISummonNodes;
		}

		public List<SummonNode> GetPlayerSummonNodes() {
			return playerSummonNodes;
		}

		#endregion

		public bool AcceptsInput() {
			return !(GeneralConfirmationBox.instance != null && GeneralConfirmationBox.instance.IsOpen) && !(menu != null && menu.isShowing);
		}
	}
}