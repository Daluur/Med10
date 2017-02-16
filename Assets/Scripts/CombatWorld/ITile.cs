using System.Collections;
using System.Collections.Generic;
using CombatWorld.Utility;

namespace CombatWorld
{
	public interface ITile
	{
		Tile GetNeighbour(Direction dir);
		void AssignNeighbour(Direction dir, Tile tile);
	}
}