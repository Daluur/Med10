using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class EncounterArea : Encounter {

		private RandomEncounter randomEncounter;
		[HideInInspector]
		public bool onTriggerEnter = true;
		[HideInInspector]
		public bool onTriggerExit = true;

		private void Start() {
			randomEncounter = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<RandomEncounter>();
			if (deckIDs == null || deckIDs.Length == 0) {
				Debug.LogError("This encounter has no decks! " + gameObject.name);
				deckIDs = new int[] { 0 };
			}
		}

		public void DoOnTriggerEnter() {
			onTriggerEnter = true;
			randomEncounter.RandomEncounterOn(type, deckIDs);
		}
		public void DoOnTriggerExit() {
			onTriggerExit = true;
			randomEncounter.RandomEncounterOff();
		}

		public void OnTriggerEnter(Collider other) {
			if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER || !onTriggerEnter) {
				return;
			}
			Debug.Log(onTriggerEnter);
			randomEncounter.RandomEncounterOn(type, deckIDs);
			Debug.Log("Rand encounter on with type: " + type);
		}

		private void OnTriggerExit(Collider other) {
			if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER || !onTriggerExit) {
				return;
			}
			randomEncounter.RandomEncounterOff();
		}

		protected override void OnMouseEnter() {
		}

		protected override void OnMouseExit() {
		}
	}
}
