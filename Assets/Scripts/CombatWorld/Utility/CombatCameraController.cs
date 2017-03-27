using UnityEngine;

namespace CombatWorld {
	public class CombatCameraController : MonoBehaviour {

		float speed = 60.0f;
		Vector3 newPos;
		Vector2 minMaxXPos = new Vector2(-40, 40);

		void LateUpdate() {
			if (Input.GetMouseButton(0)) {
				if (Input.GetAxis("Mouse X") != 0) {
					newPos = transform.position - new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed, 0, 0);
					newPos.x = Mathf.Clamp(newPos.x, minMaxXPos.x, minMaxXPos.y);
					transform.position = newPos;
				}
			}
		}
	}
}