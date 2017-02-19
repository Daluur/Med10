using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Utility;

namespace CombatWorld
{
	public class InputManager : Singleton<InputManager>
	{
		Entity currentEntity;

		public void SetEntity(Entity ent)
		{
			currentEntity = ent;
		}

		public void GotInput(Tile tile)
		{
			if (tile.CanBeMovedTo())
			{
				currentEntity.Move(tile);
			}
		}
	}
}