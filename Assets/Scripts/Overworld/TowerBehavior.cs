using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

namespace Overworld {

	public class TowerBehavior : InputSubscriber, IInteractable {

		public GameObject contextMenu;
		public GameObject playerOW;
		public float distanceToOpen = 5f;

		public GameObject[] units;
		public int[] amountOfUnits;
		private bool isRunning = false;
		private bool meClicked;


		//public Unit[] containingUnits;

		//TODO: Add custom editor to the tower behaviour so that it is possible to write in editor how many units of each the tower should have
		public Dictionary<GameObject, int>[] containing;

		// Use this for initialization
		void Start () {

			Register(this, KeyCode.Mouse0);

			if (contextMenu == null) {
				contextMenu = GameObject.FindGameObjectWithTag(TagConstants.CONTEXTUNITMENU);
			}

			if (playerOW == null) {
				playerOW = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER);
			}
		}

		// Update is called once per frame
		void Update () {

		}

		private void CheckDistance() {
			meClicked = true;
			if (!isRunning)
				StartCoroutine(IsCloseEnough());
			//if(CanOpenMenu())
			//	OpenMenu();
		}

		private void OpenMenu() {
			contextMenu.GetComponent<ContextPopUp>().DisplayMenu(units);
		}

		private bool CanOpenMenu() {
			if (Vector3.Distance(playerOW.transform.position, gameObject.transform.position) < distanceToOpen)
				return true;
			return false;
		}

		private IEnumerator IsCloseEnough() {
			isRunning = true;
			while (meClicked) {
				if (CanOpenMenu()) {
					OpenMenu();
					isRunning = false;
					yield break;
				}
				yield return new WaitForSeconds(0.3f);
			}
			isRunning = false;
			yield return null;
		}

		public void DoAction() {
		}

		public void DoAction<T>(T param) {
			CheckDistance();
		}
	}


}