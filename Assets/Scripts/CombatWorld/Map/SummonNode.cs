using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld.Map
{
	public class SummonNode : Node
	{
		public Team team;

		protected override void Setup()
		{
			GameController.instance.AddTeamNode(this, team);
			base.Setup();
		}

		#region input

		protected override void HandleInput()
		{
			switch (state)
			{
				case HighlightState.None:
					break;
				case HighlightState.Selectable:
					GameController.instance.SelectedUnit(GetOccupant());
					break;
				case HighlightState.Moveable:
					GameController.instance.SummonNodeClickHandler(this);
					break;
				case HighlightState.NotMoveable:
					break;
				case HighlightState.Attackable:
					break;
				default:
					break;
			}
			GameController.instance.GotInput();
		}

		#endregion
	}
}