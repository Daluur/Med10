﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	#region OW

	public bool GetHasPlayerMoved() {
		return DataGathering.Instance.HasMoved;
	}

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

	public int GetPlayerUsedShadowToMoveThroughOwnUnitsCount() {
		return DataGathering.Instance.movedShadowUnitThroughFriendlyUnitCount;
	}

	public bool GetPlayerHasUsedShadowToMoveThroughOwnUnits() {
		return DataGathering.Instance.movedShadowUnitThroughFriendlyUnitCount != 0;
	}

	public int GetPlayerUsedShadowToMoveThroughEnemyUnitsCount() {
		return DataGathering.Instance.movedShadowUnitThroughEnemyUnitCount;
	}

	public bool GetPlayerHasUsedShadowToMoveThroughEnemyUnits() {
		return DataGathering.Instance.movedShadowUnitThroughEnemyUnitCount != 0;
	}

	public bool GetPlayerHasUsedShadow() {
		return DataGathering.Instance.movedShadowUnitThroughEnemyUnitCount + DataGathering.Instance.movedShadowUnitThroughFriendlyUnitCount != 0;
	}

	public int GetPlayerHasUsedShadowCount() {
		return DataGathering.Instance.movedShadowUnitThroughEnemyUnitCount + DataGathering.Instance.movedShadowUnitThroughFriendlyUnitCount;
	}

	#endregion
}
