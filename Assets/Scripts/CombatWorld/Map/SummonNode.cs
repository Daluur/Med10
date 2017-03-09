using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld.Map {
	public class SummonNode : Node {
		public Team team;

		protected override void Setup() {
			GameController.instance.AddTeamNode(this, team);
			base.Setup();
		}

		#region input

		public override void HandleInput() {
			switch (state) {
				case HighlightState.Selectable:
					GameController.instance.SetSelectedUnit(GetUnit());
					break;
				case HighlightState.Moveable:
					GameController.instance.SummonNodeClickHandler(this);
					break;
				case HighlightState.NoMoreMoves:
					GameController.instance.SetSelectedUnit(GetUnit());
					break;
				case HighlightState.None:
				case HighlightState.NotMoveable:
				case HighlightState.Attackable:
				default:
					base.HandleInput();
					return;
			}
			GameController.instance.GotInput();
		}

		#endregion
	}
}