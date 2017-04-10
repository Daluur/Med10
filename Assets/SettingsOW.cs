using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsOW : MonoBehaviour {

	private int volume;
	public Text volumeNum;
	public Slider volumeSlider;

	void Update () {
		volume = (int)volumeSlider.value;
		AudioListener.volume = volume/100f;
		volumeNum.text = volume.ToString() + "%";
	}
}
