using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Units;
using CombatWorld.Utility;
using Overworld;

public class DataGathering {

	#region Singleton Things

	private static DataGathering instance;

	static bool isStatic = false;
	static bool isDynamic = true;

	public static DataGathering Instance {
		get {
			if (instance == null) {
				instance = new DataGathering();
				System.Random rand = new System.Random(Time.renderedFrameCount);
				System.Random rand2 = new System.Random((int)(Time.realtimeSinceStartup*10000000));
				if (isStatic) {
					instance.Static = true;
				}
				else if (isDynamic) {
					instance.Static = false;
				}
				else {
					instance.Static = new System.Random(DateTime.Now.Millisecond).Next(0, 2) == 0 ? true : false;
				}
				instance.ID = instance.Static ? "S" : "D";
				instance.ID += DateTime.Now.ToString("dd-HH:mm:ss:fff") + "R" + rand.Next(0,10000) + "R" + rand2.Next(0,10000);
				instance.StartTime = DateTime.Now;
			}
			return instance;
		}
	}

	#endregion

	public DateTime StartTime;

	#region ID

	public string ID;
	public bool Static;

	public void OverrideID(string id, bool st) {
		if(id == "" || id == null) {
			return;
		}
		ID = id;
		Static = st;
		DynamicTut.instance.FixVersion(st);
	}

	#endregion

	#region OW things

	//Movement
	private bool hasMoved;
	public bool HasMoved { get { return hasMoved; } set { hasMoved = value; } }

	#endregion

	#region CW things

	#region Generel stuff

	int amountOfCombats = 0;

