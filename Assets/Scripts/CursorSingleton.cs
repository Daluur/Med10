using System.Collections.Generic;
using CombatWorld.Utility;
using Overworld;
using UnityEngine;

public class CursorSingleton : Singleton<CursorSingleton> {
	private Texture2D[] cursorTextures;
	private Dictionary<string, Texture2D> nameAndCursorTextures = new Dictionary<string, Texture2D>();

	void Awake() {
		base.Awake();
		InitCursorTexturesAndHolder();
		SetCursor();
	}

	private void InitCursorTexturesAndHolder() {
		cursorTextures = Resources.LoadAll<Texture2D>(CursorTextureConstants.CURSORFOLDER);
		for (int i = 0; i < cursorTextures.Length; i++) {
			switch (cursorTextures[i].name) {
				case CursorTextureConstants.NORMALCURSOR:
					nameAndCursorTextures.Add(CursorTextureConstants.NORMALCURSOR, cursorTextures[i]);
					break;
				case CursorTextureConstants.INTERACTIONCURSOR:
					nameAndCursorTextures.Add(CursorTextureConstants.INTERACTIONCURSOR, cursorTextures[i]);
					break;
				case CursorTextureConstants.COMBATCURSOR:
					nameAndCursorTextures.Add(CursorTextureConstants.COMBATCURSOR, cursorTextures[i]);
					break;
			}
		}
	}

	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = Vector2.zero;

	public void SetCursor(GameObject go = null) {
		var texture = OverWorldGetCursor(go);
		Cursor.SetCursor(texture,hotSpot,cursorMode);
	}

	public void SetCursor(HighlightState state) {
		var texture = CombatWorldGetCursor(state);
		Cursor.SetCursor(texture,hotSpot,cursorMode);
	}

	private Texture2D OverWorldGetCursor(GameObject go = null) {
		Texture2D tmp;
		if (go != null && go.GetComponent<Encounter>()) {
			if (nameAndCursorTextures.TryGetValue(CursorTextureConstants.COMBATCURSOR, out tmp))
				return tmp;
		}
		if (go != null && go.GetComponent<TowerBehavior>()) {
			if (nameAndCursorTextures.TryGetValue(CursorTextureConstants.INTERACTIONCURSOR, out tmp))
				return tmp;
		}
		if(nameAndCursorTextures.TryGetValue(CursorTextureConstants.NORMALCURSOR, out tmp))
			return tmp;
		ErrorNoCursorTexture();
		return null;
	}

	private Texture2D CombatWorldGetCursor(HighlightState state) {
		Texture2D tmp;
		switch (state) {
			case HighlightState.Moveable:
			case HighlightState.Selectable:
			case HighlightState.SelfClick:
			case HighlightState.Summon:
			case HighlightState.NoMoreMoves:
				if (nameAndCursorTextures.TryGetValue(CursorTextureConstants.INTERACTIONCURSOR, out tmp))
					return tmp;
				break;
			case HighlightState.Attackable:
				if (nameAndCursorTextures.TryGetValue(CursorTextureConstants.COMBATCURSOR, out tmp))
					return tmp;
				break;
			default:
				if (nameAndCursorTextures.TryGetValue(CursorTextureConstants.NORMALCURSOR, out tmp))
					return tmp;
				break;
		}
		ErrorNoCursorTexture();
		return null;
	}

	private void ErrorNoCursorTexture() {
		Debug.LogError("No cursor texture was found, make sure they are located in the correct folder: " + CursorTextureConstants.CURSORFOLDER + ", and they have the apropriate names, look in Constants.cs to either redefine the names or to see how they are named");
	}

}
