using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CombatWorld.Units;

namespace CombatWorld.Utility {
	public class TooltipHandler : Singleton<TooltipHandler> {

		public UnitTooltip tooltip;

		public void CreateTooltip(Vector3 pos, Unit unit) {
			tooltip.SetData(Camera.main.WorldToScreenPoint(pos), unit);
		}

		public void CreateTooltip(Vector3 pos, CombatData data) {
			tooltip.SetData(pos, data);
		}

		public void CloseTooltip() {
			tooltip.Hide();
		}
	}
}