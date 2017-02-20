using System;
using System.Collections.Generic;

namespace CombatWorld.Utility
{
	public enum TileType
	{
		Walkable,
		UnWalkable,
	}

	public enum Direction
	{
		North,
		East, 
		South,
		West,
	}

	public enum Highlight
	{
		None,
		UnSelectable,
		Simple,
		Special,
	}
}
