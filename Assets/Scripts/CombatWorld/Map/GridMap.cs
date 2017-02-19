using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;

namespace CombatWorld
{
	public class GridMap : MonoBehaviour
	{
		public Dictionary<Vec2i, Tile> tileMap = new Dictionary<Vec2i, Tile>();
		private Vec2i size;

		public int sizeX;
		public int sizeY;
		public GameObject[] tilePrefabs;
		public GameObject entity;

		void Start()
		{
			size = new Vec2i(sizeX, sizeY);
			CreateMap();
		}

		void CreateMap()
		{
			Vec2i temp;
			Tile tile;

			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					temp = new Vec2i(x, y);
					tile = Instantiate(tilePrefabs[Random.Range(0,tilePrefabs.Length)], new Vector3(x, 0, y), Quaternion.identity, transform).GetComponent<Tile>();
					tile.Setup(temp, TileType.Walkable);
					tileMap[temp] = tile;
				}
			}
			AssignNeighbours();
		}

		void AssignNeighbours()
		{
			Vec2i temp;
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					temp = new Vec2i(x, y);
					if (!tileMap.ContainsKey(temp))
					{
						Debug.Log(tileMap[temp]);
						break;
					}
					if (x != 0)
					{
						tileMap[temp].AssignNeighbour(Direction.West, tileMap[temp + new Vec2i(-1, 0)]);
					}
					if (y != 0)
					{
						tileMap[temp].AssignNeighbour(Direction.South, tileMap[temp + new Vec2i(0, -1)]);
					}
					if (x != size.x - 1)
					{
						tileMap[temp].AssignNeighbour(Direction.East, tileMap[temp + new Vec2i(1, 0)]);
					}
					if (y != size.y - 1)
					{
						tileMap[temp].AssignNeighbour(Direction.North, tileMap[temp + new Vec2i(0, 1)]);
					}
				}
			}
			SpawnEntity();
		}

		void SpawnEntity()
		{
			Vec2i pos = new Vec2i(0, 2);
			Entity ent = Instantiate(entity).GetComponent<Entity>();
			ent.Setup(tileMap[pos]);
			InputManager.instance.SetEntity(ent);

			SpawnEntity(new Vec2i(0, 1));
			SpawnEntity(new Vec2i(2, 1));
			SpawnEntity(new Vec2i(3, 4));
			SpawnEntity(new Vec2i(9, 4));
		}

		void SpawnEntity(Vec2i pos)
		{
			Entity ent = Instantiate(entity).GetComponent<Entity>();
			ent.Setup(tileMap[pos]);
		}
	}
}