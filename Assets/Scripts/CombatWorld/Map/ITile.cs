using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;

namespace CombatWorld
{
	public interface ITile
	{
		Tile GetNeighbour(Direction dir);
		void AssignNeighbour(Direction dir, Tile tile);
		void Setup(Vec2i pos, TileType type);
		Entity GetOccupant();
		bool HasOccupant();
		Transform GetOccupantLocation();
		void SetOccupant(Entity occupant);
		void RemoveOccupant();
		TileType GetTileType();
		bool CanBeMovedTo();
	}
}