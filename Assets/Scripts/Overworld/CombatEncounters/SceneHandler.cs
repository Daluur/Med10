using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld {
	public class SceneHandler : Singleton<SceneHandler> {

		private Camera OWCam;
		private InputManager inputManager;
		private GameObject eventSystem;
		private GameObject OWCanvas;

		private int currencyToReward = 0;
		private GameObject encounterObject;

		private MapTypes mapType = MapTypes.ANY;
		private DeckData deck;

		// Use this for initialization
		void Start () {
			inputManager = gameObject.GetComponent<InputManager>();
			eventSystem = GameObject.FindGameObjectWithTag(TagConstants.OWEVENTSYSTEM);
			OWCam = Camera.main;
			OWCanvas = GameObject.FindGameObjectWithTag(TagConstants.OWCANVAS);
		}

		public void LoadScene(MapTypes type, int deckID, int currencyReward = 0, GameObject encounterObject = null) {
			AudioHandler.instance.PlayEnterCombat();
			mapType = type;
			deck = DeckHandler.GetDeckFromID(deckID);
			DisableObjectsCombatLoad();
			SceneManager.LoadScene(1,LoadSceneMode.Additive);
			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += EndEncounter;
			currencyToReward = currencyReward;
			this.encounterObject = encounterObject;
		}

		public MapTypes GetMapType() {
			return mapType;
		}

		public DeckData GetDeck() {
			return deck;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			StartCoroutine(WaitForSceneLoad());
		}

		private void EndEncounter(Scene scene) {
			EnabeleObjectsCombatLoad();
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneUnloaded -= EndEncounter;
		}

		private IEnumerator WaitForSceneLoad() {
			yield return new WaitForSeconds(0.3f);
			SceneManager.SetActiveScene(SceneManager.GetSceneByName("NodeScene"));
			inputManager.gameObject.SetActive(false);
		}

		private void DisableObjectsCombatLoad() {
			OWCam.gameObject.SetActive(false);
			eventSystem.SetActive(false);
			OWCanvas.SetActive(false);
		}

		private void EnabeleObjectsCombatLoad() {
			OWCam.gameObject.SetActive(true);
			inputManager.gameObject.SetActive(true);
			eventSystem.SetActive(true);
			OWCanvas.SetActive(true);
		}

		//Add things here if they need to happen when a player wins a battle
		public void Won() {
			AwardCurrency();
			DestroyEncounterObject();
		}

		private void AwardCurrency() {
			CurrencyHandler.AddCurrency(currencyToReward);
		}

		private void DestroyEncounterObject() {
			if (encounterObject != null) {
				Destroy(encounterObject);
			}
		}

	}
}
