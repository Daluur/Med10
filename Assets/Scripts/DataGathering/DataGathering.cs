﻿using System.Collections;
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

	#region Generel stuff

	int amountOfCombats = 0;

	public void StartNewCombat() {
		amountOfCombats++;
		TradesFromLastCombat.Clear();
		SummonedUnitsLastCombat.Clear();
	}

	#endregion

	#region Trades

	List<CombatTrades> AllTrades = new List<CombatTrades>();
	List<CombatTrades> TradesFromLastCombat = new List<CombatTrades>();

	public void AddCombatTrade(CombatTrades newTrade) {
		TradesFromLastCombat.Add(newTrade);
		AllTrades.Add(newTrade);
	}

	public List<CombatTrades> GetTradesFromLastCombat() {
		return TradesFromLastCombat;
	}

	public List<CombatTrades> GetAllTrades() {
		return AllTrades;
	}

	#endregion

	#region Summons

	List<SummonPlayerData> AllSummonedUnits = new List<SummonPlayerData>();
	List<SummonPlayerData> SummonedUnitsLastCombat = new List<SummonPlayerData>();

	public void SummonedNewUnit(SummonPlayerData type) {
		AllSummonedUnits.Add(type);
		SummonedUnitsLastCombat.Add(type);
	}

	public List<SummonPlayerData> GetSummonedLastCombat() {
		return SummonedUnitsLastCombat;
	}

	public List<SummonPlayerData> GetAllSummonedUnits() {
		return AllSummonedUnits;
	}

	public bool HasEverSummonType(ElementalTypes type) {
		foreach (SummonPlayerData data in AllSummonedUnits) {
			if(data.type == type) {
				return true;
			}
		}
		return false;
	}

	public bool HasSummonedTypeLastCombat(ElementalTypes type) {
		foreach (SummonPlayerData data in SummonedUnitsLastCombat) {
			if (data.type == type) {
				return true;
			}
		}
		return false;
	}

	public bool HasEverSummonedSpecial(bool stone, bool shadow) {
		foreach (SummonPlayerData data in AllSummonedUnits) {
			if (stone && data.stone) {
				return true;
			}
			if (shadow && data.shadow) {
				return true;
			}
		}
		return false;
	}

	public bool HasSummonedSpecialLastCombat(bool stone, bool shadow) {
		foreach (SummonPlayerData data in SummonedUnitsLastCombat) {
			if(stone && data.stone) {
				return true;
			}
			if(shadow && data.shadow) {
				return true;
			}
		}
		return false;
	}

	#endregion

	#region stone

	public int turnedToStoneCount = 0;

	public void TurnedUnitToStone() {
		Debug.Log("turned to stone");
		turnedToStoneCount++;
	}

	#endregion

	#region Shadow

	public int movedShadowUnitThroughEnemyUnitCount = 0;
	public int movedShadowUnitThroughFriendlyUnitCount = 0;

	public void MovedShadowThroughEnemyUnit() {
		Debug.Log("Moved through enemy");
		movedShadowUnitThroughEnemyUnitCount++;
	}

	public void MovedShadowThrougFriendlyUnit() {
		Debug.Log("Moved through friendly");
		movedShadowUnitThroughFriendlyUnitCount++;
	}

	#endregion

	#region Tower

	public bool hasAttackedTower = false;
	public bool hasKilledATower = false;
	public int stoodBesideTowerNotAttackingcount = 0;

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

	#region Decks

	List<int> UnitsBroughtToLastCombat = new List<int>();

	public void UnitsBroughtToCombat(List<int> units) {
		UnitsBroughtToLastCombat = units;
	}

	#endregion

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

public class SummonPlayerData {
	public ElementalTypes type;
	public bool stone;
	public bool shadow;
}