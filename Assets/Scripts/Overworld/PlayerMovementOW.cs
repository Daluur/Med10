using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Overworld {
	public class PlayerMovementOW : MonoBehaviour {

		private NavMeshAgent agent;
		private LayerMask layerMask;

		void Start() {
			layerMask = ( 1 << LayerMask.NameToLayer(LayerConstants.GROUNDLAYER) );
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

		void Update() {
			if (Input.GetMouseButton(0)) {
				PlayerMoveToMouseInput(Input.mousePosition);
			}
		}

		void PlayerMoveToMouseInput(Vector3 mousePos) {
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 500f, layerMask)) {
				agent.SetDestination(hit.point);
			}
		}
	}
}