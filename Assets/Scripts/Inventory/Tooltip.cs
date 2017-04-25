using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Overworld;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Singleton<Tooltip> {
	private Item item;
	private string data;
	private GameObject tooltip;
	private string color;
	private string special = "";
	private Text text;

	void Start() {
		tooltip = GameObject.Find ("Tooltip");
		text = tooltip.transform.GetChild(0).GetComponent<Text>();
		tooltip.SetActive (false);
	}

	void Update() {
		if (tooltip.activeSelf) {
			tooltip.transform.position = Input.mousePosition;
		}
	}

	public void Activate(Item item) {
		this.item = item;
		ConstructData ();
		tooltip.SetActive (true);
	}

	public void Activate(Encounter encounter) {
		FormatEncounterForTooltip(encounter);
		tooltip.SetActive(true);
	}

	private void FormatEncounterForTooltip(Encounter encounter) {
		var deckData = DeckHandler.GetDeckFromID(encounter.deckIDs[0]);
		var tmp = EncounterString(deckData);
		text.text = tmp;
	}

	private void PrimaryType(int deckID) {

		return;
	}

	private string EncounterString(DeckData deckData) {
		//var tmp = "<b>" + deckData.deckName + "</b>" + "\nTypes: <color=" + GetColor(deckData.type1) + ">" + deckData.type1 +
			//"</color>" + ", <color="+ GetColor(deckData.type2) + ">" + deckData.type2 + "</color>" + "\nDifficulty: " + deckData.difficulty;
		var tmp = "<b>" + deckData.deckName + "</b>" + "\nTypes: <color=" + GetColor(deckData.type1) + ">" + deckData.type1 +"</color>";
		if(deckData.type2 != "") {
			tmp += ", <color=" + GetColor(deckData.type2) + ">" + deckData.type2 + "</color>";
		}
		tmp += "\nDifficulty: " + deckData.difficulty;
		return tmp;
	}

	public void Deactivate() {
		tooltip.SetActive (false);
	}

	private String GetColor(string type) {
		string color;
		switch (type) {
			case "Fire":
				color = "#ff0000";
				break;
			case "Water":
				color = "#0099ff";
				break;
			case "Nature":
				color = "#00cc00";
				break;
			case "Lightning":
				color = "#ffff66";
				break;
			case "Shadow":
				color = "#cc33ff";
				break;
			case "Stone":
				color = "#ffffff";
				break;
			default:
				color = "#cc6600";
				break;
		}
		return color;
	}

	private string GetSpecial(bool shadow, bool stone) {
		if (shadow) {
			color = "#cc33ff";
			special = "<color=" + color + ">(Shadow)</color>";
		}
		else if (stone) {
			color = "#ffffff";
			special = "<color=" + color + ">(Stone)</color>";
		}
		else {
			special = "";
		}
		return special;
	}

	public void ConstructData() {

		color = GetColor(item.Type);
		special = GetSpecial(item.Shadow, item.Stone);

		data = "<color="+color+"><b>" + item.Title + "</b></color>\n\n" + item.Description + "\n\n<b>Type: </b>" + item.Type + " " + special + "\n<b>Attack: </b>" + item.Attack + "\n<b>Health: </b>" + item.Health + "\n<b>Moves: </b>" + item.Moves + "\n<b>Summon Cost: </b>" + item.SummonCost;
		text.text = data;
	}

}
