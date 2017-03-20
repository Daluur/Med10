using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public static QuestManager questManager;

	public List<Quest> questList = new List<Quest>();		//master quest list
	public List<Quest> currentQuests = new List<Quest>();	//current quest list

	//private vars for questobject

	void Awake () {
		if (questManager == null) {
			questManager = this;
		}
		else if (questManager != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	//CHECK quest
	public void QuestRequest(QuestGiver q) {

		if (q.availableQuestIDs.Count > 0) {

			for (int i = 0; i < questList.Count; i++) {

				for (int j = 0; j < q.availableQuestIDs.Count; j++) {

					if (questList[i].id == q.availableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.AVAILABLE) {

						Debug.Log ("Quest ID: " + q.availableQuestIDs[j] + " " + questList[i].progress);

						//quest ui manager
					}
				}
			}

			for (int i = 0; i < currentQuests.Count; i++) {

				for (int j = 0; j < q.receivableQuestIDs.Count; j++) {

					if (currentQuests[i].id == q.receivableQuestIDs[j] && currentQuests[i].progress == Quest.QuestProgress.ACCEPTED || currentQuests[i].progress == Quest.QuestProgress.COMPLETED) {

						CompleteQuest (q.receivableQuestIDs[j]);
						//quest ui manager
					}
				}
			}
		}
	}

	//ACCEPT QUEST
	public void AcceptQuest(int id) {
	
		for (int i = 0; i < questList.Count; i++) {

			if (questList[i].id == id && questList[i].progress == Quest.QuestProgress.AVAILABLE) {

				currentQuests.Add (questList [i]);
				questList [i].progress = Quest.QuestProgress.ACCEPTED;
			}
		}
	}

	//GIVE UP QUEST - MAYBE NOT NEEDED
	public void DeleteQuest(int id) {

		for (int i = 0; i < currentQuests.Count; i++) {

			if (currentQuests [i].id == id && currentQuests [i].progress == Quest.QuestProgress.ACCEPTED) {

				currentQuests [i].progress = Quest.QuestProgress.FAILED;
				//currentQuests [i].currentCollected = 0;
				//currentQuests.Remove (currentQuests [i]);
			}
		}
	}

	//COMPLETE QUEST
	public void CompleteQuest(int id) {

		for (int i = 0; i < currentQuests.Count; i++) {

			if (currentQuests[i].id == id && currentQuests[i].progress == Quest.QuestProgress.COMPLETED) {
				currentQuests [i].progress = Quest.QuestProgress.COMPLETED;
				//currentQuests.Remove (currentQuests[i]);

				//reward
			}
		}

		CheckChainQuest (id);
	}

	//check chain quest
	void CheckChainQuest(int id) {

		int tempID = 0;
		for (int i = 0; i < questList.Count; i++) {

			if (questList[i].id == id && questList[i].nextQuest > 0) {

				tempID = questList [i].nextQuest;
			}
		}

		if (tempID > 0) {

			for (int i = 0; i < questList.Count; i++) {

				if (questList[i].id == tempID && questList[i].progress == Quest.QuestProgress.NOT_AVAILABLE) {

					questList [i].progress = Quest.QuestProgress.AVAILABLE;
				}
			}
		}
	}

	//ADD ITEMS

	public void AddQuestItem (string c, int ia) {

		for (int i = 0; i < currentQuests.Count; i++) {

			if (currentQuests[i].questCollect == c && currentQuests[i].progress == Quest.QuestProgress.ACCEPTED) {
			
				currentQuests [i].currentCollected += ia;
			}

			if (currentQuests[i].currentCollected >= currentQuests[i].collectAmount && currentQuests[i].progress == Quest.QuestProgress.ACCEPTED) {

				currentQuests [i].progress = Quest.QuestProgress.COMPLETED;
			}
		}
	}

	//REMOVE items

	//========== REQUEST QUEST STATUS FUNCTIONS ==========//

	public bool RequestAvailableQuest(int questID) {		

		for (int i = 0; i < questList.Count; i++) {

			if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.AVAILABLE) {
				return true;
			}
		}
		return false;
	}

	public bool RequestAcceptedQuest(int questID) {		

		for (int i = 0; i < questList.Count; i++) {

			if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.ACCEPTED) {
				return true;
			}
		}
		return false;
	}

	public bool RequestCompletedQuest(int questID) {		

		for (int i = 0; i < questList.Count; i++) {

			if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.COMPLETED) {
				return true;
			}
		}
		return false;
	}

	//###################################################//

	public bool CheckAvailableQuests(QuestGiver q) {

		for (int i = 0; i < questList.Count; i++) {

			for (int j = 0; j < q.availableQuestIDs.Count; j++) {

				if (questList[i].id == q.availableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.AVAILABLE) {

					return true;
				}
			}
		}

		return false;
	}

	public bool CheckAcceptedQuests(QuestGiver q) {

		for (int i = 0; i < questList.Count; i++) {

			for (int j = 0; j < q.receivableQuestIDs.Count; j++) {

				if (questList[i].id == q.availableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.ACCEPTED) {

					return true;
				}
			}
		}

		return false;
	}

	public bool CheckCompletedQuests(QuestGiver q) {

		for (int i = 0; i < questList.Count; i++) {

			for (int j = 0; j < q.receivableQuestIDs.Count; j++) {

				if (questList[i].id == q.availableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.COMPLETED) {

					return true;
				}
			}
		}

		return false;
	}
}
