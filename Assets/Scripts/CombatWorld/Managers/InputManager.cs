using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using UnityEngine.EventSystems;
using Overworld;

namespace CombatWorld {
	public class InputManager : MonoBehaviour {

		InGameMenu menu;

		void Start() {
			menu = FindObjectOfType<InGameMenu>();
		}

		void Update() {
			if (Input.GetMouseButtonDown(1)) {
				GameController.instance.ClickedNothing();
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				GameController.instance.TryEndTurn();
			}

			if (Input.GetKeyDown(KeyCode.Escape)) {
				if (GeneralConfirmationBox.instance != null && GeneralConfirmationBox.instance.IsOpen) {
					GeneralConfirmationBox.instance.CloseElement();
				}
				else if(menu != null) {
					if (!menu.isRunning) {
						if (menu.isShowing) {
							menu.CloseMenu();
						}
						else {
							menu.OpenElement();
						}
					}
				}
			}
		}
	}
}