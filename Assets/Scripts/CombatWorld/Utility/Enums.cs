namespace CombatWorld.Utility {
	public enum Team {
		Player,
		AI,
		NONE,
	}

	public enum HighlightState {
		None,
		Selectable,
		Selected,
		NoMoreMoves,
		Moveable,
		NotMoveable,
		Summon,
		Attackable,
		SelfClick,
	}

	public enum ElementalTypes {
		NONE,
		Fire,
		Water,
		Nature,
		Lightning,
	}

	
}
public enum MapTypes {
	Grass,
	Snow,
	ANY,
}