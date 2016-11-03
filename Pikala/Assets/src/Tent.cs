using UnityEngine;
using System.Collections;

public class Tent : MainClass
{
	void Start () {
        Events.OnMusic("carefree");
	}
    public void BackPressed()
    {
        Data.Instance.LoadLevel("MainMenu", false);
    }
}
