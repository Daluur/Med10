namespace Overworld {
	public interface IInteractable {
		void DoAction();
		void DoAction<T>(T param);
		ControlUIElement GetControlElement();
	}
}
