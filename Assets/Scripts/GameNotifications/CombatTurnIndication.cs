using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CombatWorld {
	public class CombatTurnIndication : Singleton<CombatTurnIndication> {
		public string playerTurn = "Your turn", enemyTurn = "Enemy turn";
		public float timeBeforeDisappearing = 2f;
		private Text text;
		private void Start() {
			text = GetComponent<Text>();
			gameObject.SetActive(false);
		}

		public void PlayerTurn() {
			text.text = playerTurn;
			gameObject.SetActive(true);
			StartCoroutine(HideText());
		}

		public void EnemyTurn() {
			text.text = enemyTurn;
			gameObject.SetActive(true);
			StartCoroutine(HideText());
		}

		private IEnumerator HideText() {
			yield return new WaitForSeconds(timeBeforeDisappearing);
			gameObject.SetActive(false);
		}
	}
}
