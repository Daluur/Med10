using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class InputSubscriber : MonoBehaviour {
		[HideInInspector]
		public InputManager inputManager;
		[HideInInspector]
		public bool open;

		protected virtual void Awake() {
			inputManager = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<InputManager>();
		}

		public void Register(IInteractable interactable, KeyCode keyCode) {
			inputManager.Register(interactable, keyCode);
		}

		public void Register(IInteractable interactable) {
			inputManager.Register(interactable);
		}

		public void UnRegister(IInteractable interactable, KeyCode keyCode) {
			inputManager.UnRegister(interactable, keyCode);
		}

		public void UnRegister(IInteractable interactable) {
			inputManager.UnRegister(interactable);
		}
	}
}