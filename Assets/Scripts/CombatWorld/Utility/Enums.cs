namespace CombatWorld.Utility
{
	public enum Team
	{
		Player,
		AI,
		NONE,
	}

	public enum HighlightState
	{
		None,
		Selectable,
		Selected,
		NoMoreMoves,
		Moveable,
		NotMoveable,
		Attackable,
	}
}