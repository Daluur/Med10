using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using UnityEngine.EventSystems;

namespace CombatWorld {
	public class InputManager : MonoBehaviour {

		void Update() {
			if (Input.GetMouseButtonDown(1)) {
				GameController.instance.ClickedNothing();
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				GameController.instance.TryEndTurn();
			}
		}
	}
}