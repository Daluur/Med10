using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour {

	void Start() {
		DataToServer.SendData(this);
	}

	public void SurveyButton() {
		Application.OpenURL("http://unity3d.com/");
	}
}
