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
			OpenElement(gameObject, size, true);
		}
		public void CloseInventory() {
			CloseElement(gameObject);
		}

		public void DoAction<T>(T param) {
		}
	}
}
