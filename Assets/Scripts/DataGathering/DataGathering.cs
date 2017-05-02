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

	#region Generel stuff

	int amountOfCombats = 0;

	public void StartNewCombat() {
		amountOfCombats++;
		TradesFromLastCombat.Clear();
		SummonedUnitsLastCombat.Clear();
		SSS.Clear();
	}

	#endregion

	#region UnitSelection

	bool hasEverSelectedUnit = false;
	Unit currentlySelectedUnit;

	public void SelectedUnit(Unit unit) {
		hasEverSelectedUnit = true;
		currentlySelectedUnit = unit;
	}

	public bool HasEverHadSelectedUnit() {
		return hasEverSelectedUnit;
	}

	public void DeselectUnit() {
		currentlySelectedUnit = null;
	}

	public bool HasUnitSelected() {
		return currentlySelectedUnit != null;
	}

	public Unit GetCurrentlySelectedUnit() {
		return currentlySelectedUnit;
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

	public int GetNumberOfPlayerGoodTradesTotal() {
		int count = 0;
		foreach (CombatTrades trade in AllTrades) {
			if (trade.initiator == Team.Player && trade.good) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfPlayerBadTradesTotal() {
		int count = 0;
		foreach (CombatTrades trade in AllTrades) {
			if (trade.initiator == Team.Player && trade.bad) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfAIGoodTradesTotal() {
		int count = 0;
		foreach (CombatTrades trade in AllTrades) {
			if (trade.initiator == Team.AI && trade.good) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfAIBadTradesTotal() {
		int count = 0;
		foreach (CombatTrades trade in AllTrades) {
			if (trade.initiator == Team.AI && trade.bad) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfPlayerGoodTradesLastCombat() {
		int count = 0;
		foreach (CombatTrades trade in TradesFromLastCombat) {
			if (trade.initiator == Team.Player && trade.good) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfPlayerBadTradesLastCombat() {
		int count = 0;
		foreach (CombatTrades trade in TradesFromLastCombat) {
			if (trade.initiator == Team.Player && trade.bad) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfAIGoodTradesLastCombat() {
		int count = 0;
		foreach (CombatTrades trade in TradesFromLastCombat) {
			if (trade.initiator == Team.AI && trade.good) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfAIBadTradesLastCombat() {
		int count = 0;
		foreach (CombatTrades trade in TradesFromLastCombat) {
			if (trade.initiator == Team.AI && trade.bad) {
				count++;
			}
		}
		return count;
	}

	#endregion

	#region Summons

	#region Overall

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

	#region Specifics

	List<SpecificSummonStats> SSS = new List<SpecificSummonStats>();

	public void NewUnitSummoned(SpecificSummonStats ss) {
		SSS.Add(ss);
	}

	public SpecificSummonStats GetLatestSummonStat() {
		return SSS[SSS.Count - 1];
	}

	#endregion

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
	List<int> AIUnitsBroughtToLastCombat = new List<int>();

	public void UnitsBroughtToCombat(List<int> units) {
		UnitsBroughtToLastCombat = units;
	}

	public void AILastDeck(List<int> units) {
		AIUnitsBroughtToLastCombat = units;
	}

	public List<SimpleUnit> GetPlayerDeckAsSimpleUnits() {
		List<SimpleUnit> units = new List<SimpleUnit>();
		SimpleUnit unit;
		foreach (int i in UnitsBroughtToLastCombat) {
			unit = new SimpleUnit();
			unit.ID = i;
			unit.type = Utility.GetElementalTypeFromID(i);
			unit.shadow = Utility.GetIsShadowFromID(i);
			unit.stone = Utility.GetIsStoneFromID(i);
			units.Add(unit);
		}
		return units;
	}

	public List<SimpleUnit> GetAIDeckAsSimpleUnits() {
		List<SimpleUnit> units = new List<SimpleUnit>();
		SimpleUnit unit;
		foreach (int i in AIUnitsBroughtToLastCombat) {
			unit = new SimpleUnit();
			unit.ID = i;
			unit.type = Utility.GetElementalTypeFromID(i);
			unit.shadow = Utility.GetIsShadowFromID(i);
			unit.stone = Utility.GetIsStoneFromID(i);
			units.Add(unit);
		}
		return units;
	}

	public bool PlayerHasSpecialInDeck(bool stone, bool shadow) {
		foreach (int i in UnitsBroughtToLastCombat) {
			if(shadow && (i == 8 || i == 9)) {
				return true;
			}
			if(stone && (i == 10 || i == 11)) {
				return true;
			}
		}
		return false;
	}

	public bool PlayerHasTypeInDeck(ElementalTypes type) {
		foreach (int i in UnitsBroughtToLastCombat) {
			if(Utility.GetElementalTypeFromID(i) == type) {
				return true;
			}
		}
		return false;
	}

	#endregion

	#endregion
}

#region ExtraClasses

public class CombatTrades {
	public ElementalTypes attacker;
	public ElementalTypes defender;
	public bool good;
	public bool bad;
	public bool retaliation;
	public Team initiator;
	public bool towerHit = false;
	public bool killHit;
}

public class SummonPlayerData {
	public ElementalTypes type;
	public bool stone;
	public bool shadow;
}

public class SpecificSummonStats {
	public int cost;
	public int spotsLeftAfter;
	public int pointsLeftAfter;
}

public class SimpleUnit {
	public int ID;
	public ElementalTypes type;
	public bool shadow = false;
	public bool stone = false;
}

#endregion