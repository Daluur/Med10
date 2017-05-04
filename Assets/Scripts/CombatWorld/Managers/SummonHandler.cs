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
				SetupButtonsAndData(inventory.GetComponent<Inventory>().GetFirstXItemsFromInventory(Values.NUMOFUNITSTOBRINGTOCOMBAT));
				List<int> unitIDs = new List<int>();
				foreach (var item in inventory.GetComponent<Inventory>().GetFirstXItemsFromInventory(Values.NUMOFUNITSTOBRINGTOCOMBAT)) {
					unitIDs.Add(item.ID);
				}
				DataGathering.Instance.UnitsBroughtToCombat(unitIDs);
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
			toReturn.Add(database.FetchItemByID(8));
			toReturn.Add(database.FetchItemByID(12));
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
			DataGathering.Instance.NewUnitSummoned(new SpecificSummonStats() { cost = currentlySelectedData.cost, pointsLeftAfter = summonPoints, spotsLeftAfter = GameController.instance.GetAmountOfOccupiedPlayerSummonSpots() });
			unit.transform.parent = transform;
		}

		public void SummonButtonPressed(CombatData CD, UnitButton but) {
			currentlySelectedData = CD;
			GameController.instance.HighlightSummonNodes();
			foreach (UnitButton item in buttons) {
				if (item != but) {
					item.ResetColor();
				}
				else {
					item.Highlight();
				}
			}
		}

		void SpendPoints(int amount) {
			summonPoints -= amount;
			UpdateButtonsAndText();

			if (TutorialHandler.instance != null) {
				if (TutorialHandler.instance.summonFirst) {
					TutorialHandler.instance.summonFirst = false;
					TutorialHandler.instance.FirstSummon();
				}
			}
		}

		public void GivePoints(int amount) {
			summonPoints += amount;
			UpdateButtonsAndText();
			/*if(GameNotificationsSystem.instance != null){
				GameNotificationsSystem.instance.DisplayMessage("You gained " + amount + " summon points!");
			}*/
		}

		public void UpdateButtonsAndText() {
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