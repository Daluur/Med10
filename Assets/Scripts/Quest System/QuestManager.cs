using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

	public static QuestManager questManager;

	private JsonData questData;

	public List<Quest> questList = new List<Quest>();
	private int currentQuest = 0;

	public GameObject questTitle;
	public GameObject questDescription;

	void Awake () {
		if (questManager == null) {
			questManager = this;
		}
		else if (questManager != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		questData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Quests.json"));
		ConstructQuestDatabase ();
	}

	void Start () {

		questList [0].Progress = Quest.QuestProgress.AVAILABLE;
		AcceptQuest (0);
	}
		

	//ACCEPT QUEST
	public void AcceptQuest(int id) {
	
		for (int i = 0; i < questList.Count; i++) {

			if (questList[i].ID == id && questList[i].Progress == Quest.QuestProgress.AVAILABLE) {
				questList [i].Progress = Quest.QuestProgress.ACCEPTED;
				questTitle.GetComponent<Text> ().text = questList [i].Title;
				questDescription.GetComponent<Text> ().text = questList [i].Description;
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

	void ConstructQuestDatabase() {
		for (int i = 0; i < questData.Count; i++) {
			questList.Add(new Quest(
				(int)questData[i]["id"], 
				questData[i]["title"].ToString(), 
				(int)questData[i]["progress"],
				questData[i]["description"].ToString(),
				questData[i]["hint"].ToString(),
				questData[i]["completion"].ToString(),
				questData[i]["summary"].ToString(),
				(int)questData[i]["next"],
				questData[i]["to collect"].ToString(),
				(int)questData[i]["collect amount"], 
				(int)questData[i]["current amount"],
				(int)questData[i]["gold reward"],
				(int)questData[i]["item reward"]
			));
		}
	}
}
