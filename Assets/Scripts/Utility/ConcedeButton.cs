using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld;

public class ConcedeButton : Singleton<ConcedeButton> {

	void Start() {
		gameObject.SetActive(false);
	}

	public void Activate() {
		gameObject.SetActive(true);
	}

	public void DeActivate() {
		gameObject.SetActive(false);
	}

	public void ConcedeClick() {
		if (GameController.instance != null) {
			GameController.instance.GiveUp();
		}
	}
}
