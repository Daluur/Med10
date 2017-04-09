using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class OpenQuest : ControlUIElement, IInteractable {

		void Start () {
			Register(this, KeyCode.Q);
		}

		public void Button() {
			inputManager.FakeInput(KeyCode.Q);
		}

		public void DoAction() {
			if (!isShowing && !isRunning) {
				OpenQuestLog();
			}
			else if (isShowing && !isRunning) {
				CloseQuest();
			}
		}

		public void OpenQuestLog() {
			AudioHandler.instance.PlayOpenWindow();
			OpenElement();
		}
		public void CloseQuest() {
			AudioHandler.instance.PlayCloseWindow();
			CloseElement();
		}

		public void DoAction<T>(T param, Vector3 m = default(Vector3)) {
		}

		public ControlUIElement GetControlElement() {
			return this;
		}
	}
}