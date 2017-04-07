using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int id = -1;
	bool teleported = false;
	public Transform TPPos;

	void Awake() {
		if (id == -1) {
			Debug.LogError("This checkpoint has no ID! " + gameObject.name);
		}
		CheckpointManager.instance.AssignCheckpoint(id, this);
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

	/// <summary>
	/// DO ONLY USE THIS IF YOU ARE GOING TO TELEPORT THE PLAYER!
	/// </summary>
	/// <returns></returns>
	public Vector3 GetTPPos() {
		teleported = true;
		return TPPos.position;
	}
}
