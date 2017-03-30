using System;
using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld {
	public class StaticEncounter : Encounter, IInteractable {
		private SceneHandler sceneHandler;

		// Use this for initialization
		void Start () {
			Register(this, KeyCode.Mouse0);
			sceneHandler = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<SceneHandler>();
			if (deckIDs == null || deckIDs.Length == 0) {
				Debug.LogError("This encounter has no decks! " + gameObject.name);
				deckIDs = new int[] { 0 };
			}
		}

		// Update is called once per frame
		void Update () {

		}


		public override void PerformClosenessAction() {
			hasGeneralConfirmationBox = false;
			sceneHandler.LoadScene(0, deckIDs[UnityEngine.Random.Range(0,deckIDs.Length)], currencyForWinning, gameObject);

		}

		public static void LoadCombatScene(int typeOfMap) {

		}

		private IEnumerator WaitForSceneLoad() {
			yield return new WaitForSeconds(0.3f);
			SceneManager.SetActiveScene(SceneManager.GetSceneByName("NodeScene"));
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

		}

		void EndEncounter(Scene scene) {

		}

		public void DoAction() {
			meClicked = false;
		}

		public void DoAction<T>(T param) {
			if(hasGeneralConfirmationBox)
				return;
			meClicked = true;
			CheckDistance();
		}

		public ControlUIElement GetControlElement() {
			return null;
		}
	}
}
