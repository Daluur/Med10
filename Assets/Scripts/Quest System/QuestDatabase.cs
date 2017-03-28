using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class QuestDatabase {

	private List<Quest> database = new List<Quest>();
	private JsonData questData;

	public QuestDatabase() {
		questData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Quests.json"));
		ConstructItemDtabase ();
	}

	void ConstructItemDtabase() {
		for (int i = 0; i < questData.Count; i++) {
			database.Add(new Quest(
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
