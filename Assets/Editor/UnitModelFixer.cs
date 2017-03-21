using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using CombatWorld.Units;

public class UnitModelFixer : EditorWindow {

	static GameObject baseObject;
	static GameObject[] allModels;
	static bool running = false;
	static int number = 0;
	static int length = 100;
	static float startTime;

	[MenuItem("Unit editor/Fixer")]
	public static void Init() {
		if (running) {
			Debug.LogError("Already running! chill");
			return;
		}

		startTime = (float)EditorApplication.timeSinceStartup;
		running = true;
		number = 0;

		if(baseObject == null) {
			baseObject = Resources.Load<GameObject>("EditorObjects/Models/EmptyUnit");
		}
		allModels = Resources.LoadAll<GameObject>("Art/3D/Units/");
		length = allModels.Length;

		foreach (GameObject model in allModels) {
			EditorUtility.DisplayProgressBar("Progress", "Doing: " + number + " of " + length, (float)number / length);
			FixModel(model);
		}

		EditorUtility.ClearProgressBar();
		Debug.LogError("Took: " + (float)(EditorApplication.timeSinceStartup - startTime) + " Seconds");
		running = false;
	}

	public static void FixModel(GameObject model) {
		if (model.GetComponent<Unit>() != null) {
			ChangeLightRange(model);
			ChangeCanvasWidth(model);
			//Add stuff here, if units needs batch change.
			return;
		}
		Object tempObjParent = PrefabUtility.CreateEmptyPrefab("Assets/Resources/Art/3D/Units/temp.prefab");
		Object tempObjModel = PrefabUtility.CreateEmptyPrefab("Assets/Resources/Art/3D/Units/tempm.prefab");

		GameObject tempParent = PrefabUtility.ReplacePrefab(baseObject, tempObjParent);
		GameObject tempModel = PrefabUtility.ReplacePrefab(model, tempObjModel);

		tempModel.AddComponent<AnimationHandler>();

		GameObject sceneModel =  PrefabUtility.InstantiatePrefab(tempModel) as GameObject;
		GameObject sceneParent = PrefabUtility.InstantiatePrefab(tempParent) as GameObject;
		sceneModel.name = model.name;
		sceneModel.transform.parent = sceneParent.transform.GetChild(0);
		var final = PrefabUtility.ReplacePrefab(sceneParent, model);


		DestroyImmediate(sceneModel);
		DestroyImmediate(sceneParent);
		AssetDatabase.DeleteAsset("Assets/Resources/Art/3D/Units/temp.prefab");
		AssetDatabase.DeleteAsset("Assets/Resources/Art/3D/Units/tempm.prefab");
		number--;
		FixModel(final);
	}

	public static void ChangeLightRange(GameObject model) {
		model.GetComponentInChildren<Light>().range = 10;
	}

	public static void ChangeCanvasWidth(GameObject model) {
		model.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(model.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.y, model.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.y);
	}
}
