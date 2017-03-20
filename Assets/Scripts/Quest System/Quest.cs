using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {

	public enum QuestProgress {NOT_AVAILABLE, AVAILABLE, ACCEPTED, COMPLETED, DONE, FAILED}

	public string title;			//title for the quest
	public int id;					//ID for the quest (maybe a string?)
	public QuestProgress progress;	//state of the quest
	public string description;		//description of the quest objectives
	public string hint;				//hint to help the player
	public string completion;		//on completion reward text	#MAYBE NOT NEEDED
	public string summary;			//a summery of the quest 	#MAYBE NOT NEEDED
	public int nextQuest;			//next quest for chained quests

	public string questCollect;		//name of the entity to collect/kill/etc
	public int collectAmount;		//required amount of entities for the quest
	public int currentCollected;	//number of entities collected

	public int rewardGold;			//gold reward for completion
	public int rewardItem;			//item rewarded for completion (from json database)

}
