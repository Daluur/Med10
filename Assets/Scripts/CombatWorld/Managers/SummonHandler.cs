using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using Overworld;

namespace CombatWorld
{
	public class SummonHandler : Singleton<SummonHandler>
	{
		public Text summonPointText;
		public GameObject ButtonTemplate;
		public GameObject buttonPanel;

		List<UnitButton> buttons = new List<UnitButton>();
		CombatData currentlySelectedData;
		ItemDatabase database;

		int summonPoints = DamageConstants.STARTSUMMONPOINTS;

		private void Start() {
			database = new ItemDatabase();
			GameObject inventory = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT);
			if (inventory != null) {
				SetupButtonsAndData(inventory.GetComponent<Inventory>().GetFirstXItemsFromInventory(6));
			}
			else {
				SetupButtonsAndData(CombatNotStartedFromOverWorld());
			}
			UpdateButtonsAndText();
		}

		List<Item> CombatNotStartedFromOverWorld() {
			List<Item> toReturn = new List<Item>();
			/*for (int i = 0; i < 14; i++) {
				toReturn.Add(database.FetchItemByID(i));
			}*/
			toReturn.Add(database.FetchItemByID(0));
			toReturn.Add(database.FetchItemByID(3));
			toReturn.Add(database.FetchItemByID(4));
			toReturn.Add(database.FetchItemByID(7));
			toReturn.Add(database.FetchItemByID(12));
			toReturn.Add(database.FetchItemByID(13));
			return toReturn;
		}

		void SetupButtonsAndData(List<Item> units) {
			foreach (Item item in units) {
				buttons.Add(Instantiate(ButtonTemplate, buttonPanel.transform).GetComponent<UnitButton>().Setup(item, SummonButtonPressed));
			}
		}

		public void SummonUnit(SummonNode node)
		{
			SpendPoints(currentlySelectedData.cost);
			GameObject unit = Instantiate(currentlySelectedData.model, node.transform.position, Quaternion.identity) as GameObject;
			unit.GetComponent<Unit>().SpawnEntity(node, Team.Player, currentlySelectedData);
			unit.transform.parent = transform;
		}

		public void SummonButtonPressed(CombatData CD) {
			currentlySelectedData = CD;
			GameController.instance.HighlightSummonNodes();
		}

		void SpendPoints(int amount) {
			summonPoints -= amount;
			UpdateButtonsAndText();

			if (TutorialHandler.instance.summonFirst) {
				TutorialHandler.instance.summonFirst = false;
				GeneralConfirmationBox.instance.ShowPopUp ("Units cannot make other moves the round they are summoned.\nWhen you have performed all your moves, end your turn, after which the opponent will take theirs.", "Okay");
			}
		}

		public void GivePoints(int amount) {
			summonPoints += amount;
			UpdateButtonsAndText();
			/*if(GameNotificationsSystem.instance != null){
				GameNotificationsSystem.instance.DisplayMessage("You gained " + amount + " summon points!");
			}*/
		}

		void UpdateButtonsAndText() {
			foreach (UnitButton item in buttons) {
				item.CheckCost(summonPoints);
			}
			summonPointText.text = summonPoints.ToString();
		}

		public bool HasPointsToSummon() {
			foreach (UnitButton item in buttons) {
				if (item.CanAfford(summonPoints)) {
					return true;
				}
			}
			return false;
		}

		public Unit SummonAIUnitByID(SummonNode node, int id) {
			Item item = database.FetchItemByID(id);
			CombatData data = new CombatData(item);
			GameObject unit = Instantiate(data.model, node.transform.position, Quaternion.identity) as GameObject;
			unit.GetComponent<Unit>().SpawnEntity(node, Team.AI, data);
			unit.transform.parent = transform;
			return unit.GetComponent<Unit>();
		}
	}
}