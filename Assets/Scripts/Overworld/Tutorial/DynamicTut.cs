using System;
using System.Collections;
using System.Collections.Generic;
using CombatWorld;
using CombatWorld.Utility;
using NUnit.Framework.Internal;
using Overworld;
using UnityEngine;

public class DynamicTut : Singleton<DynamicTut> {

	public bool isDynamic;
	private string textToShow = "", buttonOne = "Okay";
	public float moveDelay = 3f, endTurnDelay = 4f, playerSelectionDelay = 4f;

	private bool showGoalAndSummon, summonSickness, playerMovement, winning, losing, playerAttacking, endTurn;

	private bool inCombat, shownType, shownRetaliation;
	private Coroutine testing;
	public float cooldownForDynamicTypeTut = 100f, cooldownForDynamicRetaliationTut = 100f;


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


	private void StartTestingForDynamicOccurences() {
		testing = StartCoroutine(Testing());
	}

	private void ForceStopTestingForDynamicOccurences() {
		if(testing!=null)
			StopCoroutine(testing);
	}

	IEnumerator Testing() {
		Debug.Log("Startedtesting");
		while(inCombat){
			yield return new WaitForSeconds(3f);
			ShowDynamicTypes();
		}
		Debug.Log("Stopped testing");
	}

	IEnumerator TypesCooldown() {
		yield return new WaitForSeconds(cooldownForDynamicTypeTut);
		shownType = false;
	}
	IEnumerator RetaliationCooldown() {
		yield return new WaitForSeconds(cooldownForDynamicRetaliationTut);
		shownRetaliation = false;
	}


	public void ShowDynamicTypes() {
		if(shownType)
			return;

		var playerTrades = GetPlayerTrades();
		var aiTrades = GetAITrades();
		if(playerTrades.Count == 0 || aiTrades.Count == 0)
			return;

		StartCoroutine(TypesCooldown());

		FilterTowerAttacks(ref playerTrades);
		FilterTowerAttacks(ref aiTrades);

		var score = playerTrades.FindAll(element => element.good).Count - playerTrades.FindAll(element => element.bad).Count;
		var aiScore =  aiTrades.FindAll(element => element.good).Count - aiTrades.FindAll(element => element.bad).Count;

		//TODO: Understand which type of bad attack triggered this and use that, or use general knowledge?
		var atk = playerTrades.Find(element => element.bad).attacker;
		var def = playerTrades.Find(element => element.bad).defender;

		if(score < -2 && aiScore > 1){
			Debug.Log("SHOW THE ELEMENTALTYPE STUFF HERE! LOOK AT TODO IN THIS FUNCTION");
			GeneralConfirmationBox.instance.ShowPopUp(
				String.Format("Type {0} is weak against {1} \n Maybe you should try using a different type of unit", atk, def),"Okay");
		}
	}

	public void ShowRetaliationDynamic() {
		if(shownRetaliation)
			return;

		var playerTrades = GetPlayerTrades();
		var aiTrades = GetAITrades();
		if(playerTrades.Count == 0 || aiTrades.Count == 0)
			return;

		StartCoroutine(RetaliationCooldown());

		FilterTowerAttacks(ref playerTrades);
		FilterTowerAttacks(ref aiTrades);

		var score = (playerTrades.Count - playerTrades.FindAll(element => element.killHit).Count) - (aiTrades.Count - aiTrades.FindAll(element => element.killHit).Count);

		if (score < 0) {
			Debug.Log("THE PLAYER NEEDS TO BE TAUGHT ABOUT RETALIATION");
			GeneralConfirmationBox.instance.ShowPopUp("If a unit survives an attack\n It will retaliate","Okay");
		}
	}

	private void FilterTowerAttacks(ref List<CombatTrades> trades) {
		trades.RemoveAll(element => element.towerHit);
	}

	private List<CombatTrades> GetPlayerTrades() {
		return DataGathering.Instance.GetAllTrades().FindAll(element => element.initiator == Team.Player);
	}
	private List<CombatTrades> GetAITrades() {
		return DataGathering.Instance.GetAllTrades().FindAll(element => element.initiator == Team.AI);
	}

	public void SetCombat(bool value) {
		inCombat = value;
		if (inCombat)
			StartTestingForDynamicOccurences();
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
