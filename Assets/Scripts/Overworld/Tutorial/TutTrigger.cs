using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutTrigger : MonoBehaviour {

	public int tutWorldID;
	bool hasShown = false;

	private void OnTriggerEnter(Collider other) {
		if(other.transform.parent == null) {
			return;
		}
		if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
			return;
		}
		if (hasShown) {
			return;
		}
		hasShown = true;
		TutorialHandler.instance.WorldTrigger(tutWorldID);
	}
}
