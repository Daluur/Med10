using System.Collections;
using System.Collections.Generic;
using CombatWorld;
using UnityEngine;
using CombatWorld.Utility;
using CombatWorld.Units;
using SimplDynTut;

public class PlayerData {

	#region Singleton Things

	private static PlayerData instance;

	public static PlayerData Instance {
		get {
			if (instance == null) {
				instance = new PlayerData();
			}
			return instance;
		}
	}

	#endregion

	#region CombatWorldTriggers
	
	public bool hasRunTryingToAttack,
		hasRunSelectingSummonedUnit,
		hasRunSelectingUnitWithNoMovesLeft,
		hasRunSummonDisplayTimer,
		hasRunUnitSelection,
		hasShownEndTurn,
		hasEnteredCombat,
		hasEverAttacked,
		hasShownMovementCW,
		hasMovedInCW;

	public int timesTryingToAttack, timesTryingToSelectSummon, timesTryingToSelectUnitWithoutMovesLeft, currentTurn = 0;
	public int timesTriedToSelectEnemyUnits {
		get { return m_TimesTriedToSelectEnemyUnits; }
		set {
			if (value >= 5) {
				m_TimesTriedToSelectEnemyUnits++;
				GameController.instance.cwTriggers.TryingToSelectEnemyUnits();
				
			}
			else {
				m_TimesTriedToSelectEnemyUnits = value;
			}
		}
	}

	private int m_TimesTriedToSelectEnemyUnits = 0;

	#endregion
	
	#region OW

	public bool GetHasPlayerMoved() {
		return DataGathering.Instance.HasMoved;
	}

	#endregion

	#region Trades

	/// <summary>
	/// Only Trades initiated by the player.
	/// </summary>
	/// <returns></returns>
	public int GetNumberOfGoodTradesLastCombat() {
		return DataGathering.Instance.GetNumberOfPlayerBadTradesLastCombat();
	}

	/// <summary>
	/// Only Trades initiated by the player.
	/// </summary>
	/// <returns></returns>
	public int GetNumberOfGoodTradesTotal() {
		return DataGathering.Instance.GetNumberOfPlayerGoodTradesTotal();
	}

	public List<CombatTrades> GetAllTrades() {
		return DataGathering.Instance.GetTradesSinceReset();
	}

	public void ResetTrades() {
		DataGathering.Instance.ResetTrades();
	}

	public List<CombatTrades> GetTradesFromLastCombat() {
		return DataGathering.Instance.GetTradesFromLastCombat();
	}

	#endregion

	#region Summons

	#region Overall

	public bool GetHasEverSummonedAUnit() {
		return DataGathering.Instance.GetAllSummonedUnits().Count != 0;
	}

	public bool GetHasSummonedAUnitLastCombat() {
		return DataGathering.Instance.GetSummonedLastCombat().Count != 0;
	}

	public bool GetHasEverSummonedOfType(ElementalTypes type) {
		return DataGathering.Instance.HasEverSummonType(type);
	}

	public bool GetHasSummonedOfTypeLastCombat(ElementalTypes type) {
		return DataGathering.Instance.HasSummonedTypeLastCombat(type);
	}

	public bool GetHasEverSummonedSpecial(bool stone, bool shadow) {
		return DataGathering.Instance.HasEverSummonedSpecial(stone, shadow);
	}

	public bool GetHasSummonedSpecialLastCombat(bool stone, bool shadow) {
		return DataGathering.Instance.HasSummonedSpecialLastCombat(stone, shadow);
	}

	public int GetShadowSummonedLastCombat() {
		return DataGathering.Instance.GetShadowSummonLastCombatCount();
	}

	public int GetShadowSummmonedAllTime() {
		return DataGathering.Instance.GetShadowSummonAllTimeCount();
	}

	#endregion

	#region Specific

	public SpecificSummonStats GetLatestSummonedPlayerUnitInfo() {
		return DataGathering.Instance.GetLatestSummonStat();
	}

	#endregion

	#endregion

	#region Tower

	public bool GetHasPlayerAttackedTower() {
		return DataGathering.Instance.hasAttackedTower;
	}

	public bool GetHasPlayerKilledTower() {
		return DataGathering.Instance.hasKilledATower;
	}

	public int GetPlayerHasNotAttackedTowerCount() {
		return DataGathering.Instance.stoodBesideTowerNotAttackingcount;
	}

	#endregion

	#region Stone

	public bool GetHasEverSummonedStone() {
		return DataGathering.Instance.HasEverSummonedSpecial(true, false);
	}

	public bool GetHasSummonedStoneLastCombat() {
		return DataGathering.Instance.HasSummonedSpecialLastCombat(true, false);
	}

	public int GetPlayerHasUsedStoneCount() {
		return DataGathering.Instance.turnedToStoneCount;
	}

