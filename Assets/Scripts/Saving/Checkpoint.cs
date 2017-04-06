using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int id = -1;
	bool teleported = false;

	void Start() {
		if (id == -1) {
			Debug.LogError("This checkpoint has no ID! " + gameObject.name);
		}
		if(id == SaveLoadHandler.Instance.GetCheckpoint()){
			GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER).GetComponent<Overworld.PlayerMovementOW>().TeleportPlayer(transform.position);
			teleported = true;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
			return;
		}
		if (teleported) {
			teleported = false;
			return;
		}
		SaveLoadHandler.Instance.Save(id);
		GameNotificationsSystem.instance.DisplayMessage(GameNotificationConstants.GAMEWASSAVED);
	}
}
