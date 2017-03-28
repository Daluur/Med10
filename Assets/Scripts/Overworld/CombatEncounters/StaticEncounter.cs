using System;
using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld {
	public class StaticEncounter : ContextInteraction, IInteractable {

		public int currencyForWinning = 25;

		public MapTypes type = MapTypes.ANY;

		public int[] deckIDs = new int[1] { 0 };

		private SceneHandler sceneHandler;

		// Use this for initialization
		void Start () {
			Register(this, KeyCode.Mouse0);
			sceneHandler = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<SceneHandler>();
			playerOW = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER);
		}

		// Update is called once per frame
		void Update () {

		}


		public override void PerformClosenessAction() {
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
			meClicked = true;
			CheckDistance();
		}

		public ControlUIElement GetControlElement() {
			return null;
		}
	}
}
