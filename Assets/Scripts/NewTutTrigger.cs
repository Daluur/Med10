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
		TutorialHandler.instance.NewWorldTrigger(TutToTrigger);
	}
}

public enum OWTUTTRIGGERS {
	OpenInventory,
	EnterShop,
	EnterCombat,
	types,
	concede,
	shadow,
}