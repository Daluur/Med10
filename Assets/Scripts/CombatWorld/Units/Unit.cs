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
			GameController.instance.UnitMadeAction();
			transform.position = node.transform.position + new Vector3(0, 1, 0);
		}

		#region spawn

		public void SpawnEntity(Node node)
		{
			currentNode = node;
			transform.position = node.transform.position + new Vector3(0, 1, 0);
			node.SetOccupant(this);
		}

		#endregion

	}
}