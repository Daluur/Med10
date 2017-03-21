using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class QuestDatabase {

	private List<Quest> database = new List<Quest>();
	private JsonData itemData;

	public QuestDatabase() {
		itemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Quests.json"));
		ConstructItemDtabase ();
	}

	void ConstructItemDtabase() {
		for (int i = 0; i < itemData.Count; i++) {
			database.Add(new Quest(
				(int)itemData[i]["id"], 
				itemData[i]["title"].ToString(), 
				(int)itemData[i]["progress"],
				itemData[i]["description"].ToString(),
				itemData[i]["hint"].ToString(),
				itemData[i]["completion"].ToString(),
				itemData[i]["summary"].ToString(),
				(int)itemData[i]["next"],
				itemData[i]["to collect"].ToString(),
				(int)itemData[i]["collect amount"], 
				(int)itemData[i]["current amount"],
				(int)itemData[i]["gold reward"],
				(int)itemData[i]["item reward"]
			));
		}
	}
}
