using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overworld;

public class CheckpointManager : Singleton<CheckpointManager> {

	Dictionary<int, Checkpoint> checkpoints = new Dictionary<int, Checkpoint>();
	int latestCheckpoint = -1;
	PlayerMovementOW player;

	void Start() {
		player = GameObject.FindGameObjectWithTag(TagConstants.OVERWORLDPLAYER).GetComponent<PlayerMovementOW>();
		latestCheckpoint = SaveLoadHandler.Instance.GetCheckpoint();
		if(latestCheckpoint != -1) {
			TeleportPlayerToLatestCheckpoint();
		}
	}

	public void SetLatestCheckpoint(int id) {
		latestCheckpoint = id;
	}

	public void AssignCheckpoint(int id, Checkpoint checkpoint) {
		if (checkpoints.ContainsKey(id)) {
			Debug.Log("There are is already a checkpoint with this ID!" + id + " - " + checkpoint.gameObject.name);
		}
		else{
			checkpoints.Add(id, checkpoint);
		}
	}

	public void TeleportPlayerToLatestCheckpoint() {
		if (!checkpoints.ContainsKey(latestCheckpoint)) {
			Debug.LogError("The given checkpoint ID: " + latestCheckpoint + " Has not been assigned! If ID is \"-1\", it might be because there is no checkpoints!");
			return;
		}
		player.TeleportPlayer(checkpoints[latestCheckpoint].GetTPPos());
		checkpoints[latestCheckpoint].Save();
	}
}
