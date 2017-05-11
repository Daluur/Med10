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
			type1 = "DummyType",
			type2 = "DummyType",
			unitIDs = new int[]  { 0, 1, 2, 3, 4, 5 },
		},
		new DeckData {
			deckName = "Spooky Spirit",
			difficulty = "Easy",
			id = 0,
			type1 = "Normal",
			unitIDs = new int[] { 12 },
		},
		new DeckData {
			deckName = "Adorable Apparition",
			difficulty = "Easy",
			id = 1,
			type1 = "Water",
			type2 = "Nature",
			unitIDs = new int[] { 2, 6 },
		},
		new DeckData {
			deckName = "Shy Spectre",
			difficulty = "Medium",
			id = 2,
			type1 = "Stone",
			type2 = "",
			unitIDs = new int[] { 10, 11 },
		},
		new DeckData {
			deckName = "Wacky Wraith",
			difficulty = "Easy",
			id = 3,
			type1 = "Nature",
			type2 = "Lightning",
			type3 = "Shadow",
			unitIDs = new int[] { 6, 4, 8 },
		},
		new DeckData {
			deckName = "Ghastly Ghost",
			difficulty = "Hard",
			id = 4,
			type1 = "Fire",
			type2 = "Water",
			type3 = "Shadow/Stone",
			unitIDs = new int[] { 0, 2, 8, 10 },
		},
		new DeckData {
			deckName = "UNUSED",
			difficulty = "Medium",
			id = 17,
			type1 = "Lightning",
			type2 = "Nature",
			unitIDs = new int[] { 0, 2, 12, 13 },
		},
		new DeckData {
			deckName = "UNUSED",
			difficulty = "Medium",
			id = 5,
			type1 = "Water",
			type2 = "Normal",
			unitIDs = new int[] { 4, 6, 12, 13 },
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