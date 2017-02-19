using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;
using System;

namespace CombatWorld
{
	public class Tile : MonoBehaviour, ITile
	{
		private Dictionary<Direction, Tile> neighbours = new Dictionary<Direction, Tile>();
		Vec2i pos;
		public TileType type;
		public Transform occupantPos;
		Entity occupant;

		public void Setup(Vec2i pos, TileType type)
		{
			this.pos = pos;
			//this.type = type;
		}

		/// <summary>
		/// Can return null, if there is no neighbour in that direction!
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		public Tile GetNeighbour(Direction dir)
		{
			return neighbours[dir];
		}

		public void AssignNeighbour(Direction dir, Tile tile)
		{
			neighbours.Add(dir, tile);
		}

		void OnMouseDown()
		{
			InputManager.instance.GotInput(this);
		}

		/// <summary>
		/// Returns null, if there is no occupant.
		/// </summary>
		/// <returns></returns>
		public Entity GetOccupant()
		{
			return occupant;
		}

		public bool HasOccupant()
		{
			return occupant;
		}

		public void SetOccupant(Entity occupant)
		{
			if (HasOccupant())
			{
				Debug.LogError("Cannot move an unit to an already occupied tile.");
				throw new NotSupportedException();
			}
			this.occupant = occupant;
		}

		public void RemoveOccupant()
		{
			occupant = null;
		}

		public Transform GetOccupantLocation()
		{
			return occupantPos;
		}

		public TileType GetTileType()
		{
			return type;
		}

		public bool CanBeMovedTo()
		{
			return type == TileType.Walkable && !HasOccupant();
		}
	}
}
