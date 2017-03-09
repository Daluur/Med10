using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashItem : MonoBehaviour, IDropHandler  {
	private Inventory inv;

	void Start() {
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
	}

	public void OnDrop(PointerEventData eventData) {
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();
		inv.items [droppedItem.slot] = new Item ();
		Destroy (droppedItem.gameObject);
	}
}
