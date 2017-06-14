using System.Collections;
using System.Collections.Generic;
using Overworld;
using SimplDynTut;
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
		GeneralConfirmationBox.instance.ShowPopUp("Your goal is to destroy the enemy towers before they destroy yours.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("Click a unit and then one of your square tiles to summon it.\nThis costs 'summon points', which are displayed next to the units name.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("Your available summon points are displayed at the top left of your screen.", "CLOSE")));
	}

	public void LosingCombat() {
		GeneralConfirmationBox.instance.ShowPopUp("Losing a battle will send you back to your last checkpoint.", "CLOSE");
	}

	public void StartingTurnSecondTurn() {
		GeneralConfirmationBox.instance.ShowPopUp("You will gain 2 summon points at the start of your turn, 1 point for killing an enemy unit, and 3 points for killing an enemy tower!", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("You can hover over both your own and enemy units to see their stats.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("You can drag with the left mouse button on screen to move the camera around the field.\nClick on a summoned unit to move it.", "CLOSE")));
	}

	public void StartingThirdTurn() {
		GeneralConfirmationBox.instance.ShowPopUp("Units can attack enemy units when they are on spots connected by a path.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("If a unit is still alive after it has been attacked it will retaliate.\nAttacking ends the units turn.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("The yellow number above your unit is the damage it deals. The red line is the health bar, which indicates how much damage the unit can take.", "CLOSE")));
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
		GeneralConfirmationBox.instance.ShowPopUp ("The 4 glowing slots are your combat slots, you will bring these units to battle.", "NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("You can drag units to different slots to change your lineup.", "CLOSE"));
	}

	public void FirstShop() {
		GeneralConfirmationBox.instance.ShowPopUp ("This is the summon shop.\n\nClick on a unit icon to add it to your inventory. Only units that you have the recipe for can be added.", "CLOSE");
	}

	public void FirstBuy() {
		GeneralConfirmationBox.instance.ShowPopUp("You are able to carry 12 units at a time.", "NEXT", () => GeneralConfirmationBox.instance.PartOfMultiPage("Close the inventory and shop by clicking the X in the top right corner, or by pressing ESC to close all windows.", "CLOSE"));
	}

	public void ShadowUnitDyn() {
		GeneralConfirmationBox.instance.ShowPopUp("SHADOW units can move through other units", "CLOSE");
	}

	public void TypeTUTDyn() {
		GeneralConfirmationBox.instance.ShowPopUp("Units have different types, these can be strong or weak to another type.", "NEXT",
			() => GeneralConfirmationBox.instance.ShowPopUp("Attacking with a unit, whose type is strong against the opponent’s type will deal double damage.", "NEXT",
			() => GeneralConfirmationBox.instance.ShowPopUp("Attacking with a unit, whose type is weak against the opponent’s type will deal half damage.", "NEXT",
			() => GeneralConfirmationBox.instance.ShowPopUp("WATER is strong against FIRE.\n\nFIRE is strong against NATURE.", "NEXT",
			() => GeneralConfirmationBox.instance.ShowPopUp("NATURE is strong against LIGHTNING.\n\nLIGHTNING is strong against WATER.", "CLOSE")))));
	}

	public void ShowBothShadowAndTypesDynTUT() {
		GeneralConfirmationBox.instance.ShowPopUp(
			"Some types are stronger against others\nMaybe you should consider using a different type of unit",
			"NEXT", () => GeneralConfirmationBox.instance.ShowPopUp("SHADOW units can move through other units", "CLOSE"));
	}


	public void EndTurn(bool isDynamicCallIn) {
		if(isDynamic == isDynamicCallIn)
			GeneralConfirmationBox.instance.ShowPopUp("NEED TEXT, END TURN FOR PLAYER DYNAMIC TUT", "Okay");
	}

	public void SelectUnits(bool isDynamicCallIn) {
		if(isDynamic == isDynamicCallIn)
			GeneralConfirmationBox.instance.ShowPopUp("NEED TEXT FOR SELECTING UNIT TUT","Okay");
	}

	public void WorldTrigger(int id) {
		if(id == 1) { //move
			GeneralConfirmationBox.instance.ShowPopUp("Click on the ground to move your character", "CLOSE");
		}
		if(id == 2) { //shop
			GeneralConfirmationBox.instance.ShowPopUp("Click on the summon tower to open the unit shop", "CLOSE");
		}
		if(id == 3) { //encounter
			GeneralConfirmationBox.instance.ShowPopUp("Click on an enemy to start a battle", "NEXT", 
				() => GeneralConfirmationBox.instance.ShowPopUp("You can hover the mouse over an enemy to see information about the enemy.", "CLOSE"));
		}
		if(id == 4) { //inventory
			GeneralConfirmationBox.instance.ShowPopUp("Press the 'I' key to open your inventory", "CLOSE");
		}
		if (id == 5) { //types
			if (!isDynamic) {
				GeneralConfirmationBox.instance.ShowPopUp("Units have different types, these can be strong or weak to another type.", "NEXT",
					() => GeneralConfirmationBox.instance.ShowPopUp("Attacking with a unit, whose type is strong against the opponent’s type will deal double damage.", "NEXT",
					() => GeneralConfirmationBox.instance.ShowPopUp("Attacking with a unit, whose type is weak against the opponent’s type will deal half damage.", "CLOSE")));
			}
		}
		if (id == 6) { //types2
			if (!isDynamic) {
				GeneralConfirmationBox.instance.ShowPopUp("WATER is strong against FIRE.", "NEXT",
					() => GeneralConfirmationBox.instance.ShowPopUp("FIRE is strong against NATURE.", "NEXT",
					() => GeneralConfirmationBox.instance.ShowPopUp("NATURE is strong against LIGHTNING.", "NEXT",
					() => GeneralConfirmationBox.instance.ShowPopUp("LIGHTNING is strong against WATER.", "CLOSE"))));
			}
		}
		if (id == 7) { //shadow
			if (isDynamic) {
				GeneralConfirmationBox.instance.ShowPopUp("Some units have special abilities, which they can use in combat.", "NEXT",
				() => GeneralConfirmationBox.instance.ShowPopUp("STONE units can hunker down in place, after which they will be unable to move, or initiate attacks, but will receive less damage and deal less damage.", "CLOSE"));
			}
			else {
				GeneralConfirmationBox.instance.ShowPopUp("Some units have special abilities, which they can use in combat.", "NEXT",
				() => GeneralConfirmationBox.instance.ShowPopUp("SHADOW units can move through other units", "NEXT",
				() => GeneralConfirmationBox.instance.ShowPopUp("STONE units can hunker down in place, after which they will be unable to move, or initiate attacks, but will receive less damage and deal less damage.", "CLOSE")));
			}
		}
		if(id == 8) { //Concede
			GeneralConfirmationBox.instance.ShowPopUp("If you want to leave a combat, there is a concede option in the escape menu.", "CLOSE");
		}
	}

	public void NewWorldTrigger(OWTUTTRIGGERS trigger) {
		switch (trigger) {
			case OWTUTTRIGGERS.OpenInventory:
				OverworldTriggers.instance.ShowInventoryOpenInformation();
				break;
			case OWTUTTRIGGERS.EnterShop:
				OverworldTriggers.instance.ShowGoToShopTutInfo();
				break;
			case OWTUTTRIGGERS.EnterCombat:
				OverworldTriggers.instance.StartEnterCombatTimer();
				break;
			case OWTUTTRIGGERS.types:
				GeneralConfirmationBox.instance.ShowPopUp("Units have different types, these can be strong or weak to another type.", "NEXT",
					() => GeneralConfirmationBox.instance.PartOfMultiPage("Attacking with a unit, whose type is strong against the opponent’s type will deal double damage.", "NEXT",
					() => GeneralConfirmationBox.instance.PartOfMultiPage("Attacking with a unit, whose type is weak against the opponent’s type will deal half damage.", "CLOSE")));
				break;
			case OWTUTTRIGGERS.concede:
				GeneralConfirmationBox.instance.ShowPopUp("If you want to leave a combat, there is a concede option in the escape menu.", "CLOSE");
				break;
			case OWTUTTRIGGERS.shadow:
				GeneralConfirmationBox.instance.ShowPopUp("Some units have special abilities, which they can use in combat.", "NEXT",
				() => GeneralConfirmationBox.instance.PartOfMultiPage("SHADOW units can move through other units", "NEXT",
				() => GeneralConfirmationBox.instance.PartOfMultiPage("STONE units can hunker down in place, after which they will be unable to move, or initiate attacks, but will receive less damage and deal less damage.", "CLOSE")));
				break;
			default:
				break;
		}
	}
}
