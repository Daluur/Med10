using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using Overworld.Shops;

namespace Overworld {

	public class TowerBehavior : ContextInteraction, IInteractable {
		Shop shop;

		void Start () {
			shop = GameObject.FindGameObjectWithTag("OWShop").GetComponent<Shop>();
			Register(this, KeyCode.Mouse0);
			//UnityAction call = () => contextInteraction.PerformClosenessAction();
		}

		private void OpenMenu() {
			if (TutorialHandler.instance.firstShop) {
				TutorialHandler.instance.firstShop = false;
				GeneralConfirmationBox.instance.ShowPopUp ("Spending your crystals here summons a unit to your inventory. Only units that you have the recipe for can be summoned.", "Okay");
			}

			shop.OpenMenu();
		}

		public override void PerformClosenessAction() {
			hasGeneralConfirmationBox = false;
			OpenMenu();
		}

		public void DoAction() {
			meClicked = false;
		}

		public void DoAction<T>(T param, Vector3 m = default(Vector3)) {
			if(hasGeneralConfirmationBox)
				return;
			meClicked = true;
			CheckDistance();
		}

		public ControlUIElement GetControlElement() {
			return null;
		}
	}
}