using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour {

	void Start() {
		DataToServer.SendData(this);
	}

	public void SurveyButton() {
		Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSf7NZQaoaHkTCUnwqwbtmAq5-b6U2SYkJzpnAjHp4ka0XqSRw/viewform?usp=pp_url&entry.1082687180&entry.595656830&entry.1882535212&entry.1496652647&entry.2012270758&entry.2105896842&entry.1685425408&entry.1021923789&entry.202100838&entry.16108917&entry.13362197&entry.49328072&entry.27673333&entry.1202604059&entry.599772398&entry.578193&entry.704483080="+DataGathering.Instance.ID);
	}

	public void Quit() {
		Application.Quit();
	}
}
