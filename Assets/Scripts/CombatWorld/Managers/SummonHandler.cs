using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using CombatWorld.AI;

namespace CombatWorld
{
	public class SummonHandler : Singleton<SummonHandler>
	{
		public Text summonPointText;
		public Text summonPointP2Text;
		public GameObject ButtonTemplate;
		public GameObject buttonPanel;
		public GameObject buttonPanelP2;
		public int[] idsP1;
		public int[] idsP2;

		List<UnitButton> buttons = new List<UnitButton>();
		CombatData currentlySelectedData;
		ItemDatabase database;

		int summonPoints = 2;

		private void Start() {
			database = new ItemDatabase();
			GameObject inventory = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT);
			if (inventory != null) {
				SetupButtonsAndData(inventory.GetComponent<Inventory>().GetFirstXItemsFromInventory(4));
			}
			else {
				SetupButtonsAndData(GetRandomUnits(6), Team.Player);
				SetupButtonsAndData(GetRandomUnits(6), Team.AI);
				//SetupButtonsAndData(PVPdifferentUnits(idsP1), Team.Player);
				//SetupButtonsAndData(PVPdifferentUnits(idsP2), Team.AI);
			}
			UpdateButtonsAndText();
			SetPlayerTurn(Team.Player);
		}

		List<Item> GetRandomUnits(int amount) {
			int a = database.GetAllItems().Count;
			List<int> temp = new List<int>();
			for (int i = 0; i < amount; i++) {
				int j = Random.Range(0, a);
				if (temp.Contains(j)) {
					i--;
				}
				else {
					temp.Add(j);
				}
			}
			List<Item> toReturn = new List<Item>();
			foreach (int i in temp) {
				toReturn.Add(database.FetchItemByID(i));
			}
			return toReturn;
		}

		public void SetPlayerTurn(Team team) {
			if(team == Team.Player) {
				buttonPanel.SetActive(true);
				buttonPanelP2.SetActive(false);
			}
			else {
				buttonPanelP2.SetActive(true);
				buttonPanel.SetActive(false);
			}
		}

		List<Item> CombatNotStartedFromOverWorld() {
			List<Item> toReturn = new List<Item>();
			for (int i = 0; i < 14; i++) {
				toReturn.Add(database.FetchItemByID(i));
			}
			return toReturn;
		}

		List<Item> PVPdifferentUnits(int[] arr) {
			List<Item> toReturn = new List<Item>();
			foreach (int item in arr) {
				toReturn.Add((database.FetchItemByID(item)));
			}
			return toReturn;
		}

		void SetupButtonsAndData(List<Item> units, Team team = Team.Player) {
			if(team == Team.Player){
				foreach (Item item in units) {
					buttons.Add(Instantiate(ButtonTemplate, buttonPanel.transform).GetComponent<UnitButton>().Setup(item, SummonButtonPressed));
				}
			}
			else{
				foreach (Item item in units) {
					buttons.Add(Instantiate(ButtonTemplate, buttonPanelP2.transform).GetComponent<UnitButton>().Setup(item, SummonButtonPressed));
				}
			}
		}

		public void SummonUnit(SummonNode node)
		{
			SpendPoints(currentlySelectedData.cost);
			GameObject unit = Instantiate(currentlySelectedData.model, node.transform.position, Quaternion.identity) as GameObject;
			if (GameController.playerVSPlayer && GameController.instance.currentTeam == Team.AI) {
				unit.GetComponent<Unit>().SpawnEntity(node, Team.AI, currentlySelectedData);
			}
			else {
				unit.GetComponent<Unit>().SpawnEntity(node, Team.Player, currentlySelectedData);
			}
		}

		public void SummonButtonPressed(CombatData CD) {
			currentlySelectedData = CD;
			GameController.instance.HighlightSummonNodes();
		}

		void SpendPoints(int amount) {
			if (GameController.playerVSPlayer && GameController.instance.currentTeam == Team.AI) {
				AIController.instance.summonPoints -= amount;
				UpdateButtonsAndText();
			}
			else {
				summonPoints -= amount;
				UpdateButtonsAndText();
			}
		}

		public void GivePoints(int amount) {
			summonPoints += amount;
			UpdateButtonsAndText();
		}

		void UpdateButtonsAndText() {
			if (GameController.playerVSPlayer && GameController.instance.currentTeam == Team.AI) {
				foreach (UnitButton item in buttons) {
					item.CheckCost(AIController.instance.summonPoints);
				}
				summonPointP2Text.text = "Summon points: " + AIController.instance.summonPoints;
			}
			else {
				foreach (UnitButton item in buttons) {
					item.CheckCost(summonPoints);
				}
				summonPointText.text = "Summon points: " + summonPoints;
			}
		}

		public bool HasPointsToSummon() {
			foreach (UnitButton item in buttons) {
				if (!item.CanAfford(summonPoints)) {
					return false;
				}
			}
			return true;
		}

		public bool HasPointsToSummon(int points) {
			if (!GameController.playerVSPlayer) {
				Debug.Log("SHOULD ONLY bE USED IN PVP MODE!");
			}
			foreach (UnitButton item in buttons) {
				if (!item.CanAfford(points)) {
					return false;
				}
			}
			return true;
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