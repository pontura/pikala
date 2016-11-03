using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class TextsMonkeys : Texts {

    public List<Vuelta> vueltas;

    [Serializable]
    public class Vuelta
    {
        public string audio;
        public string title;
        public string ok;
        public List<string> wrong;
    }
	void Start () {
        Load("monkeys");
	}
    public Vuelta GetVueltaByLevel()
    {
        if (vueltas.Count == 0) return null;

        if (Data.Instance.levelsManager.monkeys > vueltas.Count - 1)
            Data.Instance.levelsManager.monkeys = 0;

        return vueltas[Data.Instance.levelsManager.monkeys];
    }
    public override void LoadDataMinigames(string json_data)
    {        
        JSONNode Json = SimpleJSON.JSON.Parse(json_data);

        string texts = "texts";

        for (int a = 0; a < Json[texts].Count; a++)
        {
            Vuelta vuelta = new Vuelta();
            vuelta.audio = Json[texts][a]["audio"];
            vuelta.title = Json[texts][a]["title"];
            vuelta.ok = Json[texts][a]["ok"];
            vuelta.wrong = new List<string>();
            for (int b = 0; b < Json[texts][a]["wrong"].Count; b++ )
            {
                string word = Json[texts][a]["wrong"][b];
                vuelta.wrong.Add(word);
            }
            for (int b = 0; b < Json[texts][a]["wrong"].Count; b++)
            {
                string word = Json[texts][a]["wrong"][b];
                vuelta.wrong.Add(word);
            }
            Utils.ShuffleListTexts(vuelta.wrong);
            vueltas.Add(vuelta);
        }
        
    }
}
