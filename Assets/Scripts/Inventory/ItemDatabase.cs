using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemDatabase {
	private List<Item> database = new List<Item>();
	private JsonData itemData;

	public ItemDatabase() {
		/* -- Read text from Items.json into the JsonData object for temporary storage -- //
		// -- and then constructs the item database from this data					   -- */
		itemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
		ConstructItemDtabase ();
	}

	/* -- A function which fetches item information from the database based on an input id -- */
	public Item FetchItemByID(int id) {
		for (int i = 0; i < database.Count; i++) {
			if (database[i].ID == id) {
				return database[i];
			}		
		}
		return null;
	}

	/// <summary>
	/// Returns all the items in the database, used for the editor tool.
	/// </summary>
	/// <returns></returns>
	public List<Item> GetAllItems() {
		return database;
	}

	/* -- Construction of the item database 					 -- //
	// -- Adds a new entry into the database list from json data -- */
	void ConstructItemDtabase() {
		for (int i = 0; i < itemData.Count; i++) {
			database.Add(new Item(
			(int)itemData[i]["id"], 
			itemData[i]["title"].ToString(), 
			itemData[i]["type"].ToString(),
			itemData[i]["transform"].ToString(),
			(int)itemData[i]["stats"]["attack"], 
			(int)itemData[i]["stats"]["health"], 
			(int)itemData[i]["stats"]["moves"],
			(int)itemData[i]["stats"]["summonCost"],
			(int)itemData[i]["goldCost"],
			itemData[i]["description"].ToString(),
			(bool)itemData[i]["stackable"], 
			itemData[i]["slug"].ToString()
			));
		}
	}
}

/* -- Class for the item itself (unit) -- */
public class Item {
	public int ID { get; set; }
	public string Title { get; set; }
	public string Type { get; set; }
	public string Transform { get; set; }
	public int Attack { get; set; }
	public int Health { get; set; }
	public int Moves { get; set; }
	public int SummonCost { get; set; }
	public int GoldCost { get; set; }
	public string Description { get; set; }
	public bool Stackable { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }
	public GameObject Model { get; set; }

	/* -- Constructor which also loads the appropriate sprite from the Resources folder -- */
	public Item(int id, string tit, string typ, string tra, int att, int hp, int mov, int summonCost, int goldCost, string des, bool stack, string slug) {
		this.ID = id;
		this.Title = tit;
		this.Type = typ;
		this.Transform = tra;
		this.Attack = att;
		this.Health = hp;
		this.Description = des;
		this.Stackable = stack;
		this.Moves = mov;
		this.SummonCost = summonCost;
		this.GoldCost = GoldCost;
		this.Slug = slug;
		this.Sprite = Resources.Load<Sprite> ("Art/2D/Units/" + slug);
		this.Model = Resources.Load<GameObject>("Art/3D/Units/" + slug);
	}

	/* -- Set to -1 to have an empty item slot in the inventory -- */
	public Item() {
		this.ID = -1;
	}

}