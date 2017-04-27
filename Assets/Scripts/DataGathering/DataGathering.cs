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
	public bool HasMoved { get { return hasMoved; } private set { hasMoved = value; } }

	#endregion

	#region CW things

	List<CombatTrades> AllTrades = new List<CombatTrades>();
	List<CombatTrades> TradesFromLastCombat = new List<CombatTrades>();

	public void AddCombatTrade(CombatTrades newTrade) {
		TradesFromLastCombat.Add(newTrade);
		AllTrades.Add(newTrade);
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