using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckIfSaveGameExists : MonoBehaviour {

	public Text text;
	bool OnlineTest = false;

	// Use this for initialization
	void Start () {
		if (OnlineTest) {
			Destroy(gameObject);
		}
		if (File.Exists(Application.dataPath + "/StreamingAssets/SaveGame.json")) {
			text.text = "THERE IS ALREADY A SAVED GAME!\nPLEASE GO DELETE THE FILE BEFORE STARTING THE GAME!";
		}
		else {
			Destroy(gameObject);
		}
	}
}
