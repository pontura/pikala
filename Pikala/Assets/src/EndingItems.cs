using UnityEngine;
using System.Collections;

public class EndingItems : MonoBehaviour {

    public GameObject[] items_1;
    public GameObject[] items_2;
    public GameObject[] items_3;

    private int routeID;
    void Start () {
        routeID = Data.Instance.routes.routeID;
        int unlockedItems = 0;
        GameObject[] items;
        switch (routeID)
        {
            case 1:
                unlockedItems = Data.Instance.GetComponent<Items>().unlockedItems_1;
                items = items_1;
                break;
            case 2:
                unlockedItems = Data.Instance.GetComponent<Items>().unlockedItems_2;
                items = items_2;
                break;
           default:
                unlockedItems = Data.Instance.GetComponent<Items>().unlockedItems_3;
                items = items_3;
                break;
        }
        
        foreach (GameObject item in items_1)
            item.SetActive(false);
        foreach (GameObject item in items_2)
            item.SetActive(false);
        foreach (GameObject item in items_3)
            item.SetActive(false);

        int newItemID = unlockedItems + 1;
        int id = 0;
        foreach (GameObject item in items)
        {
            id++;
            if (id == newItemID)
                item.SetActive(true);
        }

        Invoke("WonItem", 0.2f);
        print("unlockedItems: " + unlockedItems);
	}
    void WonItem()
    {
        Events.WonItem(routeID);
    }
}
