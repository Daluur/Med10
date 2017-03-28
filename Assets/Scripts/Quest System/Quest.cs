using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {

	public enum QuestProgress {NOT_AVAILABLE, AVAILABLE, ACCEPTED, COMPLETED, DONE, FAILED}

	public string Title;			//title for the quest
	public int ID;					//ID for the quest (maybe a string?)
	public QuestProgress Progress;	//state of the quest
	public string Description;		//description of the quest objectives
	public string Hint;				//hint to help the player
	public string Completion;		//on completion reward text	#MAYBE NOT NEEDED
	public string Summary;			//a summery of the quest 	#MAYBE NOT NEEDED
	public int NextQuest;			//next quest for chained quests

	public string QuestCollect;		//name of the entity to collect/kill/etc
	public int CollectAmount;		//required amount of entities for the quest
	public int CurrentCollect;	//number of entities collected

	public int RewardGold;			//gold reward for completion
	public int RewardItem;			//item rewarded for completion (from json database)


	public Quest(int id, string tit, int prog, string des, string hin, string com, string sum, int nex, string col, int am, int cur, int gol, int item) {
		this.ID = id;
		this.Title = tit;
		this.Progress = (QuestProgress)prog;
		this.Description = des;
		this.Hint = hin;
		this.Completion = com;
		this.Summary = sum;
		this.NextQuest = nex;
		this.QuestCollect = col;
		this.CollectAmount = am;
		this.CurrentCollect = cur;
		this.RewardGold = gol;
		this.RewardItem = item;
	}
}
