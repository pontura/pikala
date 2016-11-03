using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProgressData : MonoBehaviour {

    public List<string> well1;
    public List<string> well2;
    public List<string> well3;

    public List<string> lastWrongWords;

    void Start () {        
        Events.OnAddWordToList += OnAddWordToList;
        //GetSavedWords();
    }
    void GetSavedWords()
    {
        for (int a = 0; a < 3; a++)
        {
            for (int b = 0; b < 40; b++)
            {
                string word = PlayerPrefs.GetString("well_" + a + 1 + "_" + b + 1, "");
                if (word != "")
                    AddWordTo(a + 1, word);
            }
        }
    }
    void AddWordTo(int arrId, string word)
    {
        switch(arrId)
        {
            case 1: well1.Add(word); break;
            case 2: well2.Add(word); break;
            case 3: well3.Add(word); break;
        }
    }
    void OnAddWordToList(GameData.types type, int id)
    {

    }

}
