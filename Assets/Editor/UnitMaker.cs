using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CombatWorld.Utility;
using LitJson;

public class UnitMaker : EditorWindow {

	public static UnitMaker window;

	bool isEditing = false;
	bool ShowingExisting = false;
	Item editing;
	ItemDatabase database;
	List<Item> items = new List<Item>();

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
		ShowExistingItems();
	}

	Vector2 scrollPos;

	void ShowExistingItems() {
		GUILayout.Label("Existing units!");
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(window.position.width), GUILayout.Height(600));
		foreach (Item item in items) {
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(item.Title);
			if (GUILayout.Button("Edit", GUILayout.Width(100))) {
				isEditing = true;
				editing = item;
				EditUnit();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Att: " + item.Attack);
			GUILayout.Label("HP: " + item.Health);
			GUILayout.Label("Type: " + item.Type);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Summon cost: " + item.SummonCost);
			GUILayout.Label("Gold cost: " + item.GoldCost);
			EditorGUILayout.EndHorizontal();
			if (item.Sprite != null) {
				GUILayout.Label(item.Sprite.texture);
			}
			else {
				GUILayout.Label("No image");
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndScrollView();
		if (GUILayout.Button("NEW!")) {
			isEditing = true;
			editing = new Item();
			EditUnit();
		}
	}

	void EditUnit() {
		ElementalTypes type = GetTypeFromString(editing.Type);
		editing.Title = EditorGUILayout.TextField(editing.Title);
		editing.Attack = EditorGUILayout.IntField(editing.Attack);
		editing.Health = EditorGUILayout.IntField(editing.Health);
		editing.GoldCost = EditorGUILayout.IntField(editing.GoldCost);
		editing.SummonCost = EditorGUILayout.IntField(editing.SummonCost);
		type = (ElementalTypes)EditorGUILayout.EnumPopup("Type: ", type);
		editing.Type = GetStringFromType(type);
		
		if (GUILayout.Button("SAVE!")) {
			Save();
		}
	}

	void Save() {

	}

	ElementalTypes GetTypeFromString(string type) {
		switch (type) {
			case "fire_unit":
				return ElementalTypes.Fire;
			case "water_unit":
				return ElementalTypes.Water;
			case "lightning_unit":
				return ElementalTypes.Lightning;
			case "nature_unit":
				return ElementalTypes.Nature;
			default:
				return ElementalTypes.NONE;
		}
	}

	string GetStringFromType(ElementalTypes type) {
		switch (type) {
			case ElementalTypes.Fire:
				return "fire_unit";
			case ElementalTypes.Water:
				return "water_unit";
			case ElementalTypes.Nature:
				return "nature_unit";
			case ElementalTypes.Lightning:
				return "lightning_unit";
			case ElementalTypes.NONE:
			default:
				return "NONE";
		}
	}
}
