using UnityEngine;

namespace Overworld {
	public interface IInteractable {
		void DoAction();
		void DoAction<T>(T param, Vector3 hitNormal = default(Vector3));
		ControlUIElement GetControlElement();
	}
}
