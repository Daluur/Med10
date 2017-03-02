using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Utility;

namespace CombatWorld.Units
{
	public class Entity : MonoBehaviour
	{
		//TODO implement health.

		protected Team team;
		protected Node currentNode;

		public Team GetTeam()
		{
			return team;
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