	public void StartNewCombat() {
		amountOfCombats++;
		TradesSinceLastReset.Clear();
		TradesFromLastCombat.Clear();
		SummonedUnitsLastCombat.Clear();
		SSS.Clear();
		movedThroughLastCombat = 0;
		didNotMoveThroughLastCombat = 0;
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
	List<CombatTrades> TradesSinceLastReset = new List<CombatTrades>();
	List<CombatTrades> TradesFromLastCombat = new List<CombatTrades>();

	public void LoadTrades(List<CombatTrades> tr) {
		if(tr == null) {
			return;
		}
		AllTrades = tr;
	}

	public void ResetTrades() {
		TradesSinceLastReset.Clear();
		AllTrades.Add(new CombatTrades() { initiator = Team.NONE, movedThroughUnit = true });
	}

	public void AddCombatTrade(CombatTrades newTrade) {
		TradesFromLastCombat.Add(newTrade);
		TradesSinceLastReset.Add(newTrade);
		AllTrades.Add(newTrade);
	}

	public List<CombatTrades> GetTradesFromLastCombat() {
		return TradesFromLastCombat;
	}

	public List<CombatTrades> GetAllTrades() {
		return AllTrades;
	}

	public List<CombatTrades> GetTradesSinceReset() {
		return TradesSinceLastReset;
	}

	public int GetNumberOfPlayerGoodTradesTotal() {
		int count = 0;
		foreach (CombatTrades trade in TradesSinceLastReset) {
			if (trade.initiator == Team.Player && trade.good) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfPlayerBadTradesTotal() {
		int count = 0;
		foreach (CombatTrades trade in TradesSinceLastReset) {
			if (trade.initiator == Team.Player && trade.bad) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfAIGoodTradesTotal() {
		int count = 0;
		foreach (CombatTrades trade in TradesSinceLastReset) {
			if (trade.initiator == Team.AI && trade.good) {
				count++;
			}
		}
		return count;
	}

	public int GetNumberOfAIBadTradesTotal() {
		int count = 0;
		foreach (CombatTrades trade in TradesSinceLastReset) {
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

	public void LoadSummons(List<SummonPlayerData> su) {
		if (su == null) {
			return;
		}
		AllSummonedUnits = su;
	}

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

	public int GetShadowSummonLastCombatCount() {
		int count = 0;
		foreach (SummonPlayerData data in SummonedUnitsLastCombat) {
			if (data.shadow) {
				count++;
			}
		}
		return count;
	}

	public int GetShadowSummonAllTimeCount() {
		int count = 0;
		foreach (SummonPlayerData data in AllSummonedUnits) {
			if (data.shadow) {
				count++;
			}
		}
		return count;
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
		turnedToStoneCount++;
	}

	#endregion

	#region Shadow

	public int movedShadowThroughOthersSaveThisValue = 0;
	public int movedShadowWithoutMovingThroughOtherUnitsSaveThisValue = 0;

	public int movedShadowUnitThroughUnitCount = 0;
	public int movedShadowWithoutMovingThroughUnitsCount = 0;

	public int movedThroughLastCombat = 0;
	public int didNotMoveThroughLastCombat = 0;

	public void LoadShadow(int s1, int s2) {
		movedShadowThroughOthersSaveThisValue = s1;
		movedShadowWithoutMovingThroughOtherUnitsSaveThisValue = s2;
	}

	public void MovedShadowThroughUnit() {
		movedShadowUnitThroughUnitCount++;
		movedShadowThroughOthersSaveThisValue++;
		movedThroughLastCombat++;
	}

	public void MoveShadowNotThroughUnits() {
		movedShadowWithoutMovingThroughUnitsCount++;
		movedShadowWithoutMovingThroughOtherUnitsSaveThisValue++;
		didNotMoveThroughLastCombat++;
	}

	public void ResetShadow() {
		movedShadowUnitThroughUnitCount = 0;
		movedShadowWithoutMovingThroughUnitsCount = 0;
		movedThroughLastCombat = 0;
		didNotMoveThroughLastCombat = 0;
	}
	
	#endregion

	#region Tower

	public bool hasAttackedTower = false;
	public bool hasKilledATower = false;
	public int stoodBesideTowerNotAttackingcount = 0;

	public void AttackedTower() {
		hasAttackedTower = true;
	}

	public void KilledTower() {
		hasKilledATower = true;
	}

	public void StoodBesideTowerAndDidNotAttack() {
		stoodBesideTowerNotAttackingcount++;
	}

	#endregion

	#region Decks

	List<DeckDataClass> ALLDeckData = new List<DeckDataClass>();
	List<int> UnitsBroughtToLastCombat = new List<int>();
	List<int> AIUnitsBroughtToLastCombat = new List<int>();

	public void LoadDecks(List<DeckDataClass> dd) {
		if (dd == null) {
			return;
		}
		ALLDeckData = dd;
	}

	public void UnitsBroughtToCombat(List<int> units) {
		UnitsBroughtToLastCombat = units;
		ALLDeckData.Add(new DeckDataClass() { playerUnits = units, encounterID = SceneHandler.instance.GetDeck().id, });
	}

	public void AILastDeck(List<int> units) {
		AIUnitsBroughtToLastCombat = units;
	}

	public List<SimpleUnit> GetPlayerDeckAsSimpleUnitsLastCombat() {
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

	public List<SimpleUnit> GetAIDeckAsSimpleUnitsLastCombat() {
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

	public List<DeckDataClass> GetAllDeckData() {
		return ALLDeckData;
	}

	public List<SimpleUnit> GetSimpleUnitsFromDeckData(DeckDataClass DDC,bool AI = false) {
		if (AI) {
			List<SimpleUnit> units = new List<SimpleUnit>();
			SimpleUnit unit;
			foreach (int i in DeckHandler.GetDeckFromID(DDC.encounterID).unitIDs) {
				unit = new SimpleUnit();
				unit.ID = i;
				unit.type = Utility.GetElementalTypeFromID(i);
				unit.shadow = Utility.GetIsShadowFromID(i);
				unit.stone = Utility.GetIsStoneFromID(i);
				units.Add(unit);
			}
			return units;
		}
		else {
			List<SimpleUnit> units = new List<SimpleUnit>();
			SimpleUnit unit;
			foreach (int i in DDC.playerUnits) {
				unit = new SimpleUnit();
				unit.ID = i;
				unit.type = Utility.GetElementalTypeFromID(i);
				unit.shadow = Utility.GetIsShadowFromID(i);
				unit.stone = Utility.GetIsStoneFromID(i);
				units.Add(unit);
			}
			return units;
		}
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

	#region tutthings

	public int typesToldCount = 0;
	public int shadowToldCount = 0;

	public int notLearnedShadowCount = 0;
	public int notLearnedTypesCount = 0;

	public void WasToldAboutShadow() {
		shadowToldCount++;
	}
	public void WasToldAboutTypes() {
		typesToldCount++;
	}
	public void HadNotLearnedShadow() {
		notLearnedShadowCount++;
	}
	public void HadNotLearnedTypes() {
		notLearnedTypesCount++;
	}

	#endregion

	#endregion
}

#region ExtraClasses

public class CombatTrades {
	public ElementalTypes attacker;
	public ElementalTypes defender;
	public bool shadow;
	public bool movedThroughUnit;
	public bool stone;
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

public class DeckDataClass {
	public List<int> playerUnits;
	public int encounterID;
}

#endregion