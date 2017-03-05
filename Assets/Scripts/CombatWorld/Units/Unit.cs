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

		bool attacked = false;

		public void Move(Node node)
		{
			currentNode.RemoveOccupant();
			node.SetOccupant(this);
			currentNode = node;
			GameController.instance.UnitMadeAction();
			transform.position = node.transform.position + new Vector3(0, 0.5f, 0);
			movesLeft--;
		}

		public bool HasMovesLeft()
		{
			return movesLeft > 0;
		}

		#region spawn

		public void SpawnEntity(Node node)
		{
			moves = movesLeft = 3;
			currentNode = node;
			node.SetOccupant(this);
		}

		#endregion

	}
}