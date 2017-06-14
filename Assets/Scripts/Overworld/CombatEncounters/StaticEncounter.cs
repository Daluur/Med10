using System.Collections;
using System.Collections.Generic;
using Overworld.Shops;
using SimplDynTut;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld {
	public class StaticEncounter : Encounter, IInteractable {

		public int StaticEncounterID = -1;
		public Transform[] spawnPoints;
		private int currentPos;
		public Teleporter teleporterPad;
		public int[] unitsToUnlock;

		// Use this for initialization
		void Start () {
			currentPos = 0;
			if (SaveLoadHandler.Instance.AmIDefeated(StaticEncounterID)) {
				Beaten(true);
				return;
			}
			if (deckIDs == null || deckIDs.Length == 0) {
				Debug.LogError("This encounter has no decks! " + gameObject.name);
				deckIDs = new int[] { 0 };
			}
			if(StaticEncounterID == -1) {
				Debug.LogError("This encounter has no ID! " + gameObject.name);
			}
			Register(this, KeyCode.Mouse0);
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

		public void DoAction<T>(T param, Vector3 m = default(Vector3)) {
			if(!OverworldTriggers.instance.ChangedDeckBeforeEnteringCombat()) {
				inputManager.StopPlayer();
				return;
			}
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

		public void MoveToNewSpawnPos() {
			if(spawnPoints.Length == 0) {
				Debug.LogError("This encounter has no spawnpoints!" + gameObject.name);
				return;
			}
			int newPos;
			do {
				newPos = Random.Range(0, spawnPoints.Length);
			} while (newPos == currentPos);
			currentPos = newPos;
			transform.position = spawnPoints[currentPos].position;
		}

		public void Beaten(bool saveFix = false) {
			//TutorialHandler.instance.PortalIsOpen();
			foreach (int id in unitsToUnlock) {
				UnlockHandler.Instance.UnlockUnitByID(id);
			}

			teleporterPad.Activate();
			
			Destroy(gameObject);
		}
	}
}
