using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;

public class DynamicTut : MonoBehaviour {

	public bool isDynamic;
	private string textToShow = "", buttonOne = "Okay";
	public float moveDelay = 3f, endTurnDelay = 4f, playerSelectionDelay = 4f;
	//TODO: Add all the different types of tuts we want to be shown in here
	public ShowGeneralInformationBox tutMove;

	void Start () {
		if(isDynamic){
			Debug.LogWarning("Tutorial is dynamic!");
		}
		else{
			Debug.LogWarning("Tutorial is NOT dynamic!");
		}
		tutMove.dynamicTut = isDynamic;
		ShowGoalAndSummon();
	}


	void Update () {
	}

	void ShowGoalAndSummon() {
		textToShow = "Your goal is to destroy the enemy towers.\n" +
		             "Click a unit and then a square summon pad to summon. This costs 'summon points', displayed next to the units name";
		StartCoroutine(TimerForPlayerMove(textToShow, buttonOne));
	}

	void ShowSummonSicknessAndTurn() {

	}

	void ShowTurnInteractionAndControlForUnitMovement() {

	}


	void PlayerMovement() {

	}

	void PlayerMovingUnit() {
	}

	void PlayerAttackingUnit() {
	}

	void PlayerWinning() {
		//TODO: TutorialHandler refactor, to actually be the one that shows the right thing
	}

	void PlayerLosing() {
	}

	IEnumerator TimeForPlayerSelectingUnit() {
		yield return new WaitForSeconds(endTurnDelay);
		Debug.Log("See if player selects a unit");

	}


	IEnumerator TimeForPlayerEndTurn() {
		yield return new WaitForSeconds(endTurnDelay);
		Debug.Log("Check for summon points left and/or summon spots filled");

	}

	IEnumerator TimerForPlayerMove(string text, string button) {
		yield return new WaitForSeconds(moveDelay);
		Debug.Log("Needs Datagathering HasSummoned a unit");
		/*if (DataGathering.Instance.HasMoved) {
			yield break;
		}*/
		GeneralConfirmationBox.instance.ShowPopUp(text, button);
	}
}
