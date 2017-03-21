using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CombatWorld.Utility;
using LitJson;


public class UnitMaker : EditorWindow {

	public static UnitMaker window;

	int imageMaxSize = 70;

	Item editing;
	ItemDatabase database;
	List<Item> items = new List<Item>();

	bool changesToList = false;

	Vector2 scrollPos;

	[MenuItem("Unit editor/Editor")]
	public static void ShowWindow() {
		//Show existing window instance. If one doesn't exist, make one.
		window = (UnitMaker)EditorWindow.GetWindow(typeof(UnitMaker));
	}

	void OnGUI() {

		if (database == null) {
			database = new ItemDatabase();
			items = database.GetAllItems();
		}
		Filter();
		ShowExistingItems();
	}

	bool filtered = false;
	int filter_health = 0;
	int filter_attack = 0;
	int filter_summoncost = 0;
	int filter_goldcost = 0;
	int filter_moves = 0;
	bool filterFor_simpletype = false;
	SimpleType filter_simpletype = SimpleType.NONE;
	bool filterFor_type = false;
	ElementalTypes filter_type = ElementalTypes.NONE;
	string filter_slug = "";

	void Filter() {
		if (EditorGUILayout.BeginFadeGroup(filtered ? 0 : 1)) {
			filtered = EditorGUILayout.Toggle("Show filters", filtered);
		}
		EditorGUILayout.EndFadeGroup();

		float t = filtered ? 1 : 0;
		if (EditorGUILayout.BeginFadeGroup(t)) {
			filter_attack = EditorGUILayout.IntField("ATT: ", filter_attack);
			filter_health = EditorGUILayout.IntField("HP: ", filter_health);
			filter_summoncost = EditorGUILayout.IntField("Summon cost: ", filter_summoncost);
			filter_goldcost = EditorGUILayout.IntField("Gold cost: ", filter_goldcost);
			filter_moves = EditorGUILayout.IntField("Moves: ", filter_moves);

			filterFor_simpletype = EditorGUILayout.Toggle("Filter for special", filterFor_simpletype);
			if (filterFor_simpletype) {
				filter_simpletype = (SimpleType)EditorGUILayout.EnumPopup("Special: ", filter_simpletype);
			}

			filterFor_type = EditorGUILayout.Toggle("Filter for type", filterFor_type);
			if (filterFor_type) {
				filter_type = (ElementalTypes)EditorGUILayout.EnumPopup("Type: ", filter_type);
			}

			filter_slug = EditorGUILayout.TextField("slug", filter_slug);

			if (GUILayout.Button("Reset")) {
				filtered = false;
				Repaint();
			}
		}
		EditorGUILayout.EndFadeGroup();
	}

	bool isFiltered(Item unit) {
		if (filter_attack != 0) {
			if(unit.Attack != filter_attack) {
				return true;
			}
		}
		if (filter_health != 0) {
			if (unit.Health != filter_health) {
				return true;
			}
		}
		if (filter_summoncost != 0) {
			if (unit.SummonCost != filter_summoncost) {
				return true;
			}
		}
		if (filter_goldcost != 0) {
			if (unit.GoldCost != filter_goldcost) {
				return true;
			}
		}
		if (filter_moves != 0) {
			if (unit.Moves != filter_moves) {
				return true;
			}
		}
		if (filterFor_simpletype) {
			switch (filter_simpletype) {
				case SimpleType.NONE:
					if(unit.Shadow || unit.Stone) {
						return true;
					}
					break;
				case SimpleType.Shadow:
					if (!unit.Shadow) {
						return true;
					}
					break;
				case SimpleType.Stone:
					if (!unit.Stone) {
						return true;
					}
					break;
				default:
					break;
			}
		}
		if (filterFor_type) {
			string checker = GetStringFromType(filter_type);
			if(unit.Type != checker) {
				return true;
			}
		}
		if (filter_slug != "") {
			if(unit.Slug != filter_slug) {
				return true;
			}
		}
		return false;
	}

