﻿using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Overworld {
	public class GeneralConfirmationBox : ControlUIElement, IInteractable {
		public static GeneralConfirmationBox instance;
		private bool isOpen;
		public bool IsOpen {  get { return isOpen; } }
		protected override void Awake() {
			if (instance == null) {
				instance = this;
			}
			else {
				//Debug.LogWarning("Duplicate singeton, destroys the new object");
				Destroy(gameObject);
			}
			base.Awake();
		}

		public GameObject blockingPanel;

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
			if (isOpen || isRunning && isShowing) {
				Queue(text, buttonTextOne, buttonAction, buttonTextTwo);
			}
			else {
				ShowNew(text, buttonTextOne, buttonAction, buttonTextTwo);
			}
		}

		public void PartOfMultiPage(string text, string buttonText = "Continue", UnityAction buttonAction = null) {
			ShowNew(text, buttonText, buttonAction);
		}

		void ShowNew(string text, string buttonTextOne, UnityAction buttonAction = null, string buttonTextTwo = null) {
			inputManager.StopPlayer();
			blockingPanel.SetActive(true);
			if (buttonTextOne == null) {
				buttonTextOne = "OK";
			}

			if (buttonTextTwo == null) {
				DoPopUp(text, buttonTextOne, buttonAction);
				if (!isOpen)
					OpenElement();
				isOpen = true;
				return;
			}
			DoPopUp(text, buttonTextOne, buttonTextTwo, buttonAction);
			if (!isOpen)
				OpenElement();
			isOpen = true;
		}

		Queue<ConfBoxQData> Q = new Queue<ConfBoxQData>();

		void Queue(string text, string buttonTextOne, UnityAction buttonAction = null, string buttonTextTwo = null) {
			Q.Enqueue(new ConfBoxQData(text, buttonTextOne, buttonAction));
		}
		
		private void DoPopUp(string text, string buttonText, UnityAction buttonAction) {
			blockingPanel.SetActive(true);
			displayText.text = text;
			buttonTwo.gameObject.SetActive(false);
			var buttonOneRect = buttonOne.GetComponent<RectTransform>();
			var pos = 0;
			buttonOneRect.anchoredPosition = new Vector2(pos, bOnePos.y);
			buttonOne.GetComponentInChildren<Text>().text = buttonText;
			if (buttonAction != null) {
				buttonOne.onClick.AddListener(buttonAction);
			}
			else {
				buttonOne.onClick.AddListener(RealClose);
			}
		}

		private void DoPopUp(string text, string buttonOneText, string buttonTwoText, UnityAction buttonAction = null) {
			blockingPanel.SetActive(true);
			displayText.text = text;
			buttonOne.GetComponent<RectTransform>().anchoredPosition = bOnePos;
			buttonTwo.GetComponent<RectTransform>().anchoredPosition = bTwoPos;
			buttonOne.GetComponentInChildren<Text>().text = buttonOneText;
			buttonTwo.GetComponentInChildren<Text>().text = buttonTwoText;
			buttonOne.onClick.AddListener(RealClose);
			buttonTwo.onClick.AddListener(RealClose);
			if(buttonAction!=null){
				buttonTwo.onClick.AddListener(buttonAction);
			}
			else {
				buttonTwo.onClick.AddListener(DOSOMETHING);
			}
		}

		public override void CloseElement() {
			if (isRunning)
				return;
			buttonOne.onClick.Invoke();
		}

		void RealClose() {
			if(Q.Count > 0) {
				var data = Q.Dequeue();
				RemoveListeners();
				PartOfMultiPage(data.showText, data.butText, data.butAction);
				return;
			}
			isOpen = false;
			RemoveListeners();
			inputManager.ResumePlayer();
			blockingPanel.SetActive(false);
			base.CloseElement();
		}

		private void OnDisable() {
			buttonOne.gameObject.SetActive(true);
			buttonTwo.gameObject.SetActive(true);
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

		public void DoAction<T>(T param, Vector3 m = default(Vector3)) {

		}

		public ControlUIElement GetControlElement() {
			return this;
		}
	}
}

struct ConfBoxQData {
	public ConfBoxQData(string t, string b, UnityAction ba) {
		showText = t;
		butText = b;
		butAction = ba;
	}
	public string showText;
	public string butText;
	public UnityAction butAction;
}