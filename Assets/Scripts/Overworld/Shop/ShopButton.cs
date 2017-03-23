using System;
using UnityEngine;
using UnityEngine.UI;

namespace Overworld.Shops {
	public class ShopButton : MonoBehaviour {

		public Text unitName;
		public Text unitCost;
		public Image img;

		Action<int, int> CB;
		int ID;
		int cost;

		public void Setup(Action<int, int> cb, Item item) {
			CB = cb;
			ID = item.ID;
			cost = item.GoldCost;
			unitName.text = item.Title;
			unitCost.text = "" + item.GoldCost;
			img.sprite = item.Sprite;
		}

		public void Clicked() {
			CB(ID, cost);
		}
	}
}