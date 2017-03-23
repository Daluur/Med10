using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public static QuestManager questManager;

	public List<Quest> questList = new List<Quest>();		//master quest list
	private int currentQuest = 0;

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
		

	//ACCEPT QUEST
	public void AcceptQuest(int id) {
	
		for (int i = 0; i < questList.Count; i++) {

			if (questList[i].ID == id && questList[i].Progress == Quest.QuestProgress.AVAILABLE) {
				questList [i].Progress = Quest.QuestProgress.ACCEPTED;
			}
		}
	}

	//COMPLETE QUEST
	public void CompleteQuest(int id) {

		for (int i = 0; i < questList.Count; i++) {

			if (questList[i].ID == id && questList[i].Progress == Quest.QuestProgress.COMPLETED) {
				questList [i].Progress = Quest.QuestProgress.DONE;
				currentQuest++;

				//reward
			}
		}
	}
		
	//ADD ITEMS

	public void AddQuestItem (string c, int ia) {

		for (int i = 0; i < questList.Count; i++) {

			if (questList[i].QuestCollect == c && questList[i].Progress == Quest.QuestProgress.ACCEPTED) {
			
				questList [i].CurrentCollect += ia;
			}

			if (questList[i].CurrentCollect >= questList[i].CollectAmount && questList[i].Progress == Quest.QuestProgress.ACCEPTED) {

				questList [i].Progress = Quest.QuestProgress.COMPLETED;
			}
		}
	}

	//REMOVE items
}
