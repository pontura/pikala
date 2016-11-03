﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FroggerSignal : MonoBehaviour {

    public int vueltaID;
    public TextMesh field;
    public bool isOk;
    public string word;

    public void Init(string word, bool isOk, int vueltaID) 
    {
        this.vueltaID = vueltaID;
        GetComponent<Animation>().Play("Idle");
        GetComponent<Animation>()["Idle"].normalizedTime = Random.Range(0, 10) / 10;
        this.word = word;
        this.isOk = isOk;
        field.text = word;
	}
    public void Break()
    {
        GetComponent<Animation>().Play("Break");
        field.text = "";
    }
    public void Rebuild()
    {
        if (!GetComponent<Animation>()) return;
        GetComponent<Animation>().Play("Idle");
        field.text = word;
    }
    public void CatchWord()
    {
        field.text = "";
    }
}
