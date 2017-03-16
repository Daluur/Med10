using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class CurrencyPickup : MonoBehaviour {
		public int amountOfCurrency = 5;
		private void OnTriggerEnter(Collider other) {
			if (other.transform.parent.tag != TagConstants.OVERWORLDPLAYER) {
				return;
			}
			CurrencyHandler.AddCurrency(amountOfCurrency);
			Destroy(gameObject);
		}
	}
}
