using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

namespace Overworld {
	public class InputManager : MonoBehaviour {

		private List<bool> uiMouseLock = new List<bool>();
		private bool isMouseBlocked = false;
		private LayerMask layerMaskPlayer, layerMaskInteractable;
		private List<IInteractable> distributeTo = new List<IInteractable>();
		private Dictionary<KeyCode, List<IInteractable>> registerTo = new Dictionary<KeyCode, List<IInteractable>>();
		private Vector3 playerMoveTo;
		private GameObject evtSystem;

		// Use this for initialization
		void Start () {
			layerMaskPlayer = (1 << LayerMask.NameToLayer(LayerConstants.GROUNDLAYER));
			layerMaskInteractable = ( 1 << LayerMask.NameToLayer(LayerConstants.INTERACTABLELAYER) );
			evtSystem = GameObject.FindGameObjectWithTag(TagConstants.OWEVENTSYSTEM);
		}

		// Update is called once per frame
		void Update () {
			HandleInputs();
		}

		void HandleInputs() {
			foreach (var keyCode in registerTo.Keys) {
				if (Input.GetKeyDown(keyCode)) {
					HandleSpecificKeys(keyCode);
				}
			}
		}

		private void HandleSpecificKeys(KeyCode keyCode) {
			switch (keyCode) {
				case KeyCode.Mouse0:
					if (!isMouseBlocked) {
						var mousePos = Input.mousePosition;
						FillDistributer(keyCode);
						DistributeAction();
						WhatIsHit(mousePos);
						DistributeAction(playerMoveTo);
					}
					break;
				case KeyCode.Escape:
					FillDistributer(keyCode);
					DistributeAction();
					break;
				case KeyCode.B:
					FillDistributer(keyCode);
					DistributeAction();
					break;
				default:
					Debug.Log("No implementation for this key exists");
					break;
			}
		}


		private void WhatIsHit(Vector3 mousePos) {
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 500f, layerMaskPlayer)) {
				playerMoveTo = hit.point;
				distributeTo.Add(GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER).GetComponent<IInteractable>());
			}
			if (Physics.Raycast(ray, out hit, 500f, layerMaskInteractable)) {
				distributeTo.Add(hit.collider.gameObject.GetComponent<IInteractable>());
			}
		}

		private void FillDistributer(KeyCode keyCode) {
			foreach (var interactable in registerTo.Where(element => element.Key == keyCode).FirstOrDefault().Value) {
				distributeTo.Add(interactable);
			}
		}

		private void DistributeAction(Vector3 playerMoveTo) {
			foreach (var interactable in distributeTo) {
				interactable.DoAction(playerMoveTo);
			}
			distributeTo.Clear();
		}

		private void DistributeAction() {
			foreach (var interactable in distributeTo) {
				interactable.DoAction();
			}
			distributeTo.Clear();
		}

		private bool IsKeyListenedFor(KeyCode keyCode) {
			return registerTo.ContainsKey(keyCode);
		}

		public void Register(IInteractable interactable, KeyCode keyCode) {
			if(registerTo.ContainsKey(keyCode))
				registerTo.Where(element => element.Key == keyCode).FirstOrDefault().Value.Add(interactable);
			else{
				var tmp = new List<IInteractable>();
				tmp.Add(interactable);
				registerTo.Add(keyCode, tmp);
			}
		}

		public void BlockMouseUI() {
			uiMouseLock.Add(true);
			isMouseBlocked = true;
		}

		public void UnblockMouseUI() {
			uiMouseLock.Remove(true);
			if (uiMouseLock.Count <= 0)
				isMouseBlocked = false;
			else {
				isMouseBlocked = true;
			}
		}

		public bool GetMouseBlocked() {
			return isMouseBlocked;
		}
	}
}
