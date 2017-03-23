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
			slots.Add (Instantiate (inventorySlot));
			slots [i].GetComponent<Slot> ().id = i;
			slots [i].transform.SetParent (slotPanel.transform);
		}

		AddItem (0);
		AddItem (1);
		AddItem (1);
		AddItem (1);
		AddItem (1);
	}

	public void AddItem(int id) {
		Item itemToAdd = database.FetchItemByID (id);

		if (itemToAdd.Stackable && CheckItemInInventory(itemToAdd)) {
			for (int i = 0; i < items.Count; i++) {
				if (items[i].ID == id) {
					ItemData data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();
					data.amount++;
					data.transform.GetChild (0).GetComponent<Text> ().text = data.amount.ToString ();

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
					ItemData data = slots[i].transform.GetChild (0).GetComponent<ItemData> ();
					data.amount = 1;
					break;
				} 
			}
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

	public ItemDatabase GetDatabase() {
		return database;
	}
}
