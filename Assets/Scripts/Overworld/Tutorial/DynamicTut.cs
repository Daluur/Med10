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

	private bool inCombat, shownType, shownRetaliation, shownShadow;
	private Coroutine testing;
	public float cooldownForDynamicTypeTut = 100f, cooldownForDynamicRetaliationTut = 100f, cooldownForShadowSpecialTut = 100f;


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
			ShowDynamicShadowSpecial();
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

	IEnumerator ShadowSpecialCooldown() {
		yield return new WaitForSeconds(cooldownForShadowSpecialTut);
		shownShadow = false;
	}

	public void ShowDynamicShadowSpecial() {
		if(!PlayerData.Instance.GetHasEverSummonedShadow())
			return;
		if(shownShadow)
			return;
		shownShadow = true;
		if (PlayerData.Instance.GetMovedShadowWithoutMovingThroughUnit() > 2) {

		}
		PlayerData.Instance.ResetTrades();
		StartCoroutine(ShadowSpecialCooldown());
		TutorialHandler.instance.ShadowUnitDyn();
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

		var score = playerTrades.FindAll(element => element.good && !element.retaliation).Count - playerTrades.FindAll(element => element.bad && !element.retaliation && !element.killHit).Count;
		var aiScore =  aiTrades.FindAll(element => element.good && !element.retaliation).Count - aiTrades.FindAll(element => element.bad && !element.retaliation && !element.killHit).Count;

		//TODO: Understand which type of bad attack triggered this and use that, or use general knowledge?
		var atk = playerTrades.Find(element => element.bad).attacker;
		var def = playerTrades.Find(element => element.bad).defender;

		if(score < -2 && aiScore > 1){
			TutorialHandler.instance.TypeTUTDyn();
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

		var score = (playerTrades.FindAll(element => element.retaliation).Count) - (aiTrades.FindAll(element => element.retaliation).Count);

		if (score < 0) {
			//Debug.Log("THE PLAYER NEEDS TO BE TAUGHT ABOUT RETALIATION");
			GeneralConfirmationBox.instance.ShowPopUp("If a unit survives an attack\n It will retaliate","Okay");
		}
	}

	private void FilterTowerAttacks(ref List<CombatTrades> trades) {
		trades.RemoveAll(element => element.towerHit);
	}

	private List<CombatTrades> GetPlayerTrades() {
		return PlayerData.Instance.GetAllTrades().FindAll(element => element.initiator == Team.Player);
	}
	private List<CombatTrades> GetAITrades() {
		return PlayerData.Instance.GetAllTrades().FindAll(element => element.initiator == Team.AI);
	}

	public void SetCombat(bool value) {
		inCombat = value;
		if (inCombat)
			StartTestingForDynamicOccurences();
	}


	private void ContinueTheDynamicTutCycle() {
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
		//summonSickness = true;
		//StartCoroutine(TimerForPlayerMove());
	}

	void ShowSummonSicknessAndTurn() {

	}

	void ShowTurnInteractionAndControlForUnitMovement() {

	}

	void PlayerSelection() {
		//StartCoroutine(TimeForPlayerSelectingUnit());
	}

	void PlayerMovement() {

	}

	void PlayerMovingUnit() {
	}

	void PlayerAttackingUnit() {
	}

	void PlayerEndTurn() {
		//StartCoroutine(TimeForPlayerEndTurn());
	}

	void PlayerWinning() {
	}

	void PlayerLosing() {
	}

/*	IEnumerator TimeForPlayerSelectingUnit() {
		yield return new WaitForSeconds(playerSelectionDelay);
		if(PlayerData.Instance.HasEverHadSelectedUnit())
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
		if (DataGathering.Instance.HasMoved) {
			yield break;
		}
		TutorialHandler.instance.ShowGoalAndSummon();
	}*/
}
