using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Units;
using CombatWorld.Utility;
using Overworld;

namespace CombatWorld.Map {
	public class SummonNode : Node {
		public Team team;

		protected override void Setup() {
			GameController.instance.AddTeamNode(this, team);
			base.Setup();
		}

		#region input

		public override void HandleInput() {
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
				case HighlightState.Summon:
					GameController.instance.SummonNodeClickHandler(this);
					break;
				case HighlightState.NoMoreMoves:
					GameController.instance.SetSelectedUnit(GetUnit());
					break;
				case HighlightState.Moveable:
				case HighlightState.None:
				case HighlightState.NotMoveable:
				case HighlightState.Attackable:
				case HighlightState.SelfClick:
				default:
					base.HandleInput();
					return;
			}
			AudioHandler.instance.PlayClick();
			GameController.instance.GotInput();
		}

		#endregion
	}
}