using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour {

	private bool inTrigger = false;

	public List<int> availableQuestIDs = new List<int>();
	public List<int> receivableQuestIDs = new List<int>();

	public GameObject questMarker;
	public Image markerImage;

	public Sprite questAvailable;
	public Sprite questReceivable;

	void Start () {

		SetQuestMarker ();
	}

	void SetQuestMarker() {

		if (QuestManager.questManager.CheckCompletedQuests(this)) {

			questMarker.SetActive (true);
			markerImage.sprite = questReceivable;
			markerImage.color = Color.yellow;
		}
		else if (QuestManager.questManager.CheckAvailableQuests(this)) {

			questMarker.SetActive (true);
			markerImage.sprite = questAvailable;
			markerImage.color = Color.red;
		}
		else if (QuestManager.questManager.CheckAcceptedQuests(this)) {

			questMarker.SetActive (true);
			markerImage.sprite = questReceivable;
			markerImage.color = Color.gray;
		} 
		else {

			questMarker.SetActive (false);
		}
	}

	void Update() {

		if (inTrigger && Input.GetKeyDown(KeyCode.Space)) {

			//quest UI manager
			QuestManager.questManager.QuestRequest (this);
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
