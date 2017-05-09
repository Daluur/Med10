using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour {

	void Start() {
		DataToServer.SendData(this);
	}

	public void SurveyButton() {
		Application.OpenURL("https://goo.gl/forms/v6S7lP5J54OuJTpJ2");
	}

	public void Quit() {
		Application.Quit();
	}
}
