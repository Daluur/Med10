using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;
using System;

namespace CombatWorld {
	public class Entity : MonoBehaviour, IEntity
	{
		Tile tile;

		public Tile GetTile()
		{
			return tile;
		}

		public void Move(Tile tile)
		{
			this.tile.RemoveOccupant();
			this.tile = tile;
			transform.position = tile.GetOccupantLocation().position;
			tile.SetOccupant(this);
		}

		public void Setup(Tile tile)
		{
			this.tile = tile;
			transform.position = tile.GetOccupantLocation().position;
			tile.SetOccupant(this);
		}
	}
}