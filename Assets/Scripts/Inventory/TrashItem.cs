using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashItem : MonoBehaviour, IDropHandler  {
	private Inventory inv;

	void Start() {
		inv = GameObject.FindGameObjectWithTag(TagConstants.VERYIMPORTANTOBJECT).GetComponent<Inventory> ();
	}

	public void OnDrop(PointerEventData eventData) {
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();
		inv.items [droppedItem.slot] = new Item ();
		Destroy (droppedItem.gameObject);
	}
}
