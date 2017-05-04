using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;

public class TutorialHandler : Singleton<TutorialHandler> {
	[HideInInspector]
	public bool unitFirst = true;
	[HideInInspector]
	public bool combatFirstTurn = true;
	[HideInInspector]
	public bool combatSecondTurn = false;
	[HideInInspector]
	public bool combatThirdTurn = false;
	[HideInInspector]
	public bool summonFirst = true;
	[HideInInspector]
	public bool firstWin = true;
	[HideInInspector]
	public bool firstLoss = true;
	[HideInInspector]
	public bool firstInventory = true;
	[HideInInspector]
	public bool firstShop = true;
	[HideInInspector]
	public bool firstBuy = true;

	public bool isDynamic;

	public void ShowGoalAndSummon() {
		GeneralConfirmationBox.instance.ShowPopUp("Your goal is to destroy the enemy towers before they destroy yours.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("Click a unit and then a square summon pad to summon.\nThis costs 'summon points', which are displayed next to the units name.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("Your available summon points are displayed at the top of the summon menu.", "CLOSE")));
	}

	public void LosingCombat() {
		GeneralConfirmationBox.instance.ShowPopUp("Losing a battle will send you back to your last checkpoint.", "CLOSE");
	}

	public void StartingTurnSecondTurn() {
		GeneralConfirmationBox.instance.ShowPopUp("You will gain 2 summon points at the start of your turn each round, or 1 point for killing an enemy unit.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("You can hover over both your own and enemy units to see their stats.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("You can drag with the left mouse button on screen to move the camera around the field.\nClick on a summoned unit to move it.", "CLOSE")));
	}

	public void StartingThirdTurn() {
		GeneralConfirmationBox.instance.ShowPopUp("Units can attack each other when they are on connected spots.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("If a unit is still alive after it has been attacked it will retaliate.\nAttacking ends the units turn.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("The yellow number above your unit is the damage it deals. The red line is the health bar, which indicates how much damage the unit can take.", "CLOSE")));
	}

	public void Winning() {
		GeneralConfirmationBox.instance.ShowPopUp("Winning a battle will grant new summon recipes", "CLOSE");
	}

	public void FirstSummon() {
		GeneralConfirmationBox.instance.ShowPopUp("Units cannot make other moves the round they are summoned.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("When you have performed all your moves, end your turn by pressing 'SPACE' or clicking the end turn button, after which the opponent will take theirs.", "CLOSE"));
	}

	public void FirstSelection() {
		GeneralConfirmationBox.instance.ShowPopUp("Click on a green spot to move to it.\nYou can only move once with a unit per turn, it can however attack if it moves to a spot with an enemy unit next to it.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("To change your view of the battlefield use the scroll wheel on the mouse to zoom in or out.", "CLOSE"));
	}

	public void PortalIsOpen() {
		GeneralConfirmationBox.instance.ShowPopUp("The portal to the next island is now open.", "CLOSE");
	}

	public void InventoryAndShop() {
		GeneralConfirmationBox.instance.ShowPopUp ("The 6 glowing slots are your combat slots, you will bring these units to battle.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("You can drag units to different slots to change your lineup.", "CLOSE"));
	}

	public void FirstShop() {
		GeneralConfirmationBox.instance.ShowPopUp ("This is the summon shop.\n\nClick on a unit icon to summon it to your inventory. Only units that you have the recipe for can be summoned.", "CLOSE");
	}

	public void FirstBuy() {
		GeneralConfirmationBox.instance.ShowPopUp("You are able to carry 12 units at a time.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("Close the inventory and shop by clicking the X in the top right corner, or by pressing ESC to close all windows.", "CLOSE"));
	}

	public void ShadowUnitDyn() {
		GeneralConfirmationBox.instance.ShowPopUp("SHADOW units can move through other units", "CLOSE");
	}

	public void TypeTUTDyn() {
		GeneralConfirmationBox.instance.ShowPopUp(
			("Remember that some types are stronger against other\nMaybe you should consider using a different type of unit"),"CLOSE");
	}


	public void EndTurn(bool isDynamicCallIn) {
		if(isDynamic == isDynamicCallIn)
			GeneralConfirmationBox.instance.ShowPopUp("NEED TEXT, END TURN FOR PLAYER DYNAMIC TUT", "Okay");
	}

	public void SelectUnits(bool isDynamicCallIn) {
		if(isDynamic == isDynamicCallIn)
			GeneralConfirmationBox.instance.ShowPopUp("NEED TEXT FOR SELECTING UNIT TUT","Okay");
	}


}
