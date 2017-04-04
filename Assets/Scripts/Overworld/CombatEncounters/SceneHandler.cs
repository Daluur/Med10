using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld {
	public class SceneHandler : Singleton<SceneHandler> {

		public Texture2D loadingTexture;
		private bool isLoading;

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
			StartCoroutine(LoadingScene());
			deck = DeckHandler.GetDeckFromID(deckID);
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
			StartCoroutine(FadeIn());
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneUnloaded -= EndEncounter;
		}

		private IEnumerator FadeIn() {
			FadingLoadingScreen.instance.StartFadeIn();
			while (FadingLoadingScreen.instance.isFading) {
				yield return new WaitForSeconds(0.1f);
			}
			yield return null;
		}

		private IEnumerator WaitForSceneLoad() {
			yield return new WaitForSeconds(0.3f);
			SceneManager.SetActiveScene(SceneManager.GetSceneByName("NodeScene"));
		}

		private void DisableObjectsCombatLoad() {
			OWCam.gameObject.SetActive(false);
			OWCanvas.SetActive(false);
			inputManager.gameObject.SetActive(false);
		}

		private void EnabeleObjectsCombatLoad() {
			OWCam.gameObject.SetActive(true);
			eventSystem.SetActive(true);
			OWCanvas.SetActive(true);
			inputManager.gameObject.SetActive(true);
		}

		private IEnumerator LoadingScene() {
			eventSystem.SetActive(false);
			FadingLoadingScreen.instance.StartFadeOut();
			while (FadingLoadingScreen.instance.isFading) {
				yield return new WaitForSeconds(0.1f);
			}
			async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
			yield return new WaitForSeconds(0.5f);
			DisableObjectsCombatLoad();
			isLoading = false;
			yield return new WaitForSeconds(0.4f);
			inputManager.gameObject.SetActive(true);
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
