using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class EncounterArea : MonoBehaviour {

		private RandomEncounter randomEncounter;
		public MapTypes type = MapTypes.ANY;
		public int[] deckIDs = new int[1] { 0 };

		private void Start() {
			randomEncounter = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<RandomEncounter>();
			if (deckIDs == null || deckIDs.Length == 0) {
				Debug.LogError("This encounter has no decks! " + gameObject.name);
				deckIDs = new int[] { 0 };
			}
		}

		public void OnTriggerEnter(Collider other) {
			if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
				return;
			}
			randomEncounter.RandomEncounterOn(type, deckIDs);
			Debug.Log("Rand encounter on with type: " + type);
		}

		private void OnTriggerExit(Collider other) {
			if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
				return;
			}
			randomEncounter.RandomEncounterOff();
		}
	}
}
