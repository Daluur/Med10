using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Overworld {
	public class GeneralConfirmationBox : ControlUIElement, IInteractable {
		public static GeneralConfirmationBox instance;

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

		private Text displayText;
		private Button buttonOne, buttonTwo;
		private Vector2 bOnePos, bTwoPos;

		void Start() {
			Register(this);
			var buttons = GetComponentsInChildren<Button>();
			if (buttons.Length != 2) {
				Debug.LogError("The general confirmation box behaviour assumes there are two buttons as children to the object, found: " + buttons.Length);
				return;
			}
			if (buttons[0].GetComponent<RectTransform>().anchoredPosition.x < buttons[1].GetComponent<RectTransform>().anchoredPosition.x) {
				buttonOne = buttons[0];
				buttonTwo = buttons[1];
			}
			else {
				buttonOne = buttons[1];
				buttonTwo = buttons[0];
			}
			bOnePos = buttonOne.GetComponent<RectTransform>().anchoredPosition;
			bTwoPos = buttonTwo.GetComponent<RectTransform>().anchoredPosition;
			foreach (var text in GetComponentsInChildren<Text>()) {
				if (text.gameObject.GetComponentInParent<Button>() == null) {
					displayText = text;
				}
			}
		}

		public void ShowPopUp(string text, string buttonTextOne, UnityAction buttonAction = null, string buttonTextTwo = null) {
			inputManager.StopPlayer();
			if (buttonTextOne == null) {
				buttonTextOne = "OK";
			}

			if (buttonTextTwo == null) {
				DoPopUp(text, buttonTextOne, buttonAction);
				OpenElement();
				return;
			}
			DoPopUp(text, buttonTextOne, buttonTextTwo, buttonAction);
			OpenElement();
		}

		private void DoPopUp(string text, string buttonText, UnityAction buttonAction) {
			displayText.text = text;
			buttonTwo.gameObject.SetActive(false);
			var buttonOneRect = buttonOne.GetComponent<RectTransform>();
			var pos = 0;
			buttonOneRect.anchoredPosition = new Vector2(pos, bOnePos.y);
			buttonOne.GetComponentInChildren<Text>().text = buttonText;
			if (buttonAction != null) {
				buttonOne.onClick.AddListener(buttonAction);
			}
			buttonOne.onClick.AddListener(ClosePopUp);
		}

		private void DoPopUp(string text, string buttonOneText, string buttonTwoText, UnityAction buttonAction = null) {
			displayText.text = text;
			buttonOne.GetComponent<RectTransform>().anchoredPosition = bOnePos;
			buttonTwo.GetComponent<RectTransform>().anchoredPosition = bTwoPos;
			buttonOne.GetComponentInChildren<Text>().text = buttonOneText;
			buttonTwo.GetComponentInChildren<Text>().text = buttonTwoText;
			buttonOne.onClick.AddListener(ClosePopUp);
			buttonTwo.onClick.AddListener(ClosePopUp);
			if(buttonAction!=null){
				buttonTwo.onClick.AddListener(buttonAction);
			}
			else {
				buttonTwo.onClick.AddListener(DOSOMETHING);
			}
		}

		private void ClosePopUp() {
			RemoveListeners();
			inputManager.ResumePlayer();
			CloseElement();
		}

		private void RemoveListeners() {
			buttonOne.onClick.RemoveAllListeners();
			buttonTwo.onClick.RemoveAllListeners();
		}

		private void DOSOMETHING() {
			Debug.LogError("The behaviour of this button has not been defined, do you need two buttons?");
		}


		public void DoAction() {

		}

		public void DoAction<T>(T param) {

		}

		public ControlUIElement GetControlElement() {
			return this;
		}
	}
}
