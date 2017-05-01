using System.Collections;
using System.Collections.Generic;
using CombatWorld;
using Overworld;
using UnityEngine;

public class DynamicTut : MonoBehaviour {

	public bool isDynamic;
	private string textToShow = "", buttonOne = "Okay";
	public float moveDelay = 3f, endTurnDelay = 4f, playerSelectionDelay = 4f;

	private bool showGoalAndSummon, summonSickness, playerMovement, winning, losing, playerAttacking, endTurn;

	//TODO: Add all the different types of tuts we want to be shown in here

	void Start () {
		if(isDynamic){
			Debug.LogWarning("Tutorial is dynamic!");
			TutorialHandler.instance.isDynamic = isDynamic;
		}
		else{
			Debug.LogWarning("Tutorial is NOT dynamic!");
			TutorialHandler.instance.isDynamic = isDynamic;
		}

	}


	void Update () {
	}


	public void ContinueTheDynamicTutCycle() {
		if(!isDynamic)
			return;
		if(!showGoalAndSummon)
			ShowGoalAndSummon();
		if(!summonSickness)
			ShowSummonSicknessAndTurn();
		if(!playerMovement)
			PlayerMovingUnit();
		if(!winning)
			PlayerWinning();
		if(!losing)
			PlayerLosing();
		if(!playerAttacking)
			PlayerAttackingUnit();
	}


	void ShowGoalAndSummon() {
		summonSickness = true;
		StartCoroutine(TimerForPlayerMove());
	}

	void ShowSummonSicknessAndTurn() {

	}

	void ShowTurnInteractionAndControlForUnitMovement() {

	}

	void PlayerSelection() {
		StartCoroutine(TimeForPlayerSelectingUnit());
	}

	void PlayerMovement() {

	}

	void PlayerMovingUnit() {
	}

	void PlayerAttackingUnit() {
	}

	void PlayerEndTurn() {
		StartCoroutine(TimeForPlayerEndTurn());
	}

	void PlayerWinning() {
	}

	void PlayerLosing() {
	}

	IEnumerator TimeForPlayerSelectingUnit() {
		yield return new WaitForSeconds(playerSelectionDelay);
		if(DataGathering.Instance.HasEverHadSelectedUnit())
			yield break;
		Debug.Log("See if player selects a unit");
	}

	IEnumerator TimeForPlayerEndTurn() {
		yield return new WaitForSeconds(endTurnDelay);
		if(DataGathering.Instance.GetAllSummonedUnits().Count != 0 || GameController.instance.GetAmountOfOccupiedPlayerSummonSpots() >= 2 )
			yield break;
		TutorialHandler.instance.EndTurn(true);
	}

	IEnumerator TimerForPlayerMove() {
		yield return new WaitForSeconds(moveDelay);
		Debug.Log("Needs Datagathering Has Moved a unit");
		/*if (DataGathering.Instance.HasMoved) {
			yield break;
		}*/
		TutorialHandler.instance.ShowGoalAndSummon(true);
	}
}
