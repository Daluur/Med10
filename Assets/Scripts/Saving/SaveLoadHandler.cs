using System.Collections;
using System.Collections.Generic;
using Overworld.Shops;
using Overworld;
using UnityEngine;
using System.Text;
using LitJson;
using System.IO;

public class SaveLoadHandler {

	#region Singleton Things

	private static SaveLoadHandler instance;

	public static SaveLoadHandler Instance {
		get {
			if (instance == null) {
				instance = new SaveLoadHandler();
			}
			return instance;
		}
	}


	public SaveLoadHandler() {
		if (!loaded) {
			Load();
		}
	}
	#endregion

	List<int> StaticEncounters = new List<int>();
	List<int> UnlockedUnits = new List<int>();
	List<int> InventoryUnits = new List<int>();
	int gold = 0;
	int checkpoint = 0;

	bool loaded = false;
	Inventory inv = null;

	#region saving

	public void BeatAStaticEncounter(int ID) {
		if (StaticEncounters.Contains(ID)) {
			Debug.LogError("EncounterAlready marked as beaten! ID: " + ID);
		}
		else {
			StaticEncounters.Add(ID);
		}
	}

	public void Save(int id) {
		if (!loaded) {
			return;
		}
		checkpoint = id;
		SaveInventory();
		SaveShop();
		SaveGold();
		WriteToJSON();
	}

	void SaveInventory() {
		if (inv == null) {
			inv = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<Inventory>();
		}
		if(inv.loadedData == false) {
			return;
		}
		InventoryUnits.Clear();
		List<Item> items = inv.GetEntireInventory();
		for (int i = 0; i < items.Count; i++) {
			InventoryUnits.Add(items[i].ID);
		}
	}

	void SaveShop() {
		UnlockedUnits.Clear();
		List<int> unlocked =  UnlockHandler.Instance.GetUnlockedUnits();
		foreach (int unlock in unlocked) {
			UnlockedUnits.Add(unlock);
		}
	}

	void SaveGold() {
		gold = CurrencyHandler.GetCurrentGold();
	}

	void WriteToJSON() {
		SaveData data = new SaveData(StaticEncounters.ToArray(), InventoryUnits.ToArray(), UnlockedUnits.ToArray(), gold, checkpoint, DataGathering.Instance.GetAllTrades(), DataGathering.Instance.GetAllSummonedUnits(), DataGathering.Instance.GetAllDeckData(), DataGathering.Instance.movedShadowThroughOthersSaveThisValue, DataGathering.Instance.movedShadowWithoutMovingThroughOtherUnitsSaveThisValue, DataGathering.Instance.ID, DataGathering.Instance.Static, DataGathering.Instance.shadowToldCount, DataGathering.Instance.typesToldCount, DataGathering.Instance.notLearnedShadowCount, DataGathering.Instance.notLearnedTypesCount);

		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);
		writer.PrettyPrint = true;
		JsonMapper.ToJson(data, writer);
		File.WriteAllText(Application.dataPath + "/StreamingAssets/SaveGame.json", sb.ToString());
	}

	[System.Serializable]
	class SaveData {
		public int[] encounters;
		public int[] inventory;
		public int[] unlocks;
		public List<CombatTrades> trades;
		public List<SummonPlayerData> summons;
		public List<DeckDataClass> decks;
		public int shadow1;
		public int shadow2;
		public int ToldShadow;
		public int ToldTypes;
		public int NotLearnedShadow;
		public int NotLearnedTypes;
		public string ID;
		public bool Static;
		public int gold;
		public int checkpoint;

		public SaveData() {

		}

		public SaveData(int[] enc, int[] inv, int[] unl, int g, int c, List<CombatTrades> tr, List<SummonPlayerData> su, List<DeckDataClass> dd, int s1, int s2, string id, bool st, int TS, int TT, int NLS, int NLT) {
			encounters = enc;
			inventory = inv;
			unlocks = unl;
			gold = g;
			checkpoint = c;
			trades = tr;
			summons = su;
			decks = dd;
			shadow1 = s1;
			shadow2 = s2;
			ID = id;
			Static = st;
			ToldShadow = TS;
			ToldTypes = TT;
			NotLearnedShadow = NLS;
			NotLearnedTypes = NLT;
		}
		
	}

	#endregion

	#region loading

	void Load() {
		if (File.Exists(Application.dataPath + "/StreamingAssets/SaveGame.json")) {
			SaveData loadedData = JsonMapper.ToObject<SaveData>(File.ReadAllText(Application.dataPath + "/StreamingAssets/SaveGame.json"));
			StaticEncounters.AddRange(loadedData.encounters);
			InventoryUnits.AddRange(loadedData.inventory);
			UnlockedUnits.AddRange(loadedData.unlocks);
			gold = loadedData.gold;
			checkpoint = loadedData.checkpoint;
			DataGathering.Instance.LoadTrades(loadedData.trades);
			DataGathering.Instance.LoadSummons(loadedData.summons);
			DataGathering.Instance.LoadDecks(loadedData.decks);
			DataGathering.Instance.LoadShadow(loadedData.shadow1, loadedData.shadow2);
			DataGathering.Instance.OverrideID(loadedData.ID, loadedData.Static);
			DataGathering.Instance.shadowToldCount = loadedData.ToldShadow;
			DataGathering.Instance.typesToldCount = loadedData.ToldTypes;
			DataGathering.Instance.notLearnedShadowCount = loadedData.NotLearnedShadow;
			DataGathering.Instance.notLearnedTypesCount = loadedData.NotLearnedTypes;
		}
		else {
			InventoryUnits.Add(12);
		}
		loaded = true;
	}

	public bool AmIDefeated(int id) {
		return StaticEncounters.Contains(id);
	}

	public List<int> GetUnlockedUnits() {
		return UnlockedUnits;
	}

	public List<int> GetInventory() {
		return InventoryUnits;
	}

	public int GetGold() {
		return gold;
	}

	public int GetCheckpoint() {
		return checkpoint;
	}

	#endregion

	public int GetCurrentIsland() {
		return checkpoint;
	}

	public void Reset() {
		StartingValues();
		WriteToJSON();
	}

	void StartingValues() {
		StaticEncounters = new List<int>();
		InventoryUnits = new List<int>();
		UnlockedUnits = new List<int>();

		InventoryUnits.Add (12);

		gold = 50;
		checkpoint = 0;
	}
}