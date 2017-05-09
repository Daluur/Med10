using System;
using System.Collections;
using System.Collections.Generic;
using CombatWorld;
using CombatWorld.Utility;
using Overworld;
using UnityEngine;


public enum LearningObjectives {
	None,
	ShadowUnit,
	TypeInteraction,
	Both
}

public class DynamicTut : Singleton<DynamicTut> {

	[Header("If both is true, it will be dynamic. Make sure BOTH is false, when building!")]
	public bool dynamicTestOverride;
	public bool staticTestOverride;

	[HideInInspector]
	public bool isDynamic;
	
	private string textToShow = "", buttonOne = "Okay";
	public float moveDelay = 3f, endTurnDelay = 4f, playerSelectionDelay = 4f;

	private bool showGoalAndSummon, summonSickness, playerMovement, winning, losing, playerAttacking, endTurn;

	private bool inCombat, shownType, shownRetaliation, shownShadow;
	private Coroutine testing;
	public float cooldownForDynamicTypeTut = 100f, cooldownForDynamicRetaliationTut = 100f, cooldownForShadowSpecialTut = 100f;


	public LearningObjectives island1, island2, island3, island4, island5;


	//TODO: Add all the different types of tuts we want to be shown in here

	void Start () {
		if (staticTestOverride || dynamicTestOverride) {
			if (staticTestOverride) {
				isDynamic = false;
			}
			if (dynamicTestOverride) {
				isDynamic = true;
			}
		}
		else {
			isDynamic = !DataGathering.Instance.Static;
		}
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
			CheckDynamicShadowSpecial();
			CheckDynamicTypes();
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

	public void CheckForDynamicTuts() {
		var dynamicTut = CheckDynamicTypes();
		var shadowSpecial = CheckDynamicShadowSpecial();
		if(dynamicTut && shadowSpecial){
			DataGathering.Instance.AddCombatTrade(new CombatTrades() { initiator = Team.NONE, attacker = ElementalTypes.Fire });
			DataGathering.Instance.WasToldAboutShadow();
			DataGathering.Instance.WasToldAboutTypes();
			if (isDynamic) {
				TutorialHandler.instance.ShowBothShadowAndTypesDynTUT();
			}
		}
		else if (shadowSpecial) {
			DataGathering.Instance.AddCombatTrade(new CombatTrades() { initiator = Team.NONE, attacker = ElementalTypes.Water });
			DataGathering.Instance.WasToldAboutShadow();
			if (isDynamic) {
				TutorialHandler.instance.ShadowUnitDyn();
			}
		}
		else if (dynamicTut) {
			DataGathering.Instance.AddCombatTrade(new CombatTrades() { initiator = Team.NONE, attacker = ElementalTypes.Nature });
			DataGathering.Instance.WasToldAboutTypes();
			if (isDynamic) {
				TutorialHandler.instance.TypeTUTDyn();
			}
		}
	}

	public bool HasLearnedEverything(int island) {
		/*if (!isDynamic) {
			return true;
		}*/
		switch (island) {
			case 0:
				return CheckLearningGoals(island1);
			case 1:
				return CheckLearningGoals(island2);
			case 2:
				return CheckLearningGoals(island3);
			case 3:
				return CheckLearningGoals(island4);
			case 4:
				return CheckLearningGoals(island5);
		}
		return true;
	}

	private bool CheckLearningGoals(LearningObjectives island) {
		bool haslearned = false;
		if (island == LearningObjectives.Both) {
			haslearned = CheckLearningObjectiveDynamicTutShadow() && CheckLearningObjectiveDynamicTutType();
			if(haslearned == false) {
				DataGathering.Instance.HadNotLearnedShadow();
				DataGathering.Instance.HadNotLearnedTypes();
			}
			DataGathering.Instance.AddCombatTrade(new CombatTrades() { initiator = Team.NONE, defender = (ElementalTypes)island, good = haslearned, bad = true });
			return haslearned;
		}
		if (island == LearningObjectives.ShadowUnit) {
			haslearned = CheckLearningObjectiveDynamicTutShadow();
			if (haslearned == false) {
				DataGathering.Instance.HadNotLearnedShadow();
			}
			DataGathering.Instance.AddCombatTrade(new CombatTrades() { initiator = Team.NONE, defender = (ElementalTypes)island, good = haslearned, bad = true });
			return haslearned;
		}
		if (island == LearningObjectives.TypeInteraction) {
			haslearned = CheckLearningObjectiveDynamicTutType();
			if (haslearned == false) {
				DataGathering.Instance.HadNotLearnedTypes();
			}
			DataGathering.Instance.AddCombatTrade(new CombatTrades() { initiator = Team.NONE, defender = (ElementalTypes)island, good = haslearned, bad = true });
			return haslearned;
		}
		return true;
	}


	private bool CheckLearningObjectiveDynamicTutType() {
		var playerTrades = PlayerData.Instance.GetTradesFromLastCombat().FindAll(element => element.initiator == Team.Player);
		var aiTrades = PlayerData.Instance.GetTradesFromLastCombat().FindAll(element => element.initiator == Team.AI);
		if(playerTrades.Count == 0 || aiTrades.Count == 0)
			return true;

		FilterTowerAttacks(ref playerTrades);
		FilterTowerAttacks(ref aiTrades);

		var score = playerTrades.FindAll(element => element.good && !element.retaliation).Count - playerTrades.FindAll(element => element.bad && !element.retaliation && !element.killHit).Count;
		var aiScore =  aiTrades.FindAll(element => element.good && !element.retaliation).Count - aiTrades.FindAll(element => element.bad && !element.retaliation && !element.killHit).Count;
		//Debug.Log(score);

		//TODO: Understand which type of bad attack triggered this and use that, or use general knowledge?

		if(score < -2 || score - aiScore < -2) {
			return false;
		}
		return true;
	}

	private bool CheckLearningObjectiveDynamicTutShadow() {
		if(PlayerData.Instance.GetHasEverSummonedShadow() == false) {
			return false;
		}
		if (PlayerData.Instance.GetMovedShadowWithoutMovingThroughUnitLastCombat() - PlayerData.Instance.GetShadowSummonedLastCombat() > 2 || PlayerData.Instance.GetTradesFromLastCombat().FindAll(element => element.shadow && element.movedThroughUnit && element.towerHit).Count > 0) {
			StartCoroutine(ShadowSpecialCooldown());
			return false;
		}
		return true;
	}


	private bool CheckDynamicShadowSpecial() {
		if(!PlayerData.Instance.GetHasEverSummonedShadow())
			return false;
		if(shownShadow)
			return false;
		var score = PlayerData.Instance.GetMovedShadowWithoutMovingThroughUnit() - PlayerData.Instance.GetPlayerHasUsedShadowCount() -
		            PlayerData.Instance.GetShadowSummonedLastCombat();
		if (score > 2) {
			shownShadow = true;
			PlayerData.Instance.ResetShadow();
			StartCoroutine(ShadowSpecialCooldown());
			return true;
		}
		return false;
	}


	private bool CheckDynamicTypes() {
		if(shownType)
			return false;

		var playerTrades = GetPlayerTrades();
		var aiTrades = GetAITrades();
		if(playerTrades.Count == 0 || aiTrades.Count == 0)
			return false;

		FilterTowerAttacks(ref playerTrades);
		FilterTowerAttacks(ref aiTrades);

		var score = playerTrades.FindAll(element => element.good && !element.retaliation).Count - playerTrades.FindAll(element => element.bad && !element.retaliation && !element.killHit).Count;
		var aiScore =  aiTrades.FindAll(element => element.good && !element.retaliation).Count - aiTrades.FindAll(element => element.bad && !element.retaliation && !element.killHit).Count;

		//TODO: Understand which type of bad attack triggered this and use that, or use general knowledge?
		//Debug.Log(score);
		if(score < -2 || score - aiScore < -2) {
			shownType = true;
			PlayerData.Instance.ResetTrades();
			StartCoroutine(TypesCooldown());
			return true;
		}
		return false;
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
