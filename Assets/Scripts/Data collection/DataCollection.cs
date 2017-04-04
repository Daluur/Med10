using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollection : Singleton<DataCollection> {

	protected override void Awake() {
		base.Awake();
		DontDestroyOnLoad(gameObject);
		UnlearnedActions.Add(ActionType.FireTrade);
		UnlearnedActions.Add(ActionType.WaterTrade);
		UnlearnedActions.Add(ActionType.LightningTrade);
		UnlearnedActions.Add(ActionType.NatureTrade);
		UnlearnedActions.Add(ActionType.StoneUsage);
		UnlearnedActions.Add(ActionType.ShadowUsage);
		StartTeachNewRandomUnlearnedAction();
	}

	public int performanceThreshold = 3;

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
			if(performedThisCombat[action] < 3) {
				continue;
			}
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
		DebugInfo();
		performedThisCombat.Clear();
	}

	public List<ActionType> GetLearnedActions() {
		return LearnedActions;
	}

	public List<ActionType> GetThinkLearnedActions() {
		return ThinkLearnedActions;
	}

	public List<ActionType> GetCurrentlyTeaching() {
		return CurrentlyTeaching;
	}

	public void StartTeachNewRandomUnlearnedAction() {
		while (true) { //Needs to test if unlearned actually has a type that has not already been taught, and is not currently being taught.
			ActionType type = UnlearnedActions[Random.Range(0, UnlearnedActions.Count)];
			if (!CurrentlyTeaching.Contains(type)) {
				CurrentlyTeaching.Add(type);
				return;
			}
		}
	}

	void DebugInfo() {
		Debug.LogError("End of combat analysis");
		Debug.LogWarning("did this combat!");
		foreach (ActionType act in performedThisCombat.Keys) {
			Debug.Log(act.ToString());
		}
		Debug.LogWarning("learned actions");
		foreach (ActionType act in LearnedActions) {
			Debug.Log(act.ToString());
		}
		Debug.LogWarning("maybe learned");
		foreach (ActionType act in ThinkLearnedActions) {
			Debug.Log(act.ToString());
		}
		Debug.LogWarning("not learned");
		foreach (ActionType act in UnlearnedActions) {
			Debug.Log(act.ToString());
		}
		Debug.LogWarning("currently teaching");
		foreach (ActionType act in CurrentlyTeaching) {
			Debug.Log(act.ToString());
		}
		Debug.LogWarning("now teaching: " + CurrentlyTeaching[0].ToString());
		Debug.LogError("End of end of combat analysis");
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