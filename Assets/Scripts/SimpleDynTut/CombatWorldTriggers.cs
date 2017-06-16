using System.Collections;
using System.Linq;
using CombatWorld;
using CombatWorld.Units;
using CombatWorld.Utility;
using UnityEngine;
using Overworld;

namespace SimplDynTut {
	public class CombatWorldTriggers : MonoBehaviour {
		public float summonDisplayTimer = 8f, unitSelectionTimer = 10f, endTurnTimer = 12f, tryingToAttackTime = 40f, selectingSummonedUnitTime = 180f, selectingUnitNoMovesLeftTime = 180f, hasNotMovedIn = 5f;
		private bool hasTimedUnitSelection;
		public int turnsTheEndTurnRuns = 3;
		public int turnBeforeCameraTut = 2;
		public int turnBeforeZoomTut = 4;
		public int turnToAttackLimit = 5;
		public int limitAmountOfTimesTryingToSelectEnemyUnit = 5;
		private int timesTryingToSelectEnemy {
			get { return PlayerData.Instance.timesTriedToSelectEnemyUnits; }
			set { PlayerData.Instance.timesTriedToSelectEnemyUnits = value; }
		}

		[HideInInspector]
		public int turnEndTurnhasRun = 0;
		private Coroutine endTurnRoutine;
		public int offsetUnitAmount = 10;
		[HideInInspector]
		public int unitStandingNextToTowerBeingAbleToAttackButNotAttacking{
			get { return toBeUsedWithWin; }
			set {
				if (value == 1) {
					toBeUsedWithWin++;
				}
				else {
					toBeUsedWithWin = value;
				}
			}
		}
		private int toBeUsedWithWin = 0;

		public int amountOfTimesUnitStandingNextToTowerDoesNotAttackTriggerWin = 5;
		
		private bool hasShownWin;

		public float limitForHowMuchPlayerHasMovedCamera = 30f;

		private int timesTryingToAttack {
			get { return PlayerData.Instance.timesTryingToAttack;  }
		}
		private int timesTryingToSelectSummon {
			get { return PlayerData.Instance.timesTryingToSelectSummon; }
		}
		private int timesTryingToSelectUnitWithoutMovesLeft {
			get { return PlayerData.Instance.timesTryingToSelectUnitWithoutMovesLeft; }
		}
		private int currentTurn {
			get { return PlayerData.Instance.currentTurn; }
		}
		
		public int limitTimesTryingToAttack = 3, limitTimesTryingToSelectSummon = 5, limitTimesTryingToSelectUnitWithoutMovesLeft = 5;
		
		void Start () {
			GameController.instance.StartedAWaitingForUnit += CheckForMovesLeftForEndTurn;
				
			if(!PlayerData.Instance.hasRunSummonDisplayTimer)
				StartCoroutine(SummonTimer());
		}


		public void CheckForEverAttacked() {
			if (currentTurn == turnToAttackLimit - 1 && !PlayerData.Instance.hasEverAttacked) {
				TutorialHandler.instance.WorldTrigger(17);
			}
		}

		public void CheckForMovement() {
			if (PlayerData.Instance.hasShownMovementCW || PlayerData.Instance.hasMovedInCW)
				return;
			PlayerData.Instance.hasShownMovementCW = true;
			StartCoroutine(MovementTimer());
		}

		private IEnumerator MovementTimer() {
			float timer = 0;
			while (timer < hasNotMovedIn) {
				if (!GeneralConfirmationBox.instance.IsOpen) {
					timer += Time.deltaTime;
				}
				yield return new WaitForEndOfFrame();
			}
			if (!PlayerData.Instance.hasMovedInCW) {
				TutorialHandler.instance.WorldTrigger(13);
			}
		}

