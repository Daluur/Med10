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


	public void ShowGoalAndSummon() {
		GeneralConfirmationBox.instance.ShowPopUp("Your goal is to destroy the enemy towers.\n" +
		                                          "Click a unit and then a square summon pad to summon. This costs 'summon points', displayed next to the units name", "Okay");
	}

	public void LosingCombat() {
		GeneralConfirmationBox.instance.ShowPopUp("Losing a battle will send you back to your last checkpoint.", "Okay");
	}

	public void StartingTurnSecondTurn() {
		GeneralConfirmationBox.instance.ShowPopUp("You will gain 2 summon points at the start of your turn each round.\nClick on a summoned unit to move it.", "Okay");
	}

	public void StartingThirdTurn() {
		GeneralConfirmationBox.instance.ShowPopUp("Attacking ends the units turn.\n" +
		                                          "The yellow number above your unit is the damage it deals. The red line is the health bar, which indicates how much damage the unit can take.", "Okay");
	}

	public void Winning() {
		GeneralConfirmationBox.instance.ShowPopUp("Winning a battle will grant you gold and new summon recipes", "Okay");
	}

	public void FirstSummon() {
		GeneralConfirmationBox.instance.ShowPopUp("Units cannot make other moves the round they are summoned.\nWhen you have performed all your moves, end your turn, after which the opponent will take theirs.", "Okay");
	}

	public void FirstSelection() {
		GeneralConfirmationBox.instance.ShowPopUp("Click on a green spot to move to it.\n" +
		                                          "You can only move once with a unit per turn, it can however attack if it moves to a spot with an enemy unit next to it.", "Okay");
	}

	public void PortalIsOpen() {
		GeneralConfirmationBox.instance.ShowPopUp("The portal to the next island is now open.", "Okay");
	}

	public void InventoryAndShop() {
		GeneralConfirmationBox.instance.ShowPopUp ("You can carry 12 units at a time.\nThe 6 glowing slots are your combat slots, you will bring these units to battle.\nDrag units to different slots to change lineup.", "Okay");
	}

	public void FirstShop() {
		GeneralConfirmationBox.instance.ShowPopUp ("Spending your crystals here summons a unit to your inventory. Only units that you have the recipe for can be summoned.", "Okay");
	}

	public void FirstBuy() {
		GeneralConfirmationBox.instance.ShowPopUp("test1", "next", () => GeneralConfirmationBox.instance.ShowPopUp("test2", "Next", () => GeneralConfirmationBox.instance.ShowPopUp("test3", "Close")));
	}


}
