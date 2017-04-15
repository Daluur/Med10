﻿public static class Constants {
}

public static class CursorTextureConstants {
	public const string NORMALCURSOR = "HandNormal";
	public const string COMBATCURSOR = "HandSword";
	public const string INTERACTIONCURSOR = "HandInteract";
	public const string CURSORFOLDER = "Art/2D/Cursor";
}


public static class LayerConstants {
	public const string GROUNDLAYER = "Ground";
	public const string NODELAYER = "Node";
	public const string INTERACTABLELAYER = "Interactable";
}

public static class TagConstants {
	public const string OVERWORLDPLAYER = "PlayerOW";
	public const string CONTEXTUNITMENU = "ContextUnitMenu";
	public const string BUTTONTEXT = "ButtonText";
	public const string VERYIMPORTANTOBJECT = "VeryImportantObject";
	public const string OWEVENTSYSTEM = "OWEventSystem";
	public const string OWCANVAS = "OWCanvas";
	public const string RECRUITBUTTON = "RecruitButton";
	public const string CURRENCYPICKUP = "CurrencyPickup";
	public const string BUTTONIMAGE = "ButtonImage";
	public const string ICONIMAGE = "IconImage";
	public const string COMBATWORLDUNIT = "CombatUnit";
}

public static class Values {
	public const float InteractDistance = 1.9f;
	public const int SUMMONPOINTSONKILL = 1;
}

public static class DamageConstants {
	public const int EFFECTIVEMULTIPLIER = 2;
	public const float INEFFECTIVEMULTIPLIER = 0.5f;
	public const bool ALLOWRETALIATIONAFTERDEATH = true;
	public const int TOWERHP = 50;
	public const int SUMMONPOINTSPERTURN = 2;
	public const int SUMMONPOINTSPERKILL = 1;
	public const int SUMMONPOINTSPERTOWERKILL = 3;
	public const int STARTSUMMONPOINTS = 8;
	public const int AISTARTSUMMONPOINTS = 9;
	public const bool EFFECTIVEMULT = false;
	public const int EFFECTIVEBONUS = 10;
	public const int INEFFECTIVEPENALTY = 5;

	public const bool ATTACKATSAMETIME = false;
}

public static class StoneUnitOptions {
	public const bool STONEUNITSGETSATTACKASHEALTH = false;
	public const bool STONEUNITTAKESSTATICDMG = true;
	public const bool STONEUNITSGETDOUBLEHEALTH = false;
	public const bool STONEUNITCANRETALIATE = true;
	public const int STONEUNITRETALIATEDMG = 10;
	public const int STONEUNITDMGTAKEN = 5;
}

public static class GameNotificationConstants {
	public const string NOTENOUGHGOLD = "Not enough gold to perform this action";
	public const string AREAISOBSTRUCTED = "The area is obstructed it is not possible to move here";
	public const string NOTENOUGHINVENTORYSPACE = "There is no more room in the inventory";
	public const string TOOFARAWAY = "This cannot be done, it is too far away";
	public const string OBSTRUCTEDNODE = "I cannot move there";
	public const string ENTEREDRANDOMENCOUNTEAREA = "You have now entered an area containing random enemies";
	public const string EXITEDRANDOMENCOUNTEAREA = "You have now exited an area containing random enemies";
	public const string GAMEWASSAVED = "The game has been saved!";
}