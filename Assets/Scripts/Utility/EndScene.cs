using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class EndScene : MonoBehaviour {

	public Text timeText;
	
	void Start() {
		//DataToServer.SendData(this);
		DateTime start = DataGathering.Instance.StartTime;
		DateTime end = DateTime.Now;
		TimeSpan timeItTook = end.Subtract(start);
		timeText.text = "For the observer! It took: " + timeItTook.Hours +":"+timeItTook.Minutes+":"+timeItTook.Seconds;
		timeItTookClass TITC = new timeItTookClass();
		TITC.hours = timeItTook.Hours;
		TITC.minutes = timeItTook.Minutes;
		TITC.seconds = timeItTook.Seconds;
		var s = JsonMapper.ToJson(TITC);
		File.WriteAllText(Application.dataPath + "/StreamingAssets/"+ start.Day.ToString()+"-"+start.Hour.ToString()+"-"+start.Minute.ToString() +".json", s);
	}
}

class timeItTookClass {
	public int hours;
	public int minutes;
	public int seconds;
}