		public void CheckUnitsPositions() {
			foreach (var unit in GameController.instance.GettAllUnitsOfTeam(Team.Player)) {
				foreach (var neighbour in unit.GetNode().neighbours) {
					if (neighbour.GetOccupant()!=null && neighbour.GetOccupant().GetType() == typeof(Tower) && neighbour.GetOccupant().GetTeam() == Team.AI && unit.CanAttack()) {
						unitStandingNextToTowerBeingAbleToAttackButNotAttacking++;
					}
				}
			}
		}

		public void CheckForWinCondition() {
			if(hasShownWin)
				return;
			if (( GameController.instance.GettAllUnitsOfTeam(Team.Player).Count -
			      GameController.instance.GettAllUnitsOfTeam(Team.AI).Count ) >= offsetUnitAmount && !GameController.instance.GetTowersForTeam((Team.AI)).Any(e => e.GetTower().GetHealth() < 50) 
			    && GameController.instance.GetTowersForTeam(Team.AI).Count != 2) {
				TutorialHandler.instance.WorldTrigger(15);
				return;
			}
			if (unitStandingNextToTowerBeingAbleToAttackButNotAttacking >=
			    amountOfTimesUnitStandingNextToTowerDoesNotAttackTriggerWin) {
				hasShownWin = true;
				TutorialHandler.instance.WorldTrigger(15);
			}
		}

		public void CheckForTurnInfo() {
			if (currentTurn == turnBeforeCameraTut - 1 && CombatCameraController.instance.playerMovementAmount < limitForHowMuchPlayerHasMovedCamera) {
				TutorialHandler.instance.WorldTrigger(10);
			}
			if (currentTurn == turnBeforeZoomTut - 1 && !CombatCameraController.instance.hasZoomed) {
				TutorialHandler.instance.WorldTrigger(14);
			}
		}

		public void StartEndTurn() {
			CheckForMovesLeftForEndTurn();
		}

		private void CheckForMovesLeftForEndTurn() {
			if(turnEndTurnhasRun >= turnsTheEndTurnRuns || currentTurn >= 3){
				GameController.instance.StartedAWaitingForUnit -= CheckForMovesLeftForEndTurn;
				return;
			}
			
			foreach (var unit in GameController.instance.GettAllUnitsOfTeam(Team.Player)) {
				if (unit.CanMove() || unit.GetNode().HasAttackableNeighbour()){
					return;
				}
			}
			if(endTurnRoutine!=null)
				StopCoroutine(endTurnRoutine);
			endTurnRoutine = StartCoroutine(EndTurnTimer());
		}

		public void StopEndTurnTimer() {
			if(endTurnRoutine!=null)
				StopCoroutine(endTurnRoutine);
		}

		private IEnumerator EndTurnTimer() {
			float timer = 0;
			while (timer < endTurnTimer) {
				if (!GeneralConfirmationBox.instance.IsOpen) {
					timer += Time.deltaTime;
				}
				yield return new WaitForEndOfFrame();
			}
			if (GameController.instance.stillPlayerTurn){
				PlayerData.Instance.hasShownEndTurn = true;
				TutorialHandler.instance.WorldTrigger(6);
			}
		}

		private IEnumerator SummonTimer() {
			PlayerData.Instance.hasRunSummonDisplayTimer = true;
			float timer = 0;
			while (timer < summonDisplayTimer) {
				if (!GeneralConfirmationBox.instance.IsOpen) {
					timer += Time.deltaTime;
				}
				yield return new WaitForEndOfFrame();
			}
			if (!PlayerData.Instance.GetHasEverSummonedAUnit())
				TutorialHandler.instance.WorldTrigger(5);
		}

		public void StartUnitSelectionTimer() {
			if (!hasTimedUnitSelection && !PlayerData.Instance.GetHasEverSelectedAUnit()) {
				hasTimedUnitSelection = true;
				StartCoroutine(UnitSelection());
			}
		}

		
		
		private IEnumerator UnitSelection() {
			GameController.instance.isLookingForUnitSelection = true;
			float timer = 0;
			while (timer < unitSelectionTimer) {
				if (!GeneralConfirmationBox.instance.IsOpen) {
					timer += Time.deltaTime;
				}
				yield return new WaitForEndOfFrame();
			}
			if (!PlayerData.Instance.GetHasEverSelectedAUnit())
				TutorialHandler.instance.WorldTrigger(11);
			GameController.instance.isLookingForUnitSelection = false;
		}

