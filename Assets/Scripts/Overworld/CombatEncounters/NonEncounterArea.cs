using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class NonEncounterArea : MonoBehaviour {
		private RandomEncounter randomEncounter;

		private void Start() {
			randomEncounter = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<RandomEncounter>();
		}

		private void OnTriggerEnter(Collider other) {
			if (other.tag != TagConstants.OVERWORLDPLAYER) {
				return;
			}
			randomEncounter.RandomEncounterOff();
			Debug.Log("Rand encounter off");
		}
	}
}