	public bool GetPlayerHasUsedStone() {
		return DataGathering.Instance.turnedToStoneCount != 0;
	}

	#endregion

	#region Shadow

	public bool GetHasEverSummonedShadow() {
		return DataGathering.Instance.HasEverSummonedSpecial(false, true);
	}

	public bool GetHasSummonedShadowLastCombat() {
		return DataGathering.Instance.HasSummonedSpecialLastCombat(false, true);
	}

	public bool GetPlayerHasUsedShadow() {
		return DataGathering.Instance.movedShadowUnitThroughUnitCount != 0;
	}

	public int GetPlayerHasUsedShadowCount() {
		return DataGathering.Instance.movedShadowUnitThroughUnitCount;
	}

	public int GetMovedShadowWithoutMovingThroughUnit() {
		return DataGathering.Instance.movedShadowWithoutMovingThroughUnitsCount;
	}

	public int GetPlayerHasUsedShadowCountLastCombat() {
		return DataGathering.Instance.movedThroughLastCombat;
	}

	public int GetMovedShadowWithoutMovingThroughUnitLastCombat() {
		return DataGathering.Instance.didNotMoveThroughLastCombat;
	}

	public void ResetShadow() {
		DataGathering.Instance.ResetShadow();
	}

	#endregion

	#region Decks

	public List<SimpleUnit> GetPlayerLastDeckAsSimpleUnits() {
		return DataGathering.Instance.GetPlayerDeckAsSimpleUnitsLastCombat();
	}

	public List<SimpleUnit> GetAILastDeckAsSimpleUnits() {
		return DataGathering.Instance.GetAIDeckAsSimpleUnitsLastCombat();
	}

	public bool GetPlayerHadSpecialInDeck(bool stone, bool shadow) {
		return DataGathering.Instance.PlayerHasSpecialInDeck(stone, shadow);
	}

	public bool GetPlayerHadTypeInDeck(ElementalTypes type) {
		return DataGathering.Instance.PlayerHasTypeInDeck(type);
	}

	public List<DeckDataClass> GetAllDeckData() {
		return DataGathering.Instance.GetAllDeckData();
	}

	public List<SimpleUnit> GetSpecificPlayerDeckFromDeckDataClass(DeckDataClass DDC) {
		return DataGathering.Instance.GetSimpleUnitsFromDeckData(DDC);
	}

	public List<SimpleUnit> GetSpecificAIDeckFromDeckDataClass(DeckDataClass DDC) {
		return DataGathering.Instance.GetSimpleUnitsFromDeckData(DDC, true);
	}

	/// <summary>
	/// A unit can counter several units. E.g. 1 fire unit against 4 nature will count as 4. Thus, 4 fire against 4 nature will be 16.
	/// </summary>
	/// <returns></returns>
	public int GetCountOfUnitsCounteringAI() {
		List<SimpleUnit> AIUnits = GetAILastDeckAsSimpleUnits();
		List<SimpleUnit> PlayerUnits = GetPlayerLastDeckAsSimpleUnits();
		int count = 0;

		foreach (SimpleUnit PlayerUnit in PlayerUnits) {
			foreach (SimpleUnit AIUnit in AIUnits) {
				if (Utility.TypeCounters(PlayerUnit.type, AIUnit.type)) {
					count++;
				}
			}
		}

		return count;
	}

	/// <summary>
	/// A unit can counter several units. E.g. 1 fire unit against 4 nature will count as 4. Thus, 4 fire against 4 nature will be 16.
	/// </summary>
	/// <returns></returns>
	public int GetCountOfUnitsGettingCounteredByAI() {
		List<SimpleUnit> AIUnits = GetAILastDeckAsSimpleUnits();
		List<SimpleUnit> PlayerUnits = GetPlayerLastDeckAsSimpleUnits();
		int count = 0;

		foreach (SimpleUnit AIUnit in PlayerUnits) {
			foreach (SimpleUnit PlayerUnit in AIUnits) {
				if (Utility.TypeCounters(AIUnit.type, PlayerUnit.type)) {
					count++;
				}
			}
		}

		return count;
	}

	#endregion

	#region UnitSelection

	public bool GetHasEverSelectedAUnit() {
		return DataGathering.Instance.HasEverHadSelectedUnit();
	}

	public bool GetHasCurrentlyAUnitSelected() {
		return DataGathering.Instance.HasUnitSelected();
	}

	/// <summary>
	/// Can return null! You can use GetHasCurrentlyAUnitSelected to see if it will be null.
	/// </summary>
	/// <returns></returns>
	public Unit GetCurrentlySelectedUnit() {
		return DataGathering.Instance.GetCurrentlySelectedUnit();
	}

	#endregion
}