using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Overworld;
using UnityEngine;

namespace Overworld {
	public class OpenInventory : ControlUIElement, IInteractable {

		void Start () {
			Register(this,KeyCode.B);
		}

		public void DoAction() {
			if (!isShowing && !isRunning) {
				OpenTheInventory();
			}
			else if (isShowing && !isRunning) {
				CloseInventory();
			}
		}

		private void OpenTheInventory() {
			OpenElement(gameObject, size, true);
		}
		private void CloseInventory() {
			CloseElement(gameObject);
		}

		public void DoAction<T>(T param) {
		}
	}
}
