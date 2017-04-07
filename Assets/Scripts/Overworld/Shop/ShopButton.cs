using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Overworld.Shops {
	public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		public Text unitName;
		public Text unitCost;
		public Image img;
		public GameObject locked;
		Button button;

		Action<int, int> CB;
		public int ID;
		int cost;

		Item item;
		//Tooltip tooltip;

		void Awake() {
			button = GetComponentInChildren<Button>();
			//tooltip = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<Tooltip>();
		}

		public ShopButton Setup(Action<int, int> cb, Item item) {
			CB = cb;
			ID = item.ID;
			this.item = item;
			cost = item.GoldCost;
			unitName.text = item.Title;
			unitCost.text = "" + item.GoldCost;
			img.sprite = item.Sprite;
			return this;
		}

		public void Clicked() {
			CB(ID, cost);
		}

		public void IsUnlocked() {
			button.interactable = true;
			locked.SetActive(false);
		}

		public void NotUnlocked() {
			button.interactable = false;
			locked.SetActive(true);
		}

		public void CheckCanAfford(int goldCount) {
			if(goldCount >= item.GoldCost) {
				unitCost.color = Color.black;
			}
			else {
				unitCost.color = Color.red;
			}
		}

		public void OnPointerEnter(PointerEventData eventData) {
			Tooltip.instance.Activate(item);
		}

		public void OnPointerExit(PointerEventData eventData) {
			Tooltip.instance.Deactivate();
		}
	}
}