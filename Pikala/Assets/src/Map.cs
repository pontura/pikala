using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public bool isMainMenu;
    public int mapID;
    public MainMenuRoute route1;
    public MainMenuRoute route2;
    public MainMenuRoute route3;

    // public GameObject container;

    public AnimationClip clipLeft;
    public AnimationClip clipRight;


    public GameObject[] items;

    public Button[] buttons;

    void Start()
    {
        if (isMainMenu)
        {
            SetPremios();
        }
        else
        {
            foreach (Button go in buttons)
                go.GetComponent<Animator>().enabled = false;
        }
    }
    void OnEnable()
    {
        if (!isMainMenu)
        {

            int routeID = Data.Instance.routes.routeID;
            int unlockedRoute = Data.Instance.routes.unlockedRoute;
            int id = 0;
            foreach (GameObject go in items)
            {
                id++;
                if ((id == routeID || id + 3 == routeID || id + 6 == routeID) && ((int)(unlockedRoute+1) < routeID))
                    go.SetActive(true);
                else
                    go.SetActive(false);
            }
        }
        else
        {
            SetPremios();
        }
    }
    void Lock(Button button)
    {
        button.interactable = false;
        button.enabled = false;
        button.GetComponent<Animator>().Stop();
    }
    public int unlockedItems;
    void SetPremios()
    {
        unlockedItems = Data.Instance.GetComponent<Items>().unlockedItems;

        int id = mapID*3;
        foreach (GameObject go in items)
        {
            if (id < unlockedItems)
                go.SetActive(false);
            else
                go.SetActive(true);
            id++;
        }

        id = mapID * 3;

        foreach (Button button in buttons)
        {
            if (id > unlockedItems)
            {
                Lock(button);
            }
            id++;
        }
    }
    public void AnimateTo(bool left)
    {
        gameObject.layer = 14;
        if (left)
            GetComponent<Animation>().clip = clipLeft;
        else
            GetComponent<Animation>().clip = clipRight;

        GetComponent<Animation>().Play();
    }
    public void Init()
    {
        int routeID = Data.Instance.routes.routeID;
        int gameID = Data.Instance.routes.gameID;

       if(gameID <= 1)
        {
            route1.ResetPlayedPoints();
            route2.ResetPlayedPoints();
            route3.ResetPlayedPoints();
        }

        List<GameData> route_a = Data.Instance.routes.route1;
        List<GameData> route_b = Data.Instance.routes.route2;
        List<GameData> route_c = Data.Instance.routes.route3;

        switch (mapID)
        {
            case 1:
                route_a = Data.Instance.routes.route4;
                route_b = Data.Instance.routes.route5;
                route_c = Data.Instance.routes.route6;
                break;

            case 2:
                route_a = Data.Instance.routes.route7;
                route_b = Data.Instance.routes.route8;
                route_c = Data.Instance.routes.route9;
                break;

        }

        for (int id = 0; id < route_a.Count; id++)
            route1.Init(id, route_a[id].perfect);

        for (int id = 0; id < route_b.Count; id++)
            route2.Init(id, route_b[id].perfect);

        for (int id = 0; id < route_c.Count; id++)
            route3.Init(id, route_c[id].perfect);
        
        if (!isMainMenu)
        {
            route1.SetOff();
            route2.SetOff();
            route3.SetOff();

            if (routeID == 1 || routeID == 4 || routeID == 7)
                route1.SetOn(gameID);
            else if (routeID == 2 || routeID == 5 || routeID == 8)
                route2.SetOn(gameID);
            else
                route3.SetOn(gameID);
        }
        
    }
   
    public void SetRutaSelected(int id)
    {
        Events.RutaSelected(id);
    }

}
