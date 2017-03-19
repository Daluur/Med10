using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;
using System;

public class NodeEditor : EditorWindow {

	static NodeEditor window;

	GameObject BasicNode;
	GameObject SummonNode;
	GameObject TowerNode;
	GameObject TowerObj;

	[MenuItem("Node Editor/Editor")]
	static void Init() {
		window = (NodeEditor)EditorWindow.GetWindow<NodeEditor>();
		window.Show();
	}

	void OnGUI() {
		GetNodesFromResources();
		if (!GetSelectedObject()) {
			NotANode();
		}
		else {
			ANode();
		}
	}

	#region Setup

	bool GetSelectedObject() {
		if (Selection.activeGameObject == null) {
			return false;
		}
		if (Selection.activeGameObject.GetComponent<Node>()) {
			return true;
		}
		if (Selection.activeTransform.parent != null) {
			if (Selection.activeTransform.parent.GetComponent<Node>()) {
				Selection.activeTransform = Selection.activeTransform.parent;
				return true;
			}
			else if (Selection.activeTransform.parent.parent != null) {
				if (Selection.activeTransform.parent.parent.GetComponent<Node>()) {
					Selection.activeTransform = Selection.activeTransform.parent.parent;
					return true;
				}
			}
		}
		return false;
	}

	void GetNodesFromResources() {
		if (BasicNode == null) {
			BasicNode = Resources.Load<GameObject>("EditorObjects/Nodes/Node");
		}
		if (SummonNode == null) {
			SummonNode = Resources.Load<GameObject>("EditorObjects/Nodes/SummonNode");
		}
		if (TowerNode == null) {
			TowerNode = Resources.Load<GameObject>("EditorObjects/Nodes/TowerNode");
		}
		if (TowerObj == null) {
			TowerObj = Resources.Load<GameObject>("EditorObjects/Nodes/Tower");
		}
	}

	void RepaintThis() {
		Repaint();
	}

	#endregion

