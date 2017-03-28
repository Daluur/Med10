using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class OpenQuest : ControlUIElement, IInteractable {

		void Start () {
			Register(this, KeyCode.Q);
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
			OpenElement();
		}
		public void CloseQuest() {
			CloseElement();
		}

		public void DoAction<T>(T param) {
		}

		public ControlUIElement GetControlElement() {
			return this;
		}
	}
}