using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using Overworld;

public class UnitEditor : EditorWindow {
	public static string name;
	public static UnitType type;

	[MenuItem ("Window/My Window")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		UnitEditor window = EditorWindow.GetWindow<UnitEditor>();
		window.Show();
	}

	void OnGUI () {

/*		GameObject selected = Selection.activeObject as GameObject;
		if ((selected == null || selected.name.Length <= 0)) {
			name = null;
			type = UnitType.Type1;
		}
		else {
			name = selected.GetComponent<Unit>().name;
			type = selected.GetComponent<Unit>().type;
		}*/

		GUILayout.Label ("Unit Editor", EditorStyles.boldLabel);

		name = EditorGUILayout.TextField("Unit name", name);

		type = (UnitType)(EditorGUILayout.EnumPopup("unit type", type));

		if (GUILayout.Button("Create new Unit")) {
			Create();
		}

	}

	[MenuItem("Assets/Create/Add C# Class")]
	static void Create()
	{
		// remove whitespace and minus
		if(name==null)
			return;

		string copyPath = "Assets/Scripts/OverWorld/Units/"+name+".cs";
		Debug.Log("Creating Classfile: " + copyPath);
		if( File.Exists(copyPath) == false ){ // do not overwrite
			using (StreamWriter outfile =
				new StreamWriter(copyPath))
			{
				outfile.WriteLine("using UnityEngine;");
				outfile.WriteLine("using System.Collections;");
				outfile.WriteLine("");
				outfile.WriteLine("namespace Overworld {");
				outfile.WriteLine("\tpublic class "+name+" : Unit {");
				outfile.WriteLine(" ");
				outfile.WriteLine(" // Use this for initialization");
				outfile.WriteLine("\t\tvoid Start () {");
				outfile.WriteLine(" ");
				outfile.WriteLine("\t\t}");
				outfile.WriteLine(" ");
				outfile.WriteLine(" ");
				outfile.WriteLine(" // Update is called once per frame");
				outfile.WriteLine("\t\tvoid Update () {");
				outfile.WriteLine(" ");
				outfile.WriteLine("\t\t}");
				outfile.WriteLine("\t}");
				outfile.WriteLine("}");
			}//File written
		}
		AssetDatabase.Refresh();
		//selected.AddComponent(Type.GetType(name));
	}
}