	void NotANode() {
		if(Selection.activeGameObject == null) {
			GUILayout.Label("No object is selected :");
			return;
		}

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Replace with node")) {
			ReplaceWith(NodeTypes.Node);
			return;
		}
		if (GUILayout.Button("Replace with summonnode")) {
			ReplaceWith(NodeTypes.SummonNode);
			return;
		}
		if (GUILayout.Button("Replace with towernode")) {
			ReplaceWith(NodeTypes.TowerNode);
			return;
		}
		EditorGUILayout.EndHorizontal();
	}

	void ANode() {
		NodeTypes selectedType = GetTypeFromSelectedObject();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Currently selected node: ");
		GUILayout.Label(GetNodeNameFromType(selectedType));
		EditorGUILayout.EndHorizontal();

		switch (selectedType) {
			case NodeTypes.Node:
				IsBasicNode();
				break;
			case NodeTypes.SummonNode:
				IsSummonNode();
				break;
			case NodeTypes.TowerNode:
				IsTowerNode();
				break;
			default:
				break;
		}
		NeighbourSetup();
	}

	void IsBasicNode() {
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Change to summon node")) {
			ReplaceWith(NodeTypes.SummonNode, true);
			return;
		}
		if (GUILayout.Button("Change to tower node")) {
			ReplaceWith(NodeTypes.TowerNode, true);
			return;
		}
		EditorGUILayout.EndHorizontal();
	}

	void IsTowerNode() {
		if (GUILayout.Button("Change to basic node")) {
			ReplaceWith(NodeTypes.Node, true);
			return;
		}

		SimpleTeam team = Team2SimpleTeam(Selection.activeGameObject.GetComponentInChildren<Tower>().GetTeam());

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Belongs to: ");
		team = (SimpleTeam)EditorGUILayout.EnumPopup(team);
		Selection.activeGameObject.GetComponentInChildren<Tower>().SetTeam(SimpleTeam2Team(team));
		EditorGUILayout.EndHorizontal();
	}

	void IsSummonNode() {
		if (GUILayout.Button("Change to basic node")) {
			ReplaceWith(NodeTypes.Node, true);
			return;
		}

		SimpleTeam team = Team2SimpleTeam(Selection.activeGameObject.GetComponent<SummonNode>().team);

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Belongs to: ");
		team = (SimpleTeam)EditorGUILayout.EnumPopup(team);
		Selection.activeGameObject.GetComponent<SummonNode>().team = SimpleTeam2Team(team);
		EditorGUILayout.EndHorizontal();
	}

	void NeighbourSetup() {
		Node current = Selection.activeGameObject.GetComponent<Node>();
		int singleConnections = 0;
		int doubleConnections = 0;
		foreach (Node node in current.neighbours) {
			if (node.neighbours.Contains(current)) {
				doubleConnections++;
			}
			else {
				singleConnections++;
			}
		}
		GUILayout.Label("Has " + doubleConnections + " double connections. " + "Has " + singleConnections + " single connections");
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Double connection")) {
			assigningNeighbour = true;
			SettingUpNeighbour(false);
		}
		if(GUILayout.Button("Single connection")) {
			assigningNeighbour = true;
			SettingUpNeighbour(true);
		}
		EditorGUILayout.EndHorizontal();
		if (assigningNeighbour) {
			GUILayout.Label("Select a new Neighbour!");
		}

		if(GUILayout.Button("Remove all neighbours!")) {
			foreach (Node node in Selection.activeGameObject.GetComponent<Node>().neighbours) {
				if (node.neighbours.Contains(Selection.activeGameObject.GetComponent<Node>())) {
					node.neighbours.Remove(Selection.activeGameObject.GetComponent<Node>());
				}
			}
			Selection.activeGameObject.GetComponent<Node>().neighbours = new List<Node>();
		}
	}

	void SettingUpNeighbour(bool single) {

		singleAssign = single;
		assigningNeighbour = true;
		selected = Selection.activeGameObject;
	}

	bool singleAssign = false;
	bool assigningNeighbour = false;
	GameObject selected;

	private void OnSelectionChange() {
		if (assigningNeighbour) {
			Node newNeighbour;
			if (GetSelectedObject()) {
				newNeighbour = Selection.activeGameObject.GetComponent<Node>();
				if(newNeighbour == null) {
					newNeighbour = Selection.activeTransform.parent.GetComponent<Node>();
					if (newNeighbour == null) {
						newNeighbour = Selection.activeTransform.parent.parent.GetComponent<Node>();
					}
				}
			}
			else {
				Debug.LogError("Gameobject is not have a node");
				ReselectNode();
				return;
			}
			if (singleAssign) {
				if (!selected.GetComponent<Node>().neighbours.Contains(newNeighbour)) {
					selected.GetComponent<Node>().neighbours.Add(newNeighbour);
				}
				else {
					Debug.LogWarning("Node already had a connection to the selected node! does nothing.");
				}
				ReselectNode();
				return;
			}
			else {
				if (!selected.GetComponent<Node>().neighbours.Contains(newNeighbour)) {
					selected.GetComponent<Node>().neighbours.Add(newNeighbour);
				}
				else {
					Debug.LogWarning("Node already had a connection to the selected node! does nothing.");
				}
				if (!newNeighbour.neighbours.Contains(selected.GetComponent<Node>())) {
					newNeighbour.neighbours.Add(selected.GetComponent<Node>());
				}
				else {
					Debug.LogWarning("Newly selected node already had a connection to this! does nothing.");
				}
				ReselectNode();
				return;
			}
		}
		else {
			Repaint();
		}
	}

	void ReselectNode() {
		Selection.activeGameObject = selected;
		assigningNeighbour = false;
		selected = null;
		Repaint();
	}

	#region replaceCode

	void ReplaceWith(NodeTypes type, bool wasNode = false) {
		GameObject temp;
		switch (type) {
			case NodeTypes.SummonNode:
				temp = Instantiate(SummonNode) as GameObject;
				break;
			case NodeTypes.TowerNode:
				temp = CreateTowerNode();
				break;
			case NodeTypes.Node:
			default:
				temp = Instantiate(BasicNode) as GameObject;
				break;
		}

		Transform oldObj = Selection.activeTransform;
		temp.transform.SetParent(oldObj.parent);
		temp.transform.position = oldObj.position;
		Node newNode = temp.GetComponent<Node>();
		Node oldNode = oldObj.GetComponent<Node>();
		if (wasNode) {
			foreach (Node node in oldNode.neighbours) {
				newNode.neighbours.Add(node);
				if (node.neighbours.Contains(oldNode)) {
					node.neighbours.Remove(oldNode);
					node.neighbours.Add(newNode);
				}
			}
		}
		DestroyImmediate(Selection.activeGameObject);
		Selection.activeGameObject = temp;
	}

	GameObject CreateTowerNode() {
		GameObject node = Instantiate(TowerNode) as GameObject;
		GameObject tower = Instantiate(TowerObj, node.transform) as GameObject;
		tower.GetComponent<Tower>().SetCurrentNode(node.GetComponent<Node>());
		node.GetComponent<Node>().SetOccupant(tower.GetComponent<Tower>());
		return node;
	}

	#endregion

	#region helperFunctions

	string GetNodeNameFromType(NodeTypes type) {
		switch (type) {
			case NodeTypes.Node:
				return "Basic node";
			case NodeTypes.SummonNode:
				return "Summon node";
			case NodeTypes.TowerNode:
				return "Tower node";
			default:
				return "Error";
		}
	}

	NodeTypes GetTypeFromSelectedObject() {
		if (Selection.activeGameObject.GetComponent<SummonNode>() != null) {
			return NodeTypes.SummonNode;
		}
		if (Selection.activeGameObject.GetComponentInChildren<Tower>() != null) {
			if (Selection.activeGameObject.GetComponentInChildren<Tower>().GetNode() == null) {
				Selection.activeGameObject.GetComponentInChildren<Tower>().SetCurrentNode(Selection.activeGameObject.GetComponent<Node>());
				Debug.Log("Assinged node to tower.");
			}
			return NodeTypes.TowerNode;
		}
		else {
			return NodeTypes.Node;
		}
	}

	SimpleTeam Team2SimpleTeam(Team team) {
		switch (team) {
			case Team.Player:
				return SimpleTeam.Player;
			case Team.AI:
				return SimpleTeam.AI;
			case Team.NONE:
			default:
				throw new NotImplementedException();
		}
	}
	Team SimpleTeam2Team(SimpleTeam team) {
		switch (team) {
			case SimpleTeam.Player:
				return Team.Player;
			case SimpleTeam.AI:
				return Team.AI;
			default:
				throw new NotImplementedException();
		}
	}

	enum NodeTypes {
		Node,
		SummonNode,
		TowerNode,
	}

	enum SimpleTeam {
		Player,
		AI,
	}

	#endregion
}
