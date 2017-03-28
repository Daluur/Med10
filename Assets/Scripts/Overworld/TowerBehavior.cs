using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using Overworld.Shops;

namespace Overworld {

	public class TowerBehavior : ContextInteraction, IInteractable {

		Shop shop;

		void Start () {
			if (playerOW == null) {
				playerOW = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER);
			}
			shop = GameObject.FindGameObjectWithTag("OWShop").GetComponent<Shop>();
			Register(this, KeyCode.Mouse0);
		}

		private void OpenMenu() {
			shop.OpenMenu();
		}

		public override void PerformClosenessAction() {
			OpenMenu();
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