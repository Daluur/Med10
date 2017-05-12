using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using CombatWorld.AI;
using Overworld;

namespace CombatWorld
{
	public class SummonHandler : Singleton<SummonHandler>
	{
		public Text summonPointText;
		public Text summonPointP2Text;
		public GameObject ButtonTemplate;
		public GameObject buttonPanel;
		public GameObject buttonPanelP2;

		[Space(20)]
		[Header("Using Deck or UnitIDS?")]
		public bool UseDeck = true;

		[Space]
		[Header("If using UnitsIDs.")]
		[Header("If length of arrays are less than units")]
		[Header("per team, the rest will be random.")]
		public int unitsPerTeam = 6;
		public int[] idsP1;
		public int[] idsP2;

		[Space]
		[Header("If using decks.")]
		[Header("Find and make decks in DeckHandler")]
		public int p1DeckID;
		public int p2DeckID;

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
				List<Item> p1Units;
				List<Item> p2Units;
				if (UseDeck) {
					p1Units = PVPdifferentUnits(DeckHandler.GetDeckFromID(p1DeckID).unitIDs);
					p2Units = PVPdifferentUnits(DeckHandler.GetDeckFromID(p2DeckID).unitIDs);
				}
				else {
					p1Units = PVPdifferentUnits(idsP1);
					p2Units = PVPdifferentUnits(idsP2);
					p1Units = GetRandomUnits(unitsPerTeam - p1Units.Count, p1Units);
					p2Units = GetRandomUnits(unitsPerTeam - p2Units.Count, p2Units);
				}
				SetupButtonsAndData(p1Units, Team.Player);
				SetupButtonsAndData(p2Units, Team.AI);
				//SetupButtonsAndData(PVPdifferentUnits(idsP1), Team.Player);
				//SetupButtonsAndData(PVPdifferentUnits(idsP2), Team.AI);
			}
			UpdateButtonsAndText();
			SetPlayerTurn(Team.Player);
		}

		List<Item> GetRandomUnits(int amount, List<Item> alreadyAdded) {
			if (amount == 0) {
				return alreadyAdded;
			}
			int a = database.GetAllItems().Count;
			List<int> existing = new List<int>();

			foreach (Item item in alreadyAdded) {
				existing.Add(item.ID);
			}

			for (int i = 0; i < amount; i++) {
				int x = Random.Range(0, a);
				if (existing.Contains(x)) {
					i--;
				}
				else {
					alreadyAdded.Add(database.FetchItemByID(x));
					existing.Add(x);
				}
			}
			alreadyAdded.Sort((x, y) => x.ID.CompareTo(y.ID));
			return alreadyAdded;
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
			if (GameController.playerVSPlayer && GameController.instance.currentTeam == Team.AI) {
				AICalculateScore.instance.WithdrawSummonPoints(amount);
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
			/*if(GameNotificationsSystem.instance != null){
				GameNotificationsSystem.instance.DisplayMessage("You gained " + amount + " summon points!");
			}*/
		}

		public void UpdateButtonsAndText() {
			if (GameController.playerVSPlayer && GameController.instance.currentTeam == Team.AI) {
				foreach (UnitButton item in buttons) {
					item.CheckCost(AICalculateScore.instance.summonPoints);
				}
				summonPointP2Text.text = "Summon points: " + AICalculateScore.instance.summonPoints;
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
				if (item.CanAfford(summonPoints)) {
					return true;
				}
			}
			return false;
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