	void ShowExistingItems() {
		float height = window.position.height - (filtered ? 272 : 120);
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(window.position.width), GUILayout.Height(height));
		foreach (Item item in items) {
			if (filtered && isFiltered(item)) {
				continue;
			}
			if (item == editing) {
				editingUnit(item);
				if (changesToList) {
					changesToList = false;
					EditorGUILayout.EndScrollView();
					Repaint();
					return;
				}
			}
			else {
				notEditingUnit(item);
				if (changesToList) {
					changesToList = false;
					EditorGUILayout.EndScrollView();
					Repaint();
					return;
				}
			}
		}
		EditorGUILayout.EndScrollView();

		if (GUILayout.Button("NEW!", GUILayout.Height(40))) {
			scrollPos.y = float.MaxValue;
			NewUnit();
		}

		if (GUILayout.Button("Save and close!", GUILayout.Height(55))) {
			Save();
			window.Close();
		}
	}

	void notEditingUnit(Item unit) {
		EditorGUILayout.BeginVertical("Box", GUILayout.Width(window.position.width - 23));

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(unit.Title);
		if (GUILayout.Button("Edit", GUILayout.Width(100))) {
			editing = unit;
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Att: " + unit.Attack);
		GUILayout.Label("HP: " + unit.Health);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Moves: " + unit.Moves);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Summon cost: " + unit.SummonCost);
		GUILayout.Label("Gold cost: " + unit.GoldCost);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Type: " + unit.Type);
		string specialText = unit.Stone ? "Stone" : unit.Shadow ? "Shadow" : "Nothing";
		GUILayout.Label("Special: " + specialText);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Description: " + unit.Description ,GUILayout.MaxWidth(window.position.width - 35));
		EditorGUILayout.EndHorizontal();

		GUILayout.Label("Slug: " + unit.Slug);

		EditorGUILayout.BeginHorizontal();
		if (unit.Sprite != null) {
			GUILayout.Label(unit.Sprite.texture, GUILayout.MaxHeight(imageMaxSize), GUILayout.MaxHeight(imageMaxSize));
		}
		else {
			GUILayout.Label("No image");
		}
		if (unit.Model != null) {
			GUILayout.Label(AssetPreview.GetAssetPreview(unit.Model), GUILayout.MaxHeight(imageMaxSize), GUILayout.MaxHeight(imageMaxSize));
		}
		else {
			GUILayout.Label("No model");
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}

	void editingUnit(Item unit) {
		EditorGUILayout.BeginVertical("Box", GUILayout.Width(window.position.width - 23));

		EditorGUILayout.BeginHorizontal();
		editing.Title = EditorGUILayout.TextField("Name: ", editing.Title);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		editing.Attack = EditorGUILayout.IntField("ATT: ", editing.Attack);
		editing.Health = EditorGUILayout.IntField("HP: ", editing.Health);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		editing.Moves = EditorGUILayout.IntField("Moves: ", editing.Moves);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		editing.SummonCost = EditorGUILayout.IntField("Summon cost: ", editing.SummonCost);
		editing.GoldCost = EditorGUILayout.IntField("Gold cost: ", editing.GoldCost);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		ElementalTypes type = GetTypeFromString(editing.Type);
		type = (ElementalTypes)EditorGUILayout.EnumPopup("Type: ", type);
		editing.Type = GetStringFromType(type);

		SimpleType specialType = GetSimpleTypeFromUnit(editing);
		specialType = (SimpleType)EditorGUILayout.EnumPopup("Special: ", specialType);
		editing.Shadow = specialType == SimpleType.Shadow ? true : false;
		editing.Stone = specialType == SimpleType.Stone ? true : false;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		editing.Description = EditorGUILayout.TextField("Description: ", editing.Description);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		string temp = EditorGUILayout.DelayedTextField("Slug: ", editing.Slug);
		if(temp != editing.Slug) {
			editing.Slug = temp;
			editing.Sprite = Resources.Load<Sprite>("Art/2D/Units/" + editing.Slug);
			editing.Model = Resources.Load<GameObject>("Art/3D/Units/" + editing.Slug);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if (editing.Sprite != null) {
			GUILayout.Label(editing.Sprite.texture, GUILayout.MaxHeight(imageMaxSize), GUILayout.MaxHeight(imageMaxSize));
		}
		else {
			GUILayout.Label("No image");
		}
		if (editing.Model != null) {
			GUILayout.Label(AssetPreview.GetAssetPreview(editing.Model), EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(imageMaxSize), GUILayout.MaxHeight(imageMaxSize));
		}
		else {
			GUILayout.Label("No model");
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Done editing")) {
			editing = null;
		}
		if (GUILayout.Button("Save")) {
			Save();
			editing = null;
		}
		EditorGUILayout.EndHorizontal();
		if (GUILayout.Button("Delete") && EditorUtility.DisplayDialog("DELETE?", "Are you sure you want to delete this unit? CANNOT be undone!", "YES!", "Cancel")) {
			Delete();
			editing = null;
		}
		EditorGUILayout.EndVertical();
	}

	void Delete() {
		items.Remove(editing);
		changesToList = true;
		editing = null;
	}

	void Save() {
		List<JSONData> data = new List<JSONData>();
		Dictionary<string, int> stats;
		JSONData newData;
		foreach (Item item in items) {
			stats = new Dictionary<string, int>();
			stats.Add("attack", item.Attack);
			stats.Add("health", item.Health);
			stats.Add("moves", item.Moves);
			stats.Add("summonCost", item.SummonCost);
			newData = new JSONData(item.ID, item.Title, item.Type, item.Transform, item.Shadow, item.Stone, stats, item.GoldCost, item.Description, false, item.Slug);
			data.Add(newData);
		}

		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);
		writer.PrettyPrint = true;
		JsonMapper.ToJson(data, writer);

		File.WriteAllText(Application.dataPath + "/StreamingAssets/Items.json", sb.ToString());
	}

	void NewUnit() {
		int newID = items[items.Count - 1].ID + 1;
		editing = new Item(newID);
		items.Add(editing);
		Repaint();
	}

	[System.Serializable]
	class JSONData{
		public int id;
		public string title;
		public string type;
		public string transform;
		public bool shadow;
		public bool stone;
		public Dictionary<string, int> stats;
		public int goldCost;
		public string description;
		public bool stackable;
		public string slug;

		public JSONData(int id, string title, string type, string transform,bool shadow,bool stone, Dictionary<string, int> stats,int goldCost, string description, bool stackable, string slug) {
			this.id = id;
			this.title = title;
			this.type = type;
			this.transform = transform;
			this.shadow = shadow;
			this.stone = stone;
			this.stats = stats;
			this.goldCost = goldCost;
			this.description = description;
			this.stackable = stackable;
			this.slug = slug;
		}
	}

	#region helperFunctions

	enum SimpleType {
		NONE,
		Shadow,
		Stone,
	}

	SimpleType GetSimpleTypeFromUnit(Item unit) {
		if(unit.Shadow && unit.Stone) {
			Debug.LogError("A unit is both shadow and stone, should never happen! " + unit.Title);
		}
		if (unit.Shadow) {
			return SimpleType.Shadow;
		}
		if (unit.Stone) {
			return SimpleType.Stone;
		}
		return SimpleType.NONE;
	}

	ElementalTypes GetTypeFromString(string type) {
		switch (type) {
			case "Fire":
				return ElementalTypes.Fire;
			case "Water":
				return ElementalTypes.Water;
			case "Lightning":
				return ElementalTypes.Lightning;
			case "Nature":
				return ElementalTypes.Nature;
			default:
				return ElementalTypes.NONE;
		}
	}

	string GetStringFromType(ElementalTypes type) {
		switch (type) {
			case ElementalTypes.Fire:
				return "Fire";
			case ElementalTypes.Water:
				return "Water";
			case ElementalTypes.Nature:
				return "Nature";
			case ElementalTypes.Lightning:
				return "Lightning";
			case ElementalTypes.NONE:
			default:
				return "Normal";
		}
	}

	#endregion
}
