using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld;
using LitJson;

public class DataToServer {

	public static void SendData(MonoBehaviour initiator) {
		WWWForm form = new WWWForm();
		form.AddField("action", "send");
		form.AddField("ID", DataGathering.Instance.ID);
		form.AddField("Control", DynamicTut.instance == null ? -1 : DynamicTut.instance.isDynamic ? 1 : 0);
		form.AddField("Trades", GetCDData());
		form.AddField("Summons", GetSummonData());
		form.AddField("Decks", GetDeckData());
		form.AddField("Shadow1", Shadow1());
		form.AddField("Shadow2", Shadow2());
		form.AddField("ToldShadow", ToldShadow());
		form.AddField("ToldType", ToldType());
		form.AddField("NLShadow", NLShadow());
		form.AddField("NLType", NLType());
		string url = "http://daluur.dk/MED10.php";
		WWW www = new WWW(url, form);
		initiator.StartCoroutine(WaitForRequest(www));
	}

	static int Shadow1() {
		return DataGathering.Instance.movedShadowThroughOthersSaveThisValue;
	}

	static int Shadow2() {
		return DataGathering.Instance.movedShadowWithoutMovingThroughOtherUnitsSaveThisValue;
	}

	static int ToldShadow() {
		return DataGathering.Instance.shadowToldCount;
	}
	static int ToldType() {
		return DataGathering.Instance.typesToldCount;
	}
	static int NLShadow() {
		return DataGathering.Instance.notLearnedShadowCount;
	}
	static int NLType() {
		return DataGathering.Instance.notLearnedTypesCount;
	}

	static string GetCDData() {
		List<CombatTrades> data = DataGathering.Instance.GetAllTrades();
		//File.WriteAllText(Application.dataPath + "/StreamingAssets/test.json", sb.ToString());
		return JsonMapper.ToJson(data);
	}

	static string GetDeckData() {
		List<DeckDataClass> data = DataGathering.Instance.GetAllDeckData();
		//File.WriteAllText(Application.dataPath + "/StreamingAssets/DeckData.json", JsonMapper.ToJson(data));
		return JsonMapper.ToJson(data);
	}

	static string GetSummonData() {
		List<SummonPlayerData> data = DataGathering.Instance.GetAllSummonedUnits();
		//File.WriteAllText(Application.dataPath + "/StreamingAssets/summonTest.json", JsonMapper.ToJson(data));
		return JsonMapper.ToJson(data);
	}

	static IEnumerator WaitForRequest(WWW www) {
		yield return www;
		// Check for errors.
		if (www.error == null) {
			//Debug.Log("WWW Ok!: " + www.text);
		}
		else {
			//Debug.Log("WWW Error: " + www.error);
		}
	}
}