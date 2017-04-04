using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollection : Singleton<DataCollection> {

	private void Awake() {
		base.Awake();
		DontDestroyOnLoad(gameObject);
		UnlearnedActions.Add(ActionType.FireTrade);
		UnlearnedActions.Add(ActionType.WaterTrade);
		UnlearnedActions.Add(ActionType.LightningTrade);
		UnlearnedActions.Add(ActionType.NatureTrade);
		UnlearnedActions.Add(ActionType.StoneUsage);
		UnlearnedActions.Add(ActionType.ShadowUsage);
		CurrentlyTeaching.Add(ActionType.FireTrade);
	}

	List<ActionType> UnlearnedActions = new List<ActionType>();
	List<ActionType> LearnedActions = new List<ActionType>();
	List<ActionType> ThinkLearnedActions = new List<ActionType>();
	List<ActionType> CurrentlyTeaching = new List<ActionType>();

	Dictionary<ActionType, int> performedThisCombat = new Dictionary<ActionType, int>();

	public void PerformedAction(ActionType action) {
		if (performedThisCombat.ContainsKey(action)) {
			performedThisCombat[action] = performedThisCombat[action] + 1;
		}
		else {
			performedThisCombat[action] = 1;
		}
	}

	public void CombatEnd() {
		foreach (ActionType action in performedThisCombat.Keys) {
			if (LearnedActions.Contains(action)) {
				continue;
			}
			if (ThinkLearnedActions.Contains(action)) {
				ThinkLearnedActions.Remove(action);
				LearnedActions.Add(action);
				if (CurrentlyTeaching.Contains(action)) {
					CurrentlyTeaching.Remove(action);
				}
				continue;
			}
			if (UnlearnedActions.Contains(action)) {
				UnlearnedActions.Remove(action);
				ThinkLearnedActions.Add(action);
				if (CurrentlyTeaching.Contains(action)) {
					CurrentlyTeaching.Remove(action);
				}
				continue;
			}
		}
		if (CurrentlyTeaching.Count == 0) {
			CurrentlyTeaching.Add(UnlearnedActions[Random.Range(0, UnlearnedActions.Count)]);
		}
		performedThisCombat.Clear();
		DebugInfo();
	}

	void DebugInfo() {
		Debug.Log("End combat analysis");
		Debug.Log("learned actions");
		foreach (ActionType act in LearnedActions) {
			Debug.Log(act.ToString());
		}
		Debug.Log("maybe learned");
		foreach (ActionType act in ThinkLearnedActions) {
			Debug.Log(act.ToString());
		}
		Debug.Log("not learned");
		foreach (ActionType act in UnlearnedActions) {
			Debug.Log(act.ToString());
		}
		Debug.Log("currently teaching");
		foreach (ActionType act in CurrentlyTeaching) {
			Debug.Log(act.ToString());
		}
	}
}


public enum ActionType {
	FireTrade,
	WaterTrade,
	LightningTrade,
	NatureTrade,
	StoneUsage,
	ShadowUsage,
}