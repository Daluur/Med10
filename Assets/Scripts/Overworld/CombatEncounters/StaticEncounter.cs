using System;
using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld {
	public class StaticEncounter : ContextInteraction, IInteractable {


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
			sceneHandler.LoadScene(0);

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
