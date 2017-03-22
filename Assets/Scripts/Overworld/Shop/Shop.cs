using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld.Shop {
	public class Shop : ControlUIElement, IInteractable {

		public GameObject buttonObj;

		private List<GameObject> createdButtons = new List<GameObject>();

		private List<int> unitsToShow = new List<int>();
		private Inventory inventory;
		private ItemDatabase database;

		void Start() {
			inventory = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<Inventory>();
			database = inventory.GetDatabase();
		}

		void CreateShop() {
			GetUnits();
			DestroyOldButtons();
			CreateNewButtons();
		}

		void DestroyOldButtons() {
			foreach (GameObject go in createdButtons) {
				Destroy(go);
			}
		}

		void CreateNewButtons() {
			foreach (int i in unitsToShow) {
				GameObject go = Instantiate(buttonObj, transform) as GameObject;

			}
		}

		void GetUnits() {
			unitsToShow = UnlockHandler.Instance.GetUnlockedUnits();
		}

		bool RebuildMenu() {
			return unitsToShow.Count != UnlockHandler.Instance.UnlockedCount();
		}

		private void AddItem(int unitID, int price) {
			if (!CurrencyHandler.RemoveCurrency(price)) {
				return;
			}
			inventory.AddItem(unitID);
		}

		public void OpenMenu() {
			if (RebuildMenu()) {
				CreateShop();
			}
			OpenElement(gameObject, size, true);
		}

		public void CloseMenu() {
			if (isRunning || !isShowing)
				return;
			CloseElement(gameObject);
		}

		public void DoAction() {
			CloseMenu();
		}

		public void DoAction<T>(T param) {
		}
	}
}