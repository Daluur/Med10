using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overworld {
	public class CurrencyHandler : MonoBehaviour {

		private static Text amountOfCurrencyUI;
		private static int amountOfCurrency;

		private void Start() {
			//TODO: Find how much currency I have, from save game
			amountOfCurrencyUI = GetComponent<Text>();
		}

		public static void AddCurrency(int amountToAdd) {
			amountOfCurrency += amountToAdd;
			UpdateCurrencyUI();
		}

		public static bool RemoveCurrency(int amountToRemove) {
			if (amountToRemove > amountOfCurrency) {
				GameNotificationsSystem.instance.DisplayMessage(GameNotificationConstants.NOTENOUGHGOLD);
				return false;
			}
			amountOfCurrency -= amountToRemove;
			UpdateCurrencyUI();
			return true;
		}

		private static void UpdateCurrencyUI() {
			amountOfCurrencyUI.text = amountOfCurrency.ToString();
		}

		public static int GetCurrentGold() {
			return amountOfCurrency;
		}

	}
}
