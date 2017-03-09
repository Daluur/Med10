using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
	public Item item;
	public int amount;
	public int slot;

	private Inventory inv;
	private Tooltip tooltip;

	void Start() {
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		tooltip = inv.GetComponent<Tooltip> ();
	}

	/* -- When a drag event is started, if there is an object in the slot  -- //
	// -- Change the parent from the slot (to not draw behind other slots) -- //
	// -- Then set the position to the current mouse position (eventData)  -- //
	// -- Disables the objects ability to 'receive' raycasts			   -- */
	public void OnBeginDrag(PointerEventData eventData) {
		if (item != null) {
			this.transform.SetParent (this.transform.parent.parent);
			this.transform.position = eventData.position;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}

	/* -- During a drag event 									 -- //
	// -- Transform position to make the object follow the mouse -- */
	public void OnDrag(PointerEventData eventData) {
		if (item != null) {
			this.transform.position = eventData.position;
		}
	}

	/* -- When you stop draggin (let go) 				 	 -- //
	// -- Set the parent as the slot in current position 	 -- //
	// -- Set the position to be the same as the parent slot -- //
	// -- Re-enable the ability to receive raycasts 		 -- */ 
	public void OnEndDrag(PointerEventData eventData) {
		this.transform.SetParent(inv.slots[slot].transform);
		this.transform.position = inv.slots [slot].transform.position;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}
		
	public void OnPointerEnter(PointerEventData eventData) {
		tooltip.Activate (item);
	}

	public void OnPointerExit(PointerEventData eventData) {
		tooltip.Deactivate ();
	}
}
