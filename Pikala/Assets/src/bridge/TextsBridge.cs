using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class TextsBridge : Texts {

    public List<Vuelta> vueltas;

    [Serializable]
    public class Vuelta
    {
        public string ok;
        public string palabra;
    }
	void Start () {
        Load("bridge");
	}
    public Vuelta GetVueltaByLevel()
    {
        if (vueltas.Count == 0) return null;

        if (Data.Instance.levelsManager.bridges > vueltas.Count - 1)
            Data.Instance.levelsManager.bridges = 0;

        return vueltas[Data.Instance.levelsManager.bridges];
    }
    public override void LoadDataMinigames(string json_data)
    {        
        JSONNode Json = SimpleJSON.JSON.Parse(json_data);

        string texts = "texts";

        for (int a = 0; a < Json[texts].Count; a++)
        {
            Vuelta vuelta = new Vuelta();
            vuelta.ok = Json[texts][a][1];
            vuelta.palabra = Json[texts][a][0];
            vueltas.Add(vuelta);
        }
    }
    public string GetPalabraReal(string ok)
    {
        foreach (Vuelta vuelta in vueltas)
            if (vuelta.palabra.ToUpper() == ok.ToUpper()) return vuelta.palabra;

        return "";
    }
    public string GetLetterReal(string letter)
    {
        switch(letter.ToUpper())
        {
            case "K": return "C";
            case "2": return "CH";
            case "3": return "LL";
        }
        return letter;
    }
}