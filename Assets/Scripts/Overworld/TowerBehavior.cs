using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

namespace Overworld {

	public class TowerBehavior : ContextInteraction, IInteractable {

		public GameObject contextMenu;

		public GameObject[] units;
		public int[] amountOfUnits;


		//public Unit[] containingUnits;

		//TODO: Add custom editor to the tower behaviour so that it is possible to write in editor how many units of each the tower should have
		public Dictionary<GameObject, int>[] containing;

		// Use this for initialization
		void Start () {

			Register(this, KeyCode.Mouse0);

			if (contextMenu == null) {
				contextMenu = GameObject.FindGameObjectWithTag(TagConstants.CONTEXTUNITMENU);
			}
		}

		// Update is called once per frame
		void Update () {

		}



		private void OpenMenu() {
			contextMenu.GetComponent<ContextPopUp>().DisplayMenu(units);
		}

		//private bool CanOpenMenu() {

//		}

		public override void PerformClosenessAction() {
			OpenMenu();
		}

		public void DoAction() {
			meClicked = false;
		}

		public void DoAction<T>(T param) {
			meClicked = true;
			CheckDistance();
//CheckDistance();
		}
	}


}