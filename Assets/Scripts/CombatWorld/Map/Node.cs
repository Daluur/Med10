using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld;
using CombatWorld.Units;
using CombatWorld.Utility;
using Overworld;

namespace CombatWorld.Map {
	public class Node : MonoBehaviour {
		public List<Node> neighbours;

		protected IEntity occupant;

		protected HighlightState state;

		protected Color basicColor;

		#region Setup

		void Awake() {
			basicColor = GetComponentInChildren<Renderer>().material.color;
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

		public bool HasAttackableNeighbour() {
			if (HasUnit()) {
				foreach (Node node in neighbours) {
					if ((node.HasUnit() && node.GetUnit().GetTeam() != GetUnit().GetTeam()) || (node.HasTower() && node.GetOccupant().GetTeam() != GetUnit().GetTeam())) {
						return true;
					}
				}
			}
			return false;
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

		public Tower GetTower() {
			return (Tower)occupant;
		}

		#endregion

		#region States

		public void SetState(HighlightState state) {
			this.state = state;
			switch (state) {
				case HighlightState.None:
					GetComponentInChildren<Renderer>().material.color = basicColor;
					break;
				case HighlightState.Selectable:
					GetComponentInChildren<Renderer>().material.color = Color.yellow;
					break;
				case HighlightState.Summon:
				case HighlightState.Moveable:
					GetComponentInChildren<Renderer>().material.color = Color.green;
					break;
				case HighlightState.NoMoreMoves:
					if (HasAttackableNeighbour() || (GetUnit().IsStoneUnit() && !GetUnit().turnedToStone)) {
						GetComponentInChildren<Renderer>().material.color = Color.blue;
					}
					else {
						GetComponentInChildren<Renderer>().material.color = Color.black;
					}
					break;
				case HighlightState.NotMoveable:
					GetComponentInChildren<Renderer>().material.color = Color.black;
					break;
				case HighlightState.Attackable:
					GetComponentInChildren<Renderer>().material.color = Color.red;
					break;
				case HighlightState.SelfClick:
					GetComponentInChildren<Renderer>().material.color = Color.magenta;
					break;
				default:
					break;
			}
		}

		public HighlightState GetState() {
			return state;
		}

		public void ResetState() {
			SetState(HighlightState.None);
		}

		#endregion
		
		#region Cursor

		private void OnMouseOver() {
			if(CursorSingleton.instance!=null)
				CursorSingleton.instance.SetCursor(state);
		}

		private void OnMouseExit() {
			if(CursorSingleton.instance!=null)
				CursorSingleton.instance.SetCursor();
		}

		#endregion

		#region Input

		private void OnMouseDown() {
			HandleInput();
		}

		public virtual void HandleInput() {
			if (!GameController.instance.AcceptsInput()) {
				return;
			}
			if (HasUnit() && GetUnit().CanMove()) {
				if (TutorialHandler.instance != null) {
					if (TutorialHandler.instance.unitFirst) {
						TutorialHandler.instance.unitFirst = false;
						//TutorialHandler.instance.FirstSelection();
					}
				}
			}
			switch (state) {
				case HighlightState.Selectable:
					GameController.instance.SetSelectedUnit(GetUnit());
					break;
				case HighlightState.Moveable:
					GameController.instance.MoveUnit(this);
					break;
				case HighlightState.NoMoreMoves:
					PlayerData.Instance.timesTryingToSelectUnitWithoutMovesLeft++;
					GameController.instance.cwTriggers.SelectingUnitWithNoMovesLeft();
					GameController.instance.SetSelectedUnit(GetUnit());
					break;
				case HighlightState.Attackable:
					GameController.instance.GetSelectedUnit().Attack(GetOccupant());
					break;
				case HighlightState.SelfClick:
					GameController.instance.NodeGotSelfClick();
					break;
				case HighlightState.NotMoveable:
					if(GetType() == typeof(SummonNode) && !GetUnit().CanMove()){
						PlayerData.Instance.timesTryingToSelectSummon++;
						GameController.instance.cwTriggers.SelectingRecentlySummonedUnit();
					}
					else if(GetUnit().GetTeam() != Team.AI && !GetUnit().CanMove()) {
						PlayerData.Instance.timesTryingToSelectUnitWithoutMovesLeft++;
						GameController.instance.cwTriggers.SelectingUnitWithNoMovesLeft();
					}
					else if (GameController.instance.GetSelectedUnit().GetTeam() == Team.Player && GetOccupant().GetTeam() == Team.AI){
						PlayerData.Instance.timesTryingToAttack++;
						GameController.instance.cwTriggers.TryingToAttackFromRangeTut();
					}
					
					break;
				case HighlightState.None:
					if (GameController.instance.GetSelectedUnit() != null && GameController.instance.GetSelectedUnit().GetTeam() == Team.Player && GetOccupant()!=null && GetOccupant().GetTeam() == Team.AI){
						PlayerData.Instance.timesTryingToAttack++;
						GameController.instance.cwTriggers.TryingToAttackFromRangeTut();
					}
					if (GetOccupant() != null && GetOccupant().GetTeam() == Team.AI) {
						PlayerData.Instance.timesTriedToSelectEnemyUnits++;
					}
					break;
				default:
					break;
			}
			AudioHandler.instance.PlayClick();
			GameController.instance.GotInput();
		}

		#endregion

		#region InEditorThings

		void OnDrawGizmos() {
			if (neighbours == null || neighbours.Count == 0) {
				return;
			}
			CleanNeighbours();
			foreach (Node node in neighbours) {
				if (node.neighbours.Contains(this)) {
					//double connection
					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0), node.transform.position + new Vector3(0, 0.5f, 0));
				}
				else {
					//single connection
					Gizmos.color = Color.red;
					Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0), node.transform.position + new Vector3(0, 0.5f, 0));
				}
			}
		}

		void OnDrawGizmosSelected() {
			if (neighbours == null || neighbours.Count == 0) {
				return;
			}
			CleanNeighbours();
			foreach (Node node in neighbours) {
				if (node.neighbours.Contains(this)) {
					//double connection
					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0), node.transform.position + new Vector3(0, 0.5f, 0));
				}
				else {
					//single connection
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0), node.transform.position + new Vector3(0, 0.5f, 0));
				}
			}
		}

		void CleanNeighbours() {
			for (int i = neighbours.Count - 1; i >= 0; i--) {
				if (neighbours[i] == null) {
					neighbours.RemoveAt(i);
				}
			}
		}

		#endregion
	}
}