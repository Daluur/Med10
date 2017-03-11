using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameNotificationsSystem : Singleton<GameNotificationsSystem> {

	private bool isRunning = false;
	private Coroutine routine;
	private Text text;
	public float timeForFade = 3f;

	void Start () {
		text = GetComponentInChildren<Text>();

	}

	public void DisplayMessage(string message) {

		UpdateMessage(message);

	}

	public void UpdateMessage(string message) {
		if (isRunning) {
			text.StopCoroutine(routine);
			text.text = message;
			routine = text.StartCoroutine(StartFade(text));
		}
		else {
			text.text = message;
			routine = text.StartCoroutine(StartFade(text));
		}
	}

	private IEnumerator StartFade(Text text) {
		isRunning = true;

		var fadingColor = text.color;
		fadingColor.a = 1;
		text.color = fadingColor;

		yield return new WaitForSeconds(timeForFade);

		while (fadingColor.a > 0.01f) {
			fadingColor.a -= Time.deltaTime;
			text.color = fadingColor;
			yield return new WaitForEndOfFrame();
		}
		text.text = "";
		fadingColor.a = 1;
		text.color = fadingColor;
		isRunning = false;
	}
}
