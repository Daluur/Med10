using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class EncounterArea : MonoBehaviour {
		private RandomEncounter randomEncounter;
		public int type = 0;
		private void Start() {
			randomEncounter = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<RandomEncounter>();
		}

		public void OnTriggerEnter(Collider other) {
			if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
				return;
			}
			randomEncounter.RandomEncounterOn(type);
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
