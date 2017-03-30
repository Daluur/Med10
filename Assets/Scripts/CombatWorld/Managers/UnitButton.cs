using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatWorld.Utility;
using UnityEngine.EventSystems;

namespace CombatWorld.Units {
	public class UnitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		public Button button;
		public Text unitName;
		public Text cost;
		public Image image;

		CombatData data;
		Action<CombatData> callback;

		public UnitButton Setup(Item unit, Action<CombatData> action) {
			this.unitName.text = unit.Title;
			this.cost.text = unit.SummonCost.ToString();
			this.image.sprite = unit.Sprite;
			data = new CombatData(unit);
			button.onClick.AddListener(Clicked);
			callback = action;
			return this;
		}

		void Clicked() {
			callback(data);
		}

		public void CheckCost(int summonPoints) {
			if(summonPoints < data.cost) {
				button.interactable = false;
			}
			else {
				button.interactable = true;
			}
		}

		public bool CanAfford(int summonPoints) {
			return summonPoints >= data.cost;
		}

		public void OnPointerEnter(PointerEventData eventData) {
			TooltipHandler.instance.CreateTooltip(transform.position, data);
		}
		public void OnPointerExit(PointerEventData eventData) {
			TooltipHandler.instance.CloseTooltip();
		}
	}
}