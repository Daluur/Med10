using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class InputSubscriber : MonoBehaviour {
		public InputManager inputManager;

		private void Awake() {
			inputManager = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<InputManager>();
		}

		public void Register(IInteractable interactable, KeyCode keyCode) {
			inputManager.Register(interactable, keyCode);
		}
	}
}