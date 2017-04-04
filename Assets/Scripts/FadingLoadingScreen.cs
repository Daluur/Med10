using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingLoadingScreen : Singleton<FadingLoadingScreen> {

	private Image img;
	public bool isFading;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image>();
		gameObject.SetActive(false);
	}

	public void StartFadeIn() {
		gameObject.SetActive(true);
		StartCoroutine(FadeIn());
	}

	public void StartFadeOut() {
		gameObject.SetActive(true);
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeIn() {
		isFading = true;
		var alpha = 1f;
		var color = Color.black;
		while (alpha > 0) {
			alpha -= 0.05f;
			color.a = alpha;
			img.color = color;
			yield return new WaitForSeconds(0.05f);
		}
		isFading = false;
		gameObject.SetActive(false);
		yield return null;
	}

	private IEnumerator FadeOut() {
		isFading = true;
		var alpha = 0f;
		var color = Color.black;
		while (alpha < 1) {
			alpha += 0.05f;
			color.a = alpha;
			img.color = color;
			yield return new WaitForSeconds(0.05f);
		}
		isFading = false;
		yield return null;
	}
}
