using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;

namespace CombatWorld.Units
{
	public class Entity : MonoBehaviour
	{
		int moves;
		int movesLeft;

		Team team;
		Node currentNode;

		public void Move(Node node)
		{
			currentNode.RemoveOccupant();
			node.SetOccupant(this);
			currentNode = node;
		}

		public Team GetTeam()
		{
			return team;
		}

	}
}