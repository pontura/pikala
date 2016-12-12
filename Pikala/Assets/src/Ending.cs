using UnityEngine;
using System.Collections;

public class Ending : MainClass {

    public EndingItems endingItems;
    public Transform container;
    private int unlockedItems;
    private string audioName = "";
    public bool isHe;
    public GameObject readySignal;
    bool winDiploma;
    public AvatarsManager avatarsManager;


	void Start () {
        avatarsManager = GetComponent<AvatarsManager>();
        readySignal.SetActive(false);
        int routeID = Data.Instance.routes.routeID;
        int count;

        switch(routeID)
        {
            case 1:
                unlockedItems =  Data.Instance.GetComponent<Items>().unlockedItems_1;
                if(unlockedItems>=3)
                {
                    Ready();
                    return;
                }
                count = Data.Instance.GetComponent<Items>().items_1.Count;
                audioName = Data.Instance.GetComponent<Items>().items_1[unlockedItems].audio;
                isHe = Data.Instance.GetComponent<Items>().items_1[unlockedItems].isHe;
                break;
            case 2:
                unlockedItems =  Data.Instance.GetComponent<Items>().unlockedItems_2;
                if (unlockedItems >= 3)
                {
                    Ready();
                    return;
                }
                count = Data.Instance.GetComponent<Items>().items_2.Count;
                audioName = Data.Instance.GetComponent<Items>().items_2[unlockedItems].audio;
                isHe = Data.Instance.GetComponent<Items>().items_2[unlockedItems].isHe;
                break;
            default:
                unlockedItems = Data.Instance.GetComponent<Items>().unlockedItems_3;
                if (unlockedItems >= 3)
                {
                    Ready();
                    return;
                }
                count = Data.Instance.GetComponent<Items>().items_3.Count;
                audioName = Data.Instance.GetComponent<Items>().items_3[unlockedItems].audio;
                isHe = Data.Instance.GetComponent<Items>().items_3[unlockedItems].isHe;
                break;
        }
        EndingItems newItems = Instantiate(endingItems);
        newItems.transform.SetParent(container);
        newItems.transform.localPosition = Vector3.zero;

        Events.OnMusic("carefree");
        Events.UnlockNextRoute();
        if (unlockedItems < count)
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
        if(isHe)
            avatarsManager.BoyTalks();
        else
            avatarsManager.GirlTalk();

        print("Ending: da premio: " + audioName);
        Events.OnSoundFX("premios/" + audioName);
        Invoke("Tent", 3);
    }
    public void Tent()
    {
        Data.Instance.LoadLevel("Tent", false);
    }
    public void Continue()
    {
        if(winDiploma)
            Data.Instance.LoadLevel("Tent", false);
        else
            Data.Instance.LoadLevel("Map", false);
    }
    void Ready()
    {
        winDiploma = true;
        readySignal.SetActive(true);
        Events.WinDiploma();
        GetComponent<Animation>().Stop();
        GetComponent<Animation>().enabled = false;
        GetComponent<AvatarsManager>().Idle();
    }
}
