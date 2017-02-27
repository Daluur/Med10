using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;
using System;

namespace CombatWorld
{
	public class Tile : MonoBehaviour
	{
		public TileType type;
		public Transform occupantPos;
		public Highlighter highlightHandler;

		private Dictionary<Direction, Tile> neighbours = new Dictionary<Direction, Tile>();
		Entity occupant;
		Vec2i pos;

		Action<Highlight> highlightChange;

		bool acceptsInput = false;

		public void Setup(Vec2i pos)
		{
			this.pos = pos;
			InputManager.instance.SubscribeToEndTurn(TurnEnded);
		}

		public Vec2i GetWorldPos()
		{
			return pos;
		}

		public IEnumerable<Tile> GetAllNeighbours()
		{
			List<Tile> tiles = new List<Tile>();
			foreach (KeyValuePair<Direction,Tile> pair in neighbours)
			{
				tiles.Add(pair.Value);
			}
			return tiles;
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
			if (!acceptsInput)
			{
				return;
			}
			InputManager.instance.GotInput(this);
		}

		void OnMouseEnter()
		{
			if (!acceptsInput)
			{
				return;
			}
			highlightChange(Highlight.Special);
		}

		void OnMouseExit()
		{
			if (!acceptsInput)
			{
				return;
			}
			SetHighlight(Highlight.Simple);
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

		public void SetHighlight(Highlight highlight)
		{
			switch (highlight)
			{
				case Highlight.None:
					acceptsInput = false;
					break;
				case Highlight.UnSelectable:
					acceptsInput = false;
					break;
				case Highlight.Simple:
					acceptsInput = true;
					break;
				default:
					break;
			}
			if(highlightChange != null)
			{
				highlightChange(highlight);
			}
		}

		void TurnEnded()
		{
			acceptsInput = false;
		}

		public void SubscribeToHighlightChange(Action<Highlight> cb)
		{
			highlightChange += cb;
		}

		public void UnsubscribeToHighlightChange(Action<Highlight> cb)
		{
			highlightChange -= cb;
		}

		void OnDestroy()
		{
			InputManager.instance.UnsubscribeToEndTurn(TurnEnded);
		}
	}
}
