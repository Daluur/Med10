using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	GameObject inventoryPanel;
	GameObject slotPanel;
	ItemDatabase database;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	int slotAmount;
	public List<Item> items = new List<Item> ();
	public List<GameObject> slots = new List<GameObject> ();

	void Awake() {
		database = new ItemDatabase();
	}

	void Start() {
		slotAmount = 12;
		inventoryPanel = GameObject.Find ("Inventory Panel");
		slotPanel = inventoryPanel.transform.FindChild ("Slot Panel").gameObject;
		for (int i = 0; i < slotAmount; i++) {
			items.Add (new Item());
			slots.Add (Instantiate (inventorySlot,slotPanel.transform));
			slots [i].GetComponent<Slot> ().id = i;
			//slots [i].transform.SetParent (slotPanel.transform);
			if (i < Values.NUMOFUNITSTOBRINGTOCOMBAT) {
				slots[i].transform.FindChild ("Glow").gameObject.SetActive(true);
			} 
		}

		List<int> savedInventory = SaveLoadHandler.Instance.GetInventory();
		for (int i = 0; i < savedInventory.Count; i++) {
			if (savedInventory[i] != -1) {
				AddItemAtSlot(savedInventory[i], i);
			}
		}
	}

	public void AddItem(int id) {
		Item itemToAdd = database.FetchItemByID (id);
		if (itemToAdd.Stackable && CheckItemInInventory(itemToAdd)) {
			for (int i = 0; i < items.Count; i++) {
				if (items[i].ID == id) {
					ItemData data = slots [i].transform.GetChild (1).GetComponent<ItemData> ();
					data.amount++;
					data.transform.GetChild (1).GetComponent<Text> ().text = data.amount.ToString ();

					break;
				}
			}
		}

		else {
			for (int i = 0; i < items.Count; i++) {
				if (items[i].ID == -1) {
					items [i] = itemToAdd;
					GameObject itemObject = Instantiate (inventoryItem,slots[i].transform,false);
					itemObject.GetComponent<ItemData> ().item = itemToAdd;
					itemObject.GetComponent<ItemData> ().slot = i;
					itemObject.GetComponent<Image> ().sprite = itemToAdd.Sprite;
					itemObject.name = itemToAdd.Title;
					ItemData data = slots[i].transform.GetChild (1).GetComponent<ItemData> ();
					data.amount = 1;
					break;
				} 
			}
		}
	}

	public void AddItemAtSlot(int id, int slotid) {
		Item itemToAdd = database.FetchItemByID(id);
		if (items[slotid].ID == -1) {
			items[slotid] = itemToAdd;
			GameObject itemObject = Instantiate(inventoryItem, slots[slotid].transform, false);
			itemObject.GetComponent<ItemData>().item = itemToAdd;
			itemObject.GetComponent<ItemData>().slot = slotid;
			itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite;
			itemObject.name = itemToAdd.Title;
			ItemData data = slots[slotid].transform.GetChild(1).GetComponent<ItemData>();
			data.amount = 1;
		}
		else {
			Debug.LogError("There were already an item in that slot. Is this called outside of load?");
		}
	}

	public bool HasInventorySpace() {
		foreach (Item item in items) {
			if(item.ID == -1) {
				return true;
			}
		}
		return false;
	}

	bool CheckItemInInventory(Item item) {
		for (int i = 0; i < items.Count; i++) {
			if (items[i].ID == item.ID) {
				return true;
			}
		}
		return false;
	}

	public List<Item> GetFirstXItemsFromInventory(int x) {
		List<Item> toReturn = new List<Item>();
		for (int i = 0; i < x; i++) {
			if(items[i].ID != -1) {
				toReturn.Add(items[i]);
			}
		}
		return toReturn;
	}

	public List<Item> GetEntireInventory() {
		return items;
	}

	public ItemDatabase GetDatabase() {
		return database;
	}
}
