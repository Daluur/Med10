﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour {

	public int ID;
	public string Name;
	public MapTypes type;

	public enum MapTypes {
		Grass,
		Snow,
	}
}
