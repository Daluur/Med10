using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;

namespace CombatWorld
{
	public class Tile : MonoBehaviour, ITile
	{
		private Dictionary<Direction,Tile> neighbours = new Dictionary<Direction,Tile>();
		Vec2i pos;
		TileType type;

		public void Setup(Vec2i pos, TileType type)
		{
			this.pos = pos;
			this.type = type;
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
			if (neighbours.ContainsKey(Direction.North))
			{
				neighbours[Direction.North].gameObject.GetComponent<Renderer>().material.color = Color.black;
			}
			if (neighbours.ContainsKey(Direction.East))
			{
				neighbours[Direction.East].gameObject.GetComponent<Renderer>().material.color = Color.red;
			}
			if (neighbours.ContainsKey(Direction.South))
			{
				neighbours[Direction.South].gameObject.GetComponent<Renderer>().material.color = Color.blue;
			}
			if (neighbours.ContainsKey(Direction.West))
			{
				neighbours[Direction.West].gameObject.GetComponent<Renderer>().material.color = Color.green;
			}
		}
	}
}
