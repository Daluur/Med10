using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overworld;

public class NewTutTrigger : MonoBehaviour {

	public OWTUTTRIGGERS TutToTrigger;

	private void OnTriggerEnter(Collider other) {
		if (other.transform.parent == null) {
			return;
		}
		if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
			return;
		}
		if (CheckCondition()) {
			TutorialHandler.instance.NewWorldTrigger(TutToTrigger);
		}
	}

	bool CheckCondition() {
		switch (TutToTrigger) {
			case OWTUTTRIGGERS.UnitsToBring:
				GeneralConfirmationBox.instance.ShowPopUp("NEED TEXT FOR SELECTING UNIT TUT", "Okay");
				break;
			case OWTUTTRIGGERS.OpenInventory:
				break;
			case OWTUTTRIGGERS.EnterShop:
				break;
			case OWTUTTRIGGERS.BuyUnits:
				break;
			case OWTUTTRIGGERS.EnterCombat:
				break;
			default:
				break;
		}
		return false;
	}
}

public enum OWTUTTRIGGERS {
	UnitsToBring,
	OpenInventory,
	EnterShop,
	BuyUnits,
	EnterCombat,
}