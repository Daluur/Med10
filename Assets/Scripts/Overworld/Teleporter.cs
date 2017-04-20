using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overworld;

public class Teleporter : MonoBehaviour {

	public Transform target;

	public void Activate() {
		gameObject.SetActive(true);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
			return;
		}
		other.transform.parent.GetComponent<PlayerMovementOW>().TeleportPlayer(target.position);
		AudioHandler.instance.PlayCollectGold();
	}
}
