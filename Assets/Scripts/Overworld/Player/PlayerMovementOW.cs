using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Overworld {
	public class PlayerMovementOW : InputSubscriber, IInteractable {

		private NavMeshAgent agent;
		private Animator animator;
		public GameObject clickMoveToObject;

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
			animator = GetComponentInChildren<Animator>();
		}

		void PlayerMoveToMouseInput(Vector3 hitPoint) {
			if (agent.SetDestination(hitPoint)) {
				Instantiate(clickMoveToObject, hitPoint, Quaternion.identity);
			}
		}

		private void Update() {
			animator.SetFloat("Speed", agent.velocity.magnitude);
		}

		/*private IEnumerator MovementAnimation() {
			isRunning = true;
			animator.SetTrigger("Run");
			while (agent.hasPath || agent.pathPending) {
				yield return new WaitForEndOfFrame();
			}
			animator.SetTrigger("Run");
			isRunning = false;
		}*/

		public void DoAction() {
			agent.Stop();
			agent.ResetPath();
		}

		public void DoAction<T>(T param) {
			agent.Resume();
			if (param.GetType() != typeof(Vector3)) {
				Debug.LogError("To move the character give it a Vector3 to move to");
				return;
			}
			PlayerMoveToMouseInput((Vector3)(param as Vector3?));
		}
	}
}