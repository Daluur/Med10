using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overworld;

public class Teleporter : ContextInteraction, IInteractable {

	public Transform target;
	public bool isFinal = false;
	bool shouldBeActive = false;

	void Start() {
		Register(this, KeyCode.Mouse0);
		gameObject.SetActive(shouldBeActive);
	}

	public void Activate() {
		shouldBeActive = true;
		gameObject.SetActive(true);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
			return;
		}
		if (isFinal) {
			UnityEngine.SceneManagement.SceneManager.LoadScene(3);
			return;
		}
		other.transform.parent.GetComponent<PlayerMovementOW>().TeleportPlayer(target.position);
		target.parent.GetComponent<Checkpoint>().Save();
		AudioHandler.instance.PlayCollectGold();
	}

	public override void PerformClosenessAction() {

	}

	public void DoAction() {
		meClicked = false;
	}

	public void DoAction<T>(T param, Vector3 hitNormal = new Vector3()) {
		meClicked = true;
		CheckDistance();
	}

	public ControlUIElement GetControlElement() {
		return null;
	}
}
