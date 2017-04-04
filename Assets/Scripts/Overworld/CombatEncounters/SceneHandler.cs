using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld {
	public class SceneHandler : Singleton<SceneHandler> {

		public Texture2D loadingTexture;

		private Camera OWCam;
		private InputManager inputManager;
		private GameObject eventSystem;
		private GameObject OWCanvas;

		private int currencyToReward = 0;
		private GameObject encounterObject;

		private MapTypes mapType = MapTypes.ANY;
		private DeckData deck;

		private Coroutine loading;
		private AsyncOperation async;

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
			loading = StartCoroutine(LoadingScene());
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

		private IEnumerator LoadingScene() {
			async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
			yield return async;
		}

		private void OnGUI() {
			if(async!=null)
			GUI.DrawTexture(new Rect(0, 0, (100 * async.progress)*Screen.width/100, Screen.height), loadingTexture);
		}

		//Add things here if they need to happen when a player wins a battle
		public void Won() {
			AwardCurrency();
			ProcessEncounteredObject();
		}

		private void AwardCurrency() {
			CurrencyHandler.AddCurrency(currencyToReward);
		}

		private void ProcessEncounteredObject() {
			Destroy(encounterObject);
		}

	}
}
