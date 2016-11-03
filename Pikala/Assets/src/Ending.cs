using UnityEngine;
using System.Collections;

public class Ending : MainClass {

    public EndingItems endingItems;
    public Transform container;
    private int unlockedItems;

	void Start () {
        unlockedItems = Data.Instance.GetComponent<Items>().unlockedItems;
        EndingItems newItems = Instantiate(endingItems);
        newItems.transform.SetParent(container);
        newItems.transform.localPosition = Vector3.zero;

        Events.OnMusic("carefree");
        Events.UnlockNextRoute();
        if (Data.Instance.GetComponent<Items>().unlockedItems < Data.Instance.GetComponent<Items>().allItems.Count)
        {
            Events.OnSoundFX("arco iris regalo");
            Invoke("DiceRegalo", 10);
        }
        else
        {
            Invoke("Continue", 3);
        }
	}
    public void DiceRegalo()
    {
       
        print("Ending: da premio: " + Data.Instance.GetComponent<Items>().allItems[Data.Instance.GetComponent<Items>().unlockedItems-1].audio);
        Events.OnSoundFX("premios/" + Data.Instance.GetComponent<Items>().allItems[Data.Instance.GetComponent<Items>().unlockedItems-1].audio);
        Invoke("Continue", 3);
    }
    public void Continue()
    {
        Data.Instance.LoadLevel("Tent", false);
    }
}
