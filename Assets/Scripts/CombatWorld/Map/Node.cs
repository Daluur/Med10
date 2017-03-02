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

		protected HighlightState state;

		#region setup

		void Start()
		{
			Setup();
		}

		protected virtual void Setup()
		{
			GameController.instance.AddNode(this);
		}

		#endregion

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
			switch (state)
			{
				case HighlightState.None:
					GetComponentInChildren<Renderer>().material.color = Color.gray;
					break;
				case HighlightState.Selectable:
					GetComponentInChildren<Renderer>().material.color = Color.yellow;
					break;
				case HighlightState.Moveable:
					GetComponentInChildren<Renderer>().material.color = Color.green;
					break;
				case HighlightState.NotMoveable:
					GetComponentInChildren<Renderer>().material.color = Color.black;
					break;
				case HighlightState.Attackable:
					GetComponentInChildren<Renderer>().material.color = Color.red;
					break;
				default:
					break;
			}
		}

		public void ResetState()
		{
			SetState(HighlightState.None);
		}

		#endregion

		#region input

		void OnMouseDown()
		{
			HandleInput();
		}

		protected virtual void HandleInput()
		{
			Debug.Log("here");
			switch (state)
			{
				case HighlightState.None:
					break;
				case HighlightState.Selectable:
					break;
				case HighlightState.Moveable:
					break;
				case HighlightState.NotMoveable:
					break;
				case HighlightState.Attackable:
					break;
				default:
					break;
			}
		}

		#endregion

		void OnDrawGizmos()
		{
			if(neighbours == null || neighbours.Count == 0)
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