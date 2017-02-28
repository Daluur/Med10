using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld.Map
{
	public class Node : MonoBehaviour
	{
		public List<Node> neighbours;

		Entity occupant;

		HighlightState state;

		#region neighbours

		public List<Node> GetNeighbours()
		{
			return neighbours;
		}
		
		#endregion

		#region occupantCode

		public void SetOccupant(Entity occupant)
		{
			this.occupant = occupant;
		}

		public void RemoveOccupant()
		{
			occupant = null;
		}

		public bool HasOccupant()
		{
			return occupant;
		}

		public Entity GetOccupant()
		{
			return occupant;

		}
		#endregion

		#region states

		public void SetState(HighlightState state)
		{
			this.state = state;
			//TODO: visual
		}

		public void ResetState()
		{
			state = HighlightState.None;
		}

		#endregion

		void OnDrawGizmos()
		{
			if(neighbours.Count == 0)
			{
				return;
			}
			foreach (var item in neighbours)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0), item.transform.position - new Vector3(0, 0.5f, 0));
			}
		}
	}
}