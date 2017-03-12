using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld
{
	public class SummonHandler : Singleton<SummonHandler>
	{
		public SummonData[] data;
		public Button[] buttons;
		public Text summonPointText;
		int currentlySelectedToSummon = 0;

		int summonPoints = 67;

		private void Start() {
			UpdateButtonsAndText();
		}

		public void SummonUnit(SummonNode node)
		{
			SpendPoints(data[currentlySelectedToSummon].Cost);
			GameObject unit = Instantiate(data[currentlySelectedToSummon].Unit, node.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
			unit.GetComponent<Unit>().SpawnEntity(node, Team.Player,data[currentlySelectedToSummon].data);
		}

		public void SummonButtonPressed(int i)
		{
			currentlySelectedToSummon = i;
			GameController.instance.HighlightSummonNodes();
		}

		public void SetCurrentSelectedToSummon(int i) {
			currentlySelectedToSummon = i;
		}

		void SpendPoints(int amount) {
			summonPoints -= amount;
			UpdateButtonsAndText();
		}

		public void GivePoints(int amount) {
			summonPoints += amount;
			UpdateButtonsAndText();
		}

		void UpdateButtonsAndText() {
			for (int i = 0; i < data.Length; i++) {
				if (data[i].Cost > summonPoints) {
					buttons[i].interactable = false;
				}
				else {
					buttons[i].interactable = true;
				}
			}
			summonPointText.text = "Summon points: " + summonPoints;
		}

		public bool HasPointsToSummon() {
			for (int i = 0; i < data.Length; i++) {
				if(summonPoints >= data[i].Cost) {
					return true;
				}
			}
			return false;
		}
	}
}