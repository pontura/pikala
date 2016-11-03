using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public bool isMainMenu;
    public MainMenuRoute route1;
    public MainMenuRoute route2;
    public MainMenuRoute route3;
    public Canvas canvas;
    public GameObject container;

    void Start()
    {
        if (!isMainMenu)
        {
            canvas.enabled = false;
            container.SetActive(false);
            Events.OnShowMap += OnShowMap;
        }
        else
        {
            Events.OnSoundFX("listos nuevo recorrido");
        }
    }
    public void Init()
    {
        int routeID = Data.Instance.routes.routeID;
        int gameID = Data.Instance.routes.gameID;

        for (int id = 0; id < Data.Instance.routes.route1.Count; id++)
            route1.Init(id, Data.Instance.routes.route1[id].perfect);

        for (int id = 0; id < Data.Instance.routes.route2.Count; id++)
            route2.Init(id, Data.Instance.routes.route2[id].perfect);

        for (int id = 0; id < Data.Instance.routes.route3.Count; id++)
            route3.Init(id, Data.Instance.routes.route3[id].perfect);

        
        if (routeID != 0)
        {
            route1.SetOff();
            route2.SetOff();
            route3.SetOff();

            if (routeID == 1)
                route1.SetOn(gameID);
            else if (routeID == 2)
                route2.SetOn(gameID);
            else
                route3.SetOn(gameID);
        }
    }
    void OnDestroy()
    {
        Events.OnShowMap -= OnShowMap;
    }
    public void OnShowMap(bool showIt)
    {
        if (showIt)
        {
            canvas.enabled = true;
            container.SetActive(true);
            container.GetComponent<Animation>().Play("map_on");
            Init();
        }
        else
        {
            if(container.activeSelf)
                container.GetComponent<Animation>().Play("map_off");
        }
    }
    public void SetRutaSelected(int id)
    {
        Events.RutaSelected(id);
    }

}
