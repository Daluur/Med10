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

		protected override void Awake() {
			base.Awake();
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

		public void TemporaryStop() {
			agent.isStopped = true;
		}

		public void ResumeFromTemporaryStop() {
			//agent.Resume();
			agent.isStopped = false;
		}

		public void Stop() {
			agent.isStopped = true;
			agent.ResetPath();
		}

		/// <summary>
		/// Stops the player, specific usage of DoAction for the player
		/// </summary>
		public void DoAction() {
			//Debug.Log("ASDASD");
			//agent.Stop();
			//agent.ResetPath();
		}

		public void DoAction<T>(T param) {
			agent.isStopped = false;
			if (param.GetType() != typeof(Vector3)) {
				Debug.LogError("To move the character give it a Vector3 to move to");
				return;
			}
			PlayerMoveToMouseInput((Vector3)(param as Vector3?));
		}

		public ControlUIElement GetControlElement() {
			return null;
		}

		public void TeleportPlayer(Vector3 pos) {
			agent.Warp(pos);
		}
	}
}