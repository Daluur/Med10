﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {
	private Inventory inv;
	[HideInInspector]
	public int id;

	void Start() {
		inv = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<Inventory> ();
	}

	public void OnDrop(PointerEventData eventData) {
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();
		if (inv.items[id].ID == -1) {
			inv.items [droppedItem.slot] = new Item ();
			AudioHandler.instance.MoveInventorySound();
			inv.items [id] = droppedItem.item;
			droppedItem.slot = id;
		}
		else if(droppedItem.slot != id) {
			Transform item = this.transform.GetChild (1);
			AudioHandler.instance.SwapInventorySound();
			item.GetComponent<ItemData> ().slot = droppedItem.slot;
			item.transform.SetParent (inv.slots[droppedItem.slot].transform);
			item.transform.position = inv.slots [droppedItem.slot].transform.position;

			droppedItem.slot = id;
			droppedItem.transform.SetParent (this.transform);
			droppedItem.transform.position = this.transform.position;

			inv.items [droppedItem.slot] = item.GetComponent<ItemData> ().item;
			inv.items [id] = droppedItem.item;
		}
	}
}
