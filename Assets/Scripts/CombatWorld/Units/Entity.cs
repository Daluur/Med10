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

		public Node GetNode()
		{
			return currentNode;
		}
	}
}