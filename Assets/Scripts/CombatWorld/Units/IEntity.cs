using System.Collections;
using System.Collections.Generic;
using CombatWorld.Utility;

namespace CombatWorld
{
	public interface IEntity
	{
		void Setup(Tile tile);
		void Move(Tile tile);
		Tile GetTile();
	}
}