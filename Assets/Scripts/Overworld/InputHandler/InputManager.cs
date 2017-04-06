using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnityEngine;

namespace Overworld {
	public class InputManager : MonoBehaviour {

		private List<bool> uiMouseLock = new List<bool>();
		private bool isMouseBlocked = false;
		private LayerMask layerMaskPlayer, layerMaskInteractable;
		private List<IInteractable> distributeTo = new List<IInteractable>();
		private Dictionary<KeyCode, List<IInteractable>> registerTo = new Dictionary<KeyCode, List<IInteractable>>();
		private List<IInteractable> registerNoKeycode = new List<IInteractable>();
		private Vector3 playerMoveTo;
		private PlayerMovementOW player;
		private IInteractable playerInteractable;
		[HideInInspector]
		public bool inGameMenuOpen = false;

		// Use this for initialization
		void Start () {
			layerMaskPlayer = (1 << LayerMask.NameToLayer(LayerConstants.GROUNDLAYER));
			layerMaskInteractable = ( 1 << LayerMask.NameToLayer(LayerConstants.INTERACTABLELAYER) );
			player = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER).GetComponent<PlayerMovementOW>();
			playerInteractable = player.gameObject.GetComponent<IInteractable>();
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
			if(inGameMenuOpen && !keyCode.Equals(KeyCode.Escape)){
				return;
			}
			switch (keyCode) {
				case KeyCode.Mouse0:
					var mousePos = Input.mousePosition;
					FillDistributer(keyCode);
					DistributeAction();
					WhatIsHit(mousePos);
					DistributeAction(playerMoveTo);
					break;
				case KeyCode.Escape:
					EscapeBehaviour();
					break;
				case KeyCode.B:
					FillDistributer(keyCode);
					DistributeAction();
					break;
				case KeyCode.I:
					FillDistributer(keyCode);
					DistributeAction();
					break;
				case KeyCode.Q:
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
			if (Physics.Raycast(ray, out hit, 500f, layerMaskInteractable)) {
				if (!isMouseBlocked) {
					playerMoveTo = hit.transform.position;
					distributeTo.Add(playerInteractable);
				}
				distributeTo.AddRange(hit.collider.gameObject.GetComponents<IInteractable>());
				return;
			}
			if (!isMouseBlocked) {
				if (Physics.Raycast(ray, out hit, 500f, layerMaskPlayer)) {
					playerMoveTo = hit.point;
					distributeTo.Add(playerInteractable);
				}
			}

		}

		private void FillDistributer(KeyCode keyCode) {
			foreach (var interactable in registerTo.Where(element => element.Key == keyCode).FirstOrDefault().Value) {
				distributeTo.Add(interactable);
			}
		}

		private void EscapeBehaviour() {
			var closedSomething = false;
			foreach (var listIInteractable in registerTo.Values) {
				foreach (var iinteractable in listIInteractable) {
					if (iinteractable.GetControlElement() != null && iinteractable.GetControlElement().open) {
						iinteractable.GetControlElement().CloseElement();
						closedSomething = true;
					}
				}
			}

			foreach (var iinteractable in registerNoKeycode) {
				if (iinteractable.GetControlElement() != null && iinteractable.GetControlElement().open) {
					iinteractable.GetControlElement().CloseElement();
					closedSomething = true;
				}
			}

			if(closedSomething) {
				inGameMenuOpen = false;
				return;
			}
			inGameMenuOpen = true;
			FillDistributer(KeyCode.Escape);
			DistributeAction();
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

		public void UnRegister(IInteractable interactable, KeyCode keyCode) {
			if (registerTo.ContainsKey(keyCode)) {
				if (registerTo[keyCode].Contains(interactable)) {
					registerTo[keyCode].Remove(interactable);
				}
				else {
					Debug.LogWarning("You are trying to unregister an interacteble that was not registered!" + interactable.GetType() + " key: " + keyCode);
				}
			}
			else {
				Debug.LogWarning("you are trying to unregister from a keycode that has not been registered.");
			}
		}

		public void Register(IInteractable interactable) {
			registerNoKeycode.Add(interactable);
		}

		public void UnRegister(IInteractable interactable) {
			registerNoKeycode.Add(interactable);
		}

		public void BlockMouseUI() {
			uiMouseLock.Add(true);
			StopPlayer();
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

		public void StopPlayer() {
			player.TemporaryStop();
		}
		public void ResumePlayer() {
			player.ResumeFromTemporaryStop();
		}
	}
}
