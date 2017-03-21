using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Units;

namespace CombatWorld.Utility {
	public class UnitTooltip : MonoBehaviour {

		public Text unitName;
		public Text unitType;
		public Text unitSpecial;
		public Text unitMoves;

		public void SetData(Vector3 pos, Unit unit) {
			transform.position = pos;
			unitName.text = unit.data.name;
			unitType.text = "Type: " + unit.data.GetStringFromType(unit.data.type);
			unitSpecial.text = "Special: " + (unit.data.shadow ? "Shadow" : unit.data.stone ? "Stone" : "None");
			unitMoves.text = "Moves: " + unit.GetMoveDistance();
			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}
	}
}