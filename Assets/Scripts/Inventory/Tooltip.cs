using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
	private Item item;
	private string data;
	private GameObject tooltip;
	private string color;
	private string special = "";

	void Start() {
		tooltip = GameObject.Find ("Tooltip");
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

	public void Deactivate() {
		tooltip.SetActive (false);
	}

	public void ConstructData() {
		switch (item.Type) {
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
		default:
			color = "#cc6600";
			break;
		}

		if (item.Shadow == true) {
			color = "#cc33ff";
			special = "<color=" + color + ">(Shadow)</color>";
		}
		else if (item.Stone == true) {
			color = "#ffffff";
			special = "<color=" + color + ">(Stone)</color>";
		} 
		else {
			special = "";
		}

		data = "<color="+color+"><b>" + item.Title + "</b></color>\n\n" + item.Description + "\n\n<b>Type: </b>" + item.Type + " " + special + "\n<b>Attack: </b>" + item.Attack + "\n<b>Health: </b>" + item.Health + "\n<b>Moves: </b>" + item.Moves + "\n<b>Summon Cost: </b>" + item.SummonCost;
		tooltip.transform.GetChild (0).GetComponent<Text> ().text = data;
	}

}
