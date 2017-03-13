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
	}

	public enum ElementalTypes {
		NONE,
		Fire,
		Water,
		Earth,
		Ligthning,
	}
}