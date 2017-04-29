using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Units;
using CombatWorld.Utility;

public class DataGathering {

	#region Singleton Things

	private static DataGathering instance;

	public static DataGathering Instance {
		get {
			if (instance == null) {
				instance = new DataGathering();
			}
			return instance;
		}
	}

	#endregion

	#region OW things

	//Movement
	private bool hasMoved;
	public bool HasMoved { get { return hasMoved; } set { hasMoved = value; Debug.Log("moved"); } }

	#endregion

	#region CW things

	List<CombatTrades> AllTrades = new List<CombatTrades>();
	List<CombatTrades> TradesFromLastCombat = new List<CombatTrades>();

	public void AddCombatTrade(CombatTrades newTrade) {
		TradesFromLastCombat.Add(newTrade);
		AllTrades.Add(newTrade);
	}

	int turnedToStoneCount = 0;

	public void TurnedUnitToStone() {
		Debug.Log("turned to stone");
		turnedToStoneCount++;
	}

	int movedShadowUnitThroughEnemyUnitCount = 0;
	int movedShadowUnitThroughFriendlyUnitCount = 0;

	public void MovedShadowThroughEnemyUnit() {
		Debug.Log("Moved through enemy");
		movedShadowUnitThroughEnemyUnitCount++;
	}

	public void MovedShadowThrougFriendlyUnit() {
		Debug.Log("Moved through friendly");
		movedShadowUnitThroughFriendlyUnitCount++;
	}

	bool hasAttackedTower = false;
	bool hasKilledATower = false;
	int stoodBesideTowerNotAttackingcount = 0;

	public void AttackedTower() {
		Debug.Log("Attacked tower");
		hasAttackedTower = true;
	}

	public void KilledTower() {
		Debug.Log("Killed a tower");
		hasKilledATower = true;
	}

	public void StoodBesideTowerAndDidNotAttack() {
		Debug.Log("did not attack tower");
		stoodBesideTowerNotAttackingcount++;
	}

	#endregion
}


public class CombatTrades {
	public ElementalTypes attacker;
	public ElementalTypes defender;
	public bool good;
	public bool bad;
	public Team initiator;
	public bool towerHit = false;
	public bool killHit;
}