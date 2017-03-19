using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld.Map {
	public class Node : MonoBehaviour {
		public List<Node> neighbours;

		protected IEntity occupant;

		protected HighlightState state;

		#region Setup

		void Awake() {
			Setup();
		}

		protected virtual void Setup() {
			GameController.instance.AddNode(this);
		}

		#endregion

		#region Neighbours

		public List<Node> GetNeighbours() {
			return neighbours;
		}

		#endregion

		#region occupantCode

		public void SetOccupant(IEntity occupant) {
			this.occupant = occupant;
		}

		public void RemoveOccupant() {
			occupant = null;
		}

		public bool HasOccupant() {
			return occupant != null;
		}

		public IEntity GetOccupant() {
			return occupant;
		}

		public bool HasUnit() {
			if(occupant != null && occupant.GetType() == typeof(Unit)) {
				return true;
			}
			return false;
		}

		public Unit GetUnit() {
			return (Unit)occupant;
		}

		public bool HasTower() {
			if (occupant != null && occupant.GetType() == typeof(Tower)) {
				return true;
			}
			return false;
		}

		#endregion

		#region States

		public void SetState(HighlightState state) {
			this.state = state;
			switch (state) {
				case HighlightState.None:
					GetComponentInChildren<Renderer>().material.color = Color.gray;
					break;
				case HighlightState.Selectable:
					GetComponentInChildren<Renderer>().material.color = Color.yellow;
					break;
				case HighlightState.Summon:
				case HighlightState.Moveable:
					GetComponentInChildren<Renderer>().material.color = Color.green;
					break;
				case HighlightState.NoMoreMoves:
					GetComponentInChildren<Renderer>().material.color = Color.blue;
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

		public void ResetState() {
			SetState(HighlightState.None);
		}

		#endregion

		#region Input

		public virtual void HandleInput() {
			switch (state) {
				case HighlightState.Selectable:
					GameController.instance.SetSelectedUnit(GetUnit());
					break;
				case HighlightState.Moveable:
					GameController.instance.MoveUnit(this);
					break;
				case HighlightState.NoMoreMoves:
					GameController.instance.SetSelectedUnit(GetUnit());
					break;
				case HighlightState.Attackable:
					GameController.instance.GetSelectedUnit().Attack(GetOccupant());
					break;
				default:
					break;
			}
			GameController.instance.GotInput();
		}

		#endregion

		void OnDrawGizmos() {
			if (neighbours == null || neighbours.Count == 0) {
				return;
			}
			foreach (var item in neighbours) {
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0), item.transform.position - new Vector3(0, 0.5f, 0));
			}
		}
	}
}