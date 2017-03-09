using System.Collections;
using System.Collections.Generic;
using Overworld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Overworld {
	public class SceneHandler : MonoBehaviour {

		private Camera OWCam;
		private InputManager inputManager;
		private GameObject eventSystem;

		// Use this for initialization
		void Start () {
			inputManager = gameObject.GetComponent<InputManager>();
			eventSystem = GameObject.FindGameObjectWithTag(TagConstants.OWEVENTSYSTEM);
			OWCam = Camera.main;
		}

		public void LoadScene(int type) {
			DisableObjectsCombatLoad();
			SceneManager.LoadScene(1,LoadSceneMode.Additive);
			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += EndEncounter;
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
		}

		private void EnabeleObjectsCombatLoad() {
			OWCam.gameObject.SetActive(true);
			inputManager.gameObject.SetActive(true);
			eventSystem.SetActive(true);
		}


	}
}
