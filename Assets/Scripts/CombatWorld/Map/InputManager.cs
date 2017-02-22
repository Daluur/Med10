using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;

namespace CombatWorld
{
	public class InputManager : Singleton<InputManager>
	{
		Entity currentEntity;
		List<Entity> entities = new List<Entity>();
		int current = 0;

		Action TurnEnded;

		public void AddEntity(Entity ent)
		{
			entities.Add(ent);
		}

		public void EndedTurn()
		{
			current++;
			if(current >= entities.Count)
			{
				current = 0;
			}
			currentEntity = entities[current];
			if(TurnEnded != null)
			{
				TurnEnded();
			}
			currentEntity.MyTurn();
		}

		public void StartGame()
		{
			currentEntity = entities[current];
			currentEntity.MyTurn();
		}

		public void GotInput(Tile tile)
		{
			if (tile.CanBeMovedTo())
			{
				currentEntity.Move(tile);
				currentEntity.EndTurn();
			}
		}

		public void SubscribeToEndTurn(Action cb)
		{
			TurnEnded += cb;
		}

		public void UnsubscribeToEndTurn(Action cb)
		{
			TurnEnded -= cb;
		}
	}
}