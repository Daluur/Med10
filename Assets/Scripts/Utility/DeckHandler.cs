using CombatWorld.Utility;

static class DeckHandler {
	public static DeckData[] decks = new DeckData[] {
		//Create new decks like this.
		/*new DeckData {
			deckName = "dummyName",
			id = -1,
			type = ElementalTypes.NONE,
			unitIDs = new int[]  { 0, 1, 2, 3, 4, 5 },
		},*/
		new DeckData {
			deckName = "dummyName",
			id = -1,
			type = ElementalTypes.NONE,
			unitIDs = new int[]  { 0, 1, 2, 3, 4, 5 },
		},
		new DeckData {
			deckName = "Fire",
			id = 0,
			type = ElementalTypes.Fire,
			unitIDs = new int[] { 0, 1, 12, 13 },
		},
		new DeckData {
			deckName = "Water",
			id = 1,
			type = ElementalTypes.Water,
			unitIDs = new int[] { 2, 3, 12, 13 },
		},
		new DeckData {
			deckName = "Lightning",
			id = 2,
			type = ElementalTypes.Lightning,
			unitIDs = new int[] { 4, 5, 12, 13 },
		},
		new DeckData {
			deckName = "Nature",
			id = 3,
			type = ElementalTypes.Nature,
			unitIDs = new int[] { 6, 7, 12, 13 },
		},
		new DeckData {
			deckName = "Shadow",
			id = 4,
			type = ElementalTypes.NONE,
			unitIDs = new int[] { 8, 9, 12, 13 },
		},
		new DeckData {
			deckName = "Stone",
			id = 5,
			type = ElementalTypes.NONE,
			unitIDs = new int[] { 10, 11, 12, 13 },
		},
	};

	public static DeckData GetDeckFromID(int id) {
		foreach (DeckData deck in decks) {
			if(deck.id == id) {
				return deck;
			}
		}
		UnityEngine.Debug.LogError("ID: " + id + " Do not exist! returns deck[0]");
		return decks[0];
	}
}