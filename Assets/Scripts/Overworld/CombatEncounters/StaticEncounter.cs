using System;
using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld {
	public class StaticEncounter : Encounter, IInteractable {

		// Use this for initialization
		void Start () {
			Register(this, KeyCode.Mouse0);
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
			LoadCombat();
		}

		public override void LoadCombat() {
			SceneHandler.instance.LoadScene(0, deckIDs[UnityEngine.Random.Range(0,deckIDs.Length)], currencyForWinning, gameObject);
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

		protected override void OnMouseEnter() {
			base.OnMouseEnter();
			Tooltip.instance.Activate(this);
		}

		protected override void OnMouseExit() {
			base.OnMouseExit();
			Tooltip.instance.Deactivate();
		}

	}
}
