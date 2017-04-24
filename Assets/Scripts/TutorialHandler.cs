using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : Singleton<TutorialHandler> {
	[HideInInspector]
	public bool unitFirst = true;
	[HideInInspector]
	public bool combatFirstTurn = true;
	[HideInInspector]
	public bool combatSecondTurn = true;
	[HideInInspector]
	public bool combatThirdTurn = true;
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

}
