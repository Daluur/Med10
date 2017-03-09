using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using UnityEngine.EventSystems;

namespace CombatWorld {
	public class InputManager : MonoBehaviour {

		private LayerMask layerMask;
		Ray ray;
		RaycastHit hit;

		void Start() {
			layerMask = (1 << LayerMask.NameToLayer(LayerConstants.NODELAYER));
		}

		void Update() {
			if (Input.GetMouseButtonDown(0)) {
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 500f, layerMask)) {
					hit.collider.gameObject.GetComponent<Node>().HandleInput();
				}
				/*//Checks to see if the mouse is currently hovering over an UI element. EventSystem.current.IsPointerOverGameObject() returns true if the pointer is over UI.
				else if (!EventSystem.current.IsPointerOverGameObject()) {
					GameController.instance.ClickedNothing();
				}*/
			}
			if (Input.GetMouseButtonDown(1)) {
				GameController.instance.ClickedNothing();
			}
		}
	}
}