﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld.Shops {
	public class Shop : ControlUIElement, IInteractable {

		public GameObject buttonObj;
		public OpenInventory inventoryPanel;

		private List<ShopButton> createdButtons = new List<ShopButton>();

		private List<int> unitsToShow = new List<int>();
		private Inventory inventory;
		private ItemDatabase database;

		void Start() {
			Register(this, KeyCode.Escape);
			inventory = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<Inventory>();
			database = inventory.GetDatabase();
			CreateButtons();
		}

		void CreateShop() {
			GetUnits();
			EnableUnlockedUnits();
			//DestroyOldButtons();
			//CreateNewButtons();
		}

		void EnableUnlockedUnits() {
			foreach (ShopButton button in createdButtons) {
				if (unitsToShow.Contains(button.ID)) {
					button.IsUnlocked();
					button.CheckCanAfford(CurrencyHandler.GetCurrentGold());
				}
				else {
					button.NotUnlocked();
				}
			}
		}
		
		/*void DestroyOldButtons() {
			foreach (GameObject go in createdButtons) {
				Destroy(go);
			}
			createdButtons.Clear();
		}

		void CreateNewButtons() {
			foreach (int i in unitsToShow) {
				GameObject go = Instantiate(buttonObj, transform) as GameObject;
				go.GetComponent<ShopButton>().Setup(AddItem, database.FetchItemByID(i));
				createdButtons.Add(go);
			}
		}*/

		void CreateButtons() {
			foreach (Item item in database.GetAllItems()) {
				GameObject go = Instantiate(buttonObj, transform) as GameObject;
				createdButtons.Add(go.GetComponent<ShopButton>().Setup(AddItem, item));
			}
		}

		void GetUnits() {
			unitsToShow = UnlockHandler.Instance.GetUnlockedUnits();
		}

		private void AddItem(int unitID, int price) {
			if (!inventory.HasInventorySpace()) {
				GameNotificationsSystem.instance.DisplayMessage(GameNotificationConstants.NOTENOUGHINVENTORYSPACE);
				return;
			}
			if (!CurrencyHandler.RemoveCurrency(price)) {
				return;
			}
			EnableUnlockedUnits();
			inventory.AddItem(unitID);
		}

		public void OpenMenu() {
			CreateShop();
			OpenElement(gameObject, size, true);
			inventoryPanel.OpenTheInventory();
		}

		public void CloseMenu() {
			if (isRunning || !isShowing)
				return;
			CloseElement(gameObject);
			if (inventoryPanel.isShowing) {
				inventoryPanel.CloseInventory();
			}
		}

		public void DoAction() {
			CloseMenu();
		}

		public void DoAction<T>(T param) {
		}
	}
}