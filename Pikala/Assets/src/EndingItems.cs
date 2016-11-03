using UnityEngine;
using System.Collections;

public class EndingItems : MonoBehaviour {

    public GameObject[] items;

	void Start () {
       
        int unlockedItems = Data.Instance.GetComponent<Items>().unlockedItems;
        int newItemID = unlockedItems +1;
        int id = 0;
        foreach (GameObject item in items)
        {
            id++;
            if (id == newItemID)
                item.SetActive(true);
            else
                item.SetActive(false);
        }
        Events.WonItem();

        print("unlockedItems: " + unlockedItems);
	}
}
