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

		public GameObject unit;

		protected override void Setup()
		{
			GameController.instance.AddTeamNode(this, team);
			base.Setup();
		}

		#region input

		protected override void HandleInput()
		{
			Debug.Log("here2");
			switch (state)
			{
				case HighlightState.None:
					break;
				case HighlightState.Selectable:
					SummonUnit();
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

		#region summmon

		void SummonUnit()
		{
			GameObject spawnedUnit = Instantiate(unit, transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
			spawnedUnit.GetComponent<Unit>().SpawnEntity(this);
		}

		#endregion
	}
}