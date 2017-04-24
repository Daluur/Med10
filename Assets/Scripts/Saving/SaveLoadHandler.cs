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
		checkpoint = id;
		SaveInventory();
		SaveShop();
		SaveGold();
		WriteToJSON();
	}

	void SaveInventory() {
		InventoryUnits.Clear();
		var inv = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<Inventory>();
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
		SaveData data = new SaveData(StaticEncounters.ToArray(), InventoryUnits.ToArray(), UnlockedUnits.ToArray(), gold, checkpoint);

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
		public int gold;
		public int checkpoint;

		public SaveData() {

		}

		public SaveData(int[] enc, int[] inv, int[] unl, int g, int c) {
			encounters = enc;
			inventory = inv;
			unlocks = unl;
			gold = g;
			checkpoint = c;
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

		InventoryUnits.Add(12);
		InventoryUnits.Add(13);

		gold = 50;
		checkpoint = 0;
	}
}