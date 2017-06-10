using System.Collections;
using CombatWorld;
using CombatWorld.Utility;
using UnityEngine;

namespace SimplDynTut {
	public class CombatWorldTriggers : MonoBehaviour {
		public float summonDisplayTimer = 8f, unitSelectionTimer = 10f, endTurnTimer = 12f, tryingToAttackTime = 40f, selectingSummonedUnitTime = 180f, selectingUnitNoMovesLeftTime = 180f;
		private bool hasTimedUnitSelection;
		public int turnsTheEndTurnRuns = 3;
		[HideInInspector]
		public int turnEndTurnhasRun = 0;
		private Coroutine endTurnRoutine;

		private int timesTryingToAttack {
			get { return PlayerData.Instance.timesTryingToAttack;  }
		}
		private int timesTryingToSelectSummon {
			get { return PlayerData.Instance.timesTryingToSelectSummon; }
		}
		private int timesTryingToSelectUnitWithoutMovesLeft {
			get { return PlayerData.Instance.timesTryingToSelectUnitWithoutMovesLeft; }
		}
		
		public int limitTimesTryingToAttack = 3, limitTimesTryingToSelectSummon = 5, limitTimesTryingToSelectUnitWithoutMovesLeft = 5;
		
		void Start () {
			
			GameController.instance.StartedAWaitingForUnit += CheckForMovesLeftForEndTurn;
				
			if(!PlayerData.Instance.hasRunSummonDisplayTimer)
				StartCoroutine(SummonTimer());
		}

		public void StartEndTurn() {
			CheckForMovesLeftForEndTurn();
		}

		private void CheckForMovesLeftForEndTurn() {
			if(turnEndTurnhasRun >= turnsTheEndTurnRuns){
				GameController.instance.StartedAWaitingForUnit -= CheckForMovesLeftForEndTurn;
				return;
			}
			
			foreach (var unit in GameController.instance.GettAllUnitsOfTeam(Team.Player)) {
				if (unit.CanMove() || unit.GetNode().HasAttackableNeighbour()){
					Debug.Log("Not done");
					return;
				}
			}
			if(endTurnRoutine!=null)
				StopCoroutine(endTurnRoutine);
			endTurnRoutine = StartCoroutine(EndTurnTimer());
		}

		public void StopEndTurnTimer() {
			StopCoroutine(endTurnRoutine);
		}

		private IEnumerator EndTurnTimer() {
			yield return new WaitForSeconds(endTurnTimer);
			if(GameController.instance.stillPlayerTurn){
				PlayerData.Instance.hasShownEndTurn = true;
				Debug.Log("End turn trigger, triggered show the end turn tut information");
			}
		}

		private IEnumerator SummonTimer() {
			PlayerData.Instance.hasRunSummonDisplayTimer = true;
			yield return new WaitForSeconds(summonDisplayTimer);
			if(!PlayerData.Instance.GetHasEverSummonedAUnit())
				Debug.Log("Summon display triggered Put in tutorial to show");
		}

		public void StartUnitSelectionTimer() {
			if (!hasTimedUnitSelection && PlayerData.Instance.GetHasEverSelectedAUnit()) {
				hasTimedUnitSelection = true;
				StartCoroutine(UnitSelection());
			}
		}

		
		
		private IEnumerator UnitSelection() {
			GameController.instance.isLookingForUnitSelection = true;
			yield return new WaitForSeconds(unitSelectionTimer);
			if(!GameController.instance.selectedAUnit)
				Debug.Log("Show the unit selection tutorial tut");
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
				timeVar++;
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
			Debug.Log("Show the basic turn understanding, the player tried to select units multiple times that has no moves left");
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
				timeVar++;
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
			Debug.Log("Show the summon sickness, the player tried to select units multiple times that still has summon sickness");
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
				timeVar++;
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
			Debug.Log("Show the attacking, the player tried to attack units multiple times without being in proximity");
		}


	}
}
