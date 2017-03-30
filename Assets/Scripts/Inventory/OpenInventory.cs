using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Overworld;
using UnityEngine;

namespace Overworld {
	public class OpenInventory : ControlUIElement, IInteractable {

		void Start () {
			Register(this, KeyCode.B);
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
			CloseElement();
		}

		public void DoAction<T>(T param) {
		}

		public ControlUIElement GetControlElement() {
			return this;
		}
	}
}
