using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

namespace Overworld {

	public class TowerBehavior : MonoBehaviour {

		public GameObject contextMenu;
		public GameObject playerOW;
		public float distanceToOpen = 5f;

		public GameObject[] units;

		public Unit[] containingUnits;

		// Use this for initialization
		void Start () {
			if (contextMenu == null) {
				contextMenu = GameObject.FindGameObjectWithTag(TagConstants.CONTEXTUNITMENU);
			}

			if (playerOW == null) {
				playerOW = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER);
			}

			for(int i=0;i<units.Length;i++) {
				containingUnits[i] = units[i].GetComponent<Unit>();
			}



		}


		// Update is called once per frame
		void Update () {

		}

		private void OnMouseDown() {
			Debug.Log("Here");
		}

		private void OpenMenu() {
		//	if(CanOpenMenu())
			//	contextMenu.GetComponent<ContextPopUp>().DisplayMenu();
		}

		private bool CanOpenMenu() {
			if (Vector3.Distance(playerOW.transform.position, gameObject.transform.position) < distanceToOpen)
				return true;
			return false;
		}


	}


}