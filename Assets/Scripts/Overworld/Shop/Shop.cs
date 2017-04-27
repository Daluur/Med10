using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld.Shops {
	public class Shop : ControlUIElement, IInteractable {

		public GameObject buttonObj;
		public OpenInventory inventoryPanel;
		public RectTransform parent;

		private List<ShopButton> createdButtons = new List<ShopButton>();

		private List<int> unitsToShow = new List<int>();
		private Inventory inventory;
		private ItemDatabase database;

		void Start() {
			Register(this, KeyCode.Q);
			inventory = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<Inventory>();
			database = inventory.GetDatabase();
			float num = (database.GetAllItems().Count * 50); // 50 is the height of the buttons. Is hardcoded for now.
			parent.sizeDelta = new Vector2(parent.sizeDelta.x, num);
			parent.anchoredPosition = new Vector2(parent.anchoredPosition.x, -(num / 2));
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
				GameObject go = Instantiate(buttonObj, parent) as GameObject;
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
			AudioHandler.instance.PurchaseSound();
			EnableUnlockedUnits();
			inventory.AddItem(unitID);
			if (TutorialHandler.instance.firstBuy) {
				TutorialHandler.instance.firstBuy = false;
				//GeneralConfirmationBox.instance.ShowPopUp ("You are able to carry 12 units at a time.\nClose the shop by pressing the X icon - or close all windows by pressing ESC.", "Okay");
				GeneralConfirmationBox.instance.ShowPopUp("test1", "next", () => GeneralConfirmationBox.instance.ShowPopUp("test2", "Next", () => GeneralConfirmationBox.instance.ShowPopUp("test3", "Close")));
			}
		}

		public void OpenMenu() {
			if (isRunning || isShowing)
				return;
			AudioHandler.instance.PlayOpenShop();
			CreateShop();
			OpenElement();
			inventoryPanel.OpenTheInventory();
		}

		public void CloseMenu() {
			if (isRunning || !isShowing)
				return;
			CloseElement();
		}

		public void DoAction() {
			CloseMenu();
		}

		public void DoAction<T>(T param, Vector3 m = default(Vector3)) {
		}

		public ControlUIElement GetControlElement() {
			return this;
		}
	}
}