		public void SelectingUnitWithNoMovesLeft() {
			if (timesTryingToSelectUnitWithoutMovesLeft >= limitTimesTryingToSelectUnitWithoutMovesLeft) {
				DisplayBasicUnitUnderstanding();
			}
//if(!PlayerData.Instance.hasRunSelectingUnitWithNoMovesLeft)
				//StartCoroutine(SelectingUnitWithNoMovesLeft());
		}

		private IEnumerator TimerSelectingUnitWithNoMovesLeft() {
			PlayerData.Instance.hasRunSelectingUnitWithNoMovesLeft = true;
			int timeVar = 0;
			while (selectingUnitNoMovesLeftTime > timeVar) {
				if (!GeneralConfirmationBox.instance.IsOpen) {
					timeVar++;
				}
				yield return new WaitForSeconds(1f);
				if(timesTryingToSelectUnitWithoutMovesLeft >= limitTimesTryingToSelectUnitWithoutMovesLeft){
					DisplayBasicUnitUnderstanding();
					yield break;
				}
			}
			Debug.Log("Selecting units with no more moves left has run out");
		}

		private void DisplayBasicUnitUnderstanding() {
			PlayerData.Instance.timesTryingToSelectUnitWithoutMovesLeft = 0;
			TutorialHandler.instance.WorldTrigger(16);
		}

		public void SelectingRecentlySummonedUnit() {
			if (timesTryingToSelectSummon >= limitTimesTryingToSelectSummon) {
				DisplaySummonSicknessTut();
			}
/*
			if(!PlayerData.Instance.hasRunSelectingSummonedUnit)
				StartCoroutine(SelectingRecentlySummonedUnit());*/
		}

		private IEnumerator TimerBasedSelectingRecentlySummonedUnit() {
			PlayerData.Instance.hasRunSelectingSummonedUnit = true;
			int timeVar = 0;
			while (selectingSummonedUnitTime > timeVar) {
				if (!GeneralConfirmationBox.instance.IsOpen) {
					timeVar++;
				}
				yield return new WaitForSeconds(1f);
				if(timesTryingToSelectSummon >= limitTimesTryingToSelectSummon){
					DisplaySummonSicknessTut();
					yield break;
				}
			}
			Debug.Log("Selecting recently summoned unit has expired");
		}

		private void DisplaySummonSicknessTut() {
			PlayerData.Instance.timesTryingToSelectSummon = 0;
			TutorialHandler.instance.WorldTrigger(7);
		}

		public void TryingToAttackFromRangeTut() {
			if (timesTryingToAttack >= limitTimesTryingToAttack) {
				DisplayAttackingTut();
			}
			//if(!PlayerData.Instance.hasRunTryingToAttack)
				
		}

		private IEnumerator TryingToAttack() {
			PlayerData.Instance.hasRunTryingToAttack = true;
			int timeVar = 0;
			while (tryingToAttackTime > timeVar) {
				if (!GeneralConfirmationBox.instance.IsOpen) {
					timeVar++;
				}
				yield return new WaitForSeconds(1f);
				if(timesTryingToAttack >= limitTimesTryingToAttack){
					DisplayAttackingTut();
					yield break;
				}
			}
			Debug.Log("Trying to attack has now run out");
		}

		private void DisplayAttackingTut() {
			PlayerData.Instance.timesTryingToAttack = 0;
			TutorialHandler.instance.WorldTrigger(12);
		}

		public void TryingToSelectEnemyUnits() {
			if (timesTryingToSelectEnemy >= limitAmountOfTimesTryingToSelectEnemyUnit) {
				TutorialHandler.instance.WorldTrigger(18);
				timesTryingToSelectEnemy = 0;
			}
		}

	}
}
