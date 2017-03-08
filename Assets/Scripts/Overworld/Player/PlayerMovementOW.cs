using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Overworld {
	public class PlayerMovementOW : InputSubscriber, IInteractable {

		private NavMeshAgent agent;



		void Start() {

			Register(this, KeyCode.Mouse0);

			agent = GetComponent<NavMeshAgent>();
			if (agent == null) {
				Debug.LogError("The Player character needs a nav mesh agent to move around!!!!");
			}
			if (!agent.SetDestination(gameObject.transform.position)) {
				Debug.LogError("There needs to be a nav mesh baked before the player can move around");
			}
			if (gameObject.tag != TagConstants.OVERWORLDPLAYER) {
				Debug.LogError("The player is missing the tag: " + TagConstants.OVERWORLDPLAYER);
			}
		}

		void PlayerMoveToMouseInput(Vector3 hitPoint) {
			agent.SetDestination(hitPoint);
		}

		public void DoAction() {
		}

		public void DoAction<T>(T param) {
			if (param.GetType() != typeof(Vector3)) {
				Debug.LogError("To move the character give it a Vector3 to move to");
				return;
			}
			PlayerMoveToMouseInput((Vector3)(param as Vector3?));
		}
	}
}