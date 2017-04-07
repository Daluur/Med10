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
			difficulty = "dummyDifficulty",
			id = -1,
			type = "DummyType",
			unitIDs = new int[]  { 0, 1, 2, 3, 4, 5 },
		},
		new DeckData {
			deckName = "Fire",
			difficulty = "Easy",
			id = 0,
			type = "Fire",
			unitIDs = new int[] { 0, 1, 12, 13 },
		},
		new DeckData {
			deckName = "Water",
			difficulty = "Easy",
			id = 1,
			type = "Water",
			unitIDs = new int[] { 2, 3, 12, 13 },
		},
		new DeckData {
			deckName = "Lightning",
			difficulty = "Easy",
			id = 2,
			type = "Lightning",
			unitIDs = new int[] { 4, 5, 12, 13 },
		},
		new DeckData {
			deckName = "Nature",
			difficulty = "Easy",
			id = 3,
			type = "Nature",
			unitIDs = new int[] { 6, 7, 12, 13 },
		},
		new DeckData {
			deckName = "Shadow",
			difficulty = "Easy",
			id = 4,
			type = "Shadow",
			unitIDs = new int[] { 8, 9, 12, 13 },
		},
		new DeckData {
			deckName = "Stone",
			difficulty = "Easy",
			id = 5,
			type = "Stone",
			unitIDs = new int[] { 10, 11 },
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