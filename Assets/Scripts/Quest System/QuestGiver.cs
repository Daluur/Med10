using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour {

	private bool inTrigger = false;

	public GameObject questMarker;
	public Image markerImage;

	public Sprite questAvailable;
	public Sprite questReceivable;

	void Start () {

		SetQuestMarker ();
	}

	void SetQuestMarker() {

	}

	void Update() {

		if (inTrigger && Input.GetKeyDown(KeyCode.Space)) {

		}
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player") {

			inTrigger = true;
		}
	}

	void OnTriggerExit(Collider other) {

		if (other.tag == "Player") {

			inTrigger = false;
		}
	}
}
