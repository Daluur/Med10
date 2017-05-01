using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Overworld {
	public class ShowGeneralInformationBox : ContextInteraction, IInteractable {

		public ShowTypeGeneralInformation typeOfGeneralInformationTrigger;
		public TypeOfGeneralInformation typeOfGeneralInformation;
		public string[] displayText = { "Empty"};
		public string[] buttonOne = { "BACKTEMPLATE" };
		public string buttonTwo = "Next template";
		private ContextInteraction interaction;
		private UnityAction call;
		private bool hasShown = false;

		private void Start() {
			Register(this, KeyCode.Mouse0);

			var tmpCI = GetComponents<ContextInteraction>();
			foreach (var contextInteraction in tmpCI) {
				if (!contextInteraction.Equals(this)) {
					contextInteraction.hasGeneralConfirmationBox = true;
					call = () => contextInteraction.PerformClosenessAction();
				}
			}
			var tmpTrigger = GetComponents<EncounterArea>();
			foreach (var trigger in tmpTrigger) {
				switch (typeOfGeneralInformationTrigger) {
					case ShowTypeGeneralInformation.OnTriggerEnter:
						trigger.onTriggerEnter = false;
						call = () => trigger.DoOnTriggerEnter();
						break;
					case ShowTypeGeneralInformation.OnTriggerExit:
						trigger.onTriggerExit = false;
						call = () => trigger.DoOnTriggerExit();
						break;
				}
			}
			//TODO: Is this needed?
			if(buttonOne.Length != displayText.Length) {
				Debug.LogError("button length and display lenght not the same! "+gameObject.name);
			}
		}

		public void DoAction() {
			meClicked = false;
		}

		public void DoAction<T>(T param, Vector3 m = default(Vector3)) {
			if (hasShown)
				return;
			switch (typeOfGeneralInformationTrigger) {
				case ShowTypeGeneralInformation.IfClickedAndCloseEnough:
					meClicked = true;
					CheckDistance();
					break;
				case ShowTypeGeneralInformation.OnTriggerEnter:
					break;
				case ShowTypeGeneralInformation.OnTriggerExit:
					break;
				default:
					Debug.Log(typeOfGeneralInformationTrigger + " has not been implemented yet");
					break;
			}
		}

		public ControlUIElement GetControlElement() {
			return null;
		}

		public override void PerformClosenessAction() {
			GetComponents<ContextInteraction>().First(element => !element.Equals(this)).hasGeneralConfirmationBox = false;
			ShowPopUp();
		}

		private void OnTriggerEnter(Collider other) {
			if(other.tag != TagConstants.OVERWORLDPLAYER && typeOfGeneralInformationTrigger != ShowTypeGeneralInformation.OnTriggerEnter)
				return;
			ShowPopUp();
		}


		private void OnTriggerExit(Collider other) {
			if(other.tag!= TagConstants.OVERWORLDPLAYER && typeOfGeneralInformationTrigger != ShowTypeGeneralInformation.OnTriggerExit)
				return;
			ShowPopUp();
		}

		public void ShowPopUp() {
			if (hasShown)
				return;
			if(typeOfGeneralInformation == TypeOfGeneralInformation.OneButton){
				if (displayText.Length == 2) {
					GeneralConfirmationBox.instance.ShowPopUp(displayText[0], buttonOne[0], 
						() => GeneralConfirmationBox.instance.ShowPopUp(displayText[1], buttonOne[1]));
				}
				else if (displayText.Length == 3) {
					GeneralConfirmationBox.instance.ShowPopUp(displayText[0], buttonOne[0], 
						() => GeneralConfirmationBox.instance.ShowPopUp(displayText[1], buttonOne[1],
						() => GeneralConfirmationBox.instance.ShowPopUp(displayText[2], buttonOne[2])));
				}
				else if (displayText.Length == 4) {
					GeneralConfirmationBox.instance.ShowPopUp(displayText[0], buttonOne[0],
						() => GeneralConfirmationBox.instance.ShowPopUp(displayText[1], buttonOne[1],
						() => GeneralConfirmationBox.instance.ShowPopUp(displayText[2], buttonOne[2],
						() => GeneralConfirmationBox.instance.ShowPopUp(displayText[3], buttonOne[3]))));
				}
				else {
					GeneralConfirmationBox.instance.ShowPopUp(displayText[0], buttonOne[0], call);
				}
			}
			else {
				GeneralConfirmationBox.instance.ShowPopUp(displayText[0], buttonOne[0], call, buttonTwo);
			}
			hasShown = true;
		}

		public enum ShowTypeGeneralInformation {
			OnTriggerEnter,
			OnTriggerExit,
			IfClickedAndCloseEnough
		}

		public enum TypeOfGeneralInformation {
			OneButton,
			TwoButtons
		}
	}
}
