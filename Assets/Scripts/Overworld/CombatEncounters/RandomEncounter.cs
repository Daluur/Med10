using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Overworld {
	public class RandomEncounter : MonoBehaviour {
		private Vector3 lastPos = Vector3.zero, currPos;
		private GameObject player;
		private bool randEncounter = false;
		private float currentChance = 0f;
		public float startChance = 0f;
		private float maximumChance = 1.1f;
		public float chanceIncrease = 0.002f;
		private int type = 0;
		private SceneHandler sceneHandler;
		private bool isRunning = false;
		private Coroutine encounter;
		public bool printChances = false;

		private void Start() {
			player = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER);
			sceneHandler = GetComponent<SceneHandler>();
		}

		private IEnumerator CheckForEncounter() {
			isRunning = true;
			while (randEncounter) {
				currPos = player.transform.position;
				yield return new WaitForSeconds(0.4f);
				ChanceOfEncounter();
				lastPos = currPos;
			}
			isRunning = false;
		}

		private bool ChanceOfEncounter() {
			if (currPos == lastPos){
				return false;
			}

			var randGenerator = Random.Range(startChance, maximumChance);
			if (printChances) {
				Debug.Log("Current chance: " + currentChance + " , current roll: " + randGenerator);
			}

			if (currentChance >= randGenerator) {
				currentChance = startChance;
				sceneHandler.LoadScene(type);
				return true;
			}
			else {
				currentChance += chanceIncrease;
				return false;
			}
		}

		public void RandomEncounterOn(int type) {
			randEncounter = true;
			this.type = type;
			if(gameObject.activeInHierarchy && !isRunning)
				encounter = StartCoroutine(CheckForEncounter());

		}

		public void RandomEncounterOff() {
			randEncounter = false;
		}

		private void OnEnable() {
			if (randEncounter)
				encounter = StartCoroutine(CheckForEncounter());
		}
	}
}
