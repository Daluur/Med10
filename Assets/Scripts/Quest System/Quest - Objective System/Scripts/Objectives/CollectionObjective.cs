using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem {
	public class CollectionObjective : IQuestObjective {

		private string title;
		private string description;
		private bool isComplete;
		private bool isBonus;
		private string type;
		private int collectAmount; 
		private int currentAmount; 
		private GameObject itemToCollect;

		/// <summary>
		/// This constructor builds a collection objective.
		/// </summary>
		/// <param name="t">type of collection. </param>
		/// <param name="a">amount needed for objective. </param>
		/// <param name="c">current amount on player. </param>
		/// <param name="i">item to collect. </param>
		/// <param name="d">description. </param>
		/// <param name="b">if the objective is a bonus objective. </param>

		public CollectionObjective(string t, int a, int c, GameObject i, string d, bool b) {
			title = t + " " + a + " " + i.name;
			type = t;
			description = d;
			itemToCollect = i;
			collectAmount = a;
			currentAmount = c;
			isBonus = b;
			CheckProgress ();
		}


		public void UpdateProgress ()
		{
			return;
		}

		public void CheckProgress ()
		{
			if (currentAmount >= collectAmount) {
				isComplete = true;
			} 
			else {
				isComplete = false;
			}
		}

		public override string ToString ()
		{
			return currentAmount + "/" + collectAmount + " " + itemToCollect.name + " " + type + "ed!";
		}

		public string Title {
			get {
				return title;
			}
		}

		public string Description {
			get {
				return description;
			}
		}
			
		public int CollectAmount {
			get {
				return collectAmount;
			}
		}

		public int CurrentAmount {
			get {
				return currentAmount;
			}
		}

		public GameObject ItemToCollect {
			get {
				return itemToCollect;
			}
		}

		public bool IsComplete {
			get {
				return isComplete;
			}
		}

		public bool IsBonus {
			get {
				return isBonus;
			}
		}
	}
}