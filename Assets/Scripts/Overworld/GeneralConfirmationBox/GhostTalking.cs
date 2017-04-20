using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overworld {
	public class GhostTalking : ControlUIElement, IInteractable {
		public static GhostTalking instance;

		protected override void Awake() {
			if (instance == null) {
				instance = this;
			}
			else {
				Debug.LogWarning("Duplicate singeton, destroys the new object");
				Destroy(gameObject);
			}
			base.Awake();
		}

		public Sprite[] ghostPictures;
		public Image ghostPic;
		public Text displayText;

		void Start() {
			Register(this);
		}

		public void ShowPopUp(string text, int ghostID) {
			inputManager.StopPlayer();
			displayText.text = text;
			ghostPic.sprite = ghostPictures[ghostID];
			gameObject.SetActive(true);
			StartCoroutine(WaitWithPopUp());
		}

		IEnumerator WaitWithPopUp() {
			while (FadingLoadingScreen.instance.isFading) {
				yield return new WaitForSeconds(0.1f);
			}
			OpenElement();
		}

		public void ClosePopUp() {
			inputManager.ResumePlayer();
			CloseElement();
		}


		public void DoAction() {
			throw new NotImplementedException();
		}

		public void DoAction<T>(T param, Vector3 hitNormal = default(Vector3)) {
			throw new NotImplementedException();
		}

		public ControlUIElement GetControlElement() {
			return this;
		}
	}
}
