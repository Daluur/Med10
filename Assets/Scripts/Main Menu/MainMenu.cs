using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	private int volume = 100;
	public Text volumeNum;
	public Slider volumeSlider;

	void Start() {
		AudioHandler.instance.StartOWBGMusic();
	}

	void Update () {
		volume = (int)volumeSlider.value;
		AudioListener.volume = volume/100f;
		volumeNum.text = volume.ToString() + "%";
	}

	public void LoadGame () {

		Application.LoadLevel ("Overworld Map");
	}
}
