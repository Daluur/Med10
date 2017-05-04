using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Units;

namespace CombatWorld.Utility {
	public class UnitTooltip : MonoBehaviour {

		public Text tooltip;
		private string data;
		private string color;
		private string special;

		//BR, TR, TL, BL
		Vector2[] pivots = new Vector2[] { new Vector2(-0.3f, 1.15f), new Vector2(-0.3f, -0.15f), new Vector2(1.3f, -0.15f), new Vector2(1.3f, 1.15f) };
		Vector2 screenSize;
		private RectTransform tooltipRect;
		float addValue = 100;

		void Start() {
			tooltipRect = GetComponent<RectTransform>();
			screenSize = new Vector2(Screen.width / 2, Screen.height / 2);
			gameObject.SetActive(false);
		}

		void SetPivot() {
			if (Input.mousePosition.x > screenSize.x) {
				//in right side
				if (Input.mousePosition.y > screenSize.y) {
					//in top
					tooltipRect.pivot = pivots[3];
				}
				else {
					//in bottom
					tooltipRect.pivot = pivots[2];
				}
			}
			else {
				//in left side
				if (Input.mousePosition.y > screenSize.y) {
					//in top
					tooltipRect.pivot = pivots[0];
				}
				else {
					//in bottom
					tooltipRect.pivot = pivots[1];
				}
			}
		}

		public void SetData(Vector3 pos, Unit unit) {
			SetPivot();
			switch (unit.data.GetStringFromType(unit.data.type)) {
				case "Fire":
					color = "#ff0000";
					break;
				case "Water":
					color = "#0099ff";
					break;
				case "Nature":
					color = "#00cc00";
					break;
				case "Lightning":
					color = "#ffff66";
					break;
				default:
					color = "#cc6600";
					break;
				}

			if (unit.data.shadow == true) {
					color = "#cc33ff";
					special = "<color=" + color + ">(Shadow)</color>";
				}
			else if (unit.data.stone == true) {
					color = "#ffffff";
					special = "<color=" + color + ">(Stone)</color>";
				} 
				else {
					special = "";
				}

			transform.position = new Vector3(pos.x, pos.y, pos.z);
			data = "<color="+color+"><b>" + unit.data.name + "</b></color>\n\n<b>Health: </b>" + unit.GetHealth() + " / " + unit.data.healthValue + "\n<b>Type: </b>" + unit.data.GetStringFromType(unit.data.type) + " " + special + "\n<b>Moves: </b>" + unit.GetMoveDistance() + "\n<b>Summon Cost: </b>" + unit.data.cost;
			tooltip.text = data;
			gameObject.SetActive(true);
		}

		public void SetData(Vector3 pos, CombatData cData) {
			SetPivot();
			switch (cData.GetStringFromType(cData.type)) {
			case "Fire":
				color = "#ff0000";
				break;
			case "Water":
				color = "#0099ff";
				break;
			case "Nature":
				color = "#00cc00";
				break;
			case "Lightning":
				color = "#ffff66";
				break;
			default:
				color = "#cc6600";
				break;
			}

			if (cData.shadow == true) {
				color = "#cc33ff";
				special = "<color=" + color + ">(Shadow)</color>";
			}
			else if (cData.stone == true) {
				color = "#ffffff";
				special = "<color=" + color + ">(Stone)</color>";
			} 
			else {
				special = "";
			}

			transform.position = new Vector3(pos.x + addValue, pos.y, pos.z);
			data = "<color="+color+"><b>" + cData.name + "</b></color>\n\n<b>Type: </b>" + cData.GetStringFromType(cData.type) + " " + special + "\n<b>Attack: </b>" + cData.attackValue + "\n<b>Health: </b> " + cData.healthValue + "\n<b>Moves: </b>" + cData.moveDistance;
			tooltip.text = data;
			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}
	}
}