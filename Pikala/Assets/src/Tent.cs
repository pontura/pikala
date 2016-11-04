using UnityEngine;
using System.Collections;

public class Tent : MainClass
{
    public SpecialItemInTent special1;
    public SpecialItemInTent special2;
    public SpecialItemInTent special3;

    void Start () {

        Events.OnMusic("carefree");

        if (Data.Instance.GetComponent<Items>().unlockedItems_1 > 2)
            special1.Init(true);
        else special1.Init(false);

        if (Data.Instance.GetComponent<Items>().unlockedItems_2 > 2)
            special2.Init(true);
        else special2.Init(false);

        if (Data.Instance.GetComponent<Items>().unlockedItems_3 > 2)
            special3.Init(true);
        else special3.Init(false);

    }
    public void BackPressed()
    {
        Data.Instance.LoadLevel("MainMenu", false);
    }
}
