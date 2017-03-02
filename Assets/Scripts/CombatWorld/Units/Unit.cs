using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;

namespace CombatWorld.Units
{
	public class Unit : Entity
	{
		int moves;
		int movesLeft;

		public void Move(Node node)
		{
			currentNode.RemoveOccupant();
			node.SetOccupant(this);
			currentNode = node;
		}
	}
}