using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Overworld;
using UnityEngine;

namespace Overworld {
	public class OpenSettings : MenuUIController {

		void Start () {
			//Register(this, KeyCode.B);
		}

		public void DoAction() {
			if (!isShowing && !isRunning) {
				OpenSettingsTab();
			}
			else if (isShowing && !isRunning) {
				CloseSettings();
			}
		}

		public void OpenSettingsTab() {
			if (isRunning || isShowing)
				return;
			OpenElement(gameObject, size);
			//Register(this, KeyCode.Escape);
		}
		public void CloseSettings() {
			//UnRegister(this, KeyCode.Escape);
			CloseElement(gameObject);
		}

		public void DoAction<T>(T param) {
		}
	}
}