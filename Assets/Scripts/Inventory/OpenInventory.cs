using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Overworld;
using UnityEngine;

namespace Overworld {
	public class OpenInventory : ControlUIElement, IInteractable {

		void Start () {
			//Register (this, KeyCode.B);
			Register (this, KeyCode.I);
		}

		public void Button() {
			inputManager.FakeInput(KeyCode.I);
		}

		public void DoAction() {
			if (!isShowing && !isRunning) {
				OpenTheInventory();
			}
			else if (isShowing && !isRunning) {
				CloseInventory();
			}
		}

		public void OpenTheInventory() {
			if (isRunning || isShowing)
				return;
			AudioHandler.instance.PlayOpenWindow();
			OpenElement();
		}
		public void CloseInventory() {
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
