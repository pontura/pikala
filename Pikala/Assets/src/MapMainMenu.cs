using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapMainMenu : MainClass
{

    public Map map1;
    public Map map2;
    public Map map3;

    public GameObject next;
    public GameObject prev;

    void Start()
    {
        Events.OnSoundFX("listos nuevo recorrido");

        Events.RutaSelected += RutaSelected;
        SetActiveMap();

        SetButtonsState();
    }
    void SetButtonsState()
    {
        switch (Data.Instance.GetComponent<MapInData>().mapID)
        {
            case 0:
                next.SetActive(true);
                prev.SetActive(false);
                break;
            case 1:
                next.SetActive(true);
                prev.SetActive(true);
                break;
            case 2:
                next.SetActive(false);
                prev.SetActive(true);
                break;
        }
    }

    public void BackPressed()
    {
        Data.Instance.LoadLevel("MainMenu", false);
    }
    void OnDestroy()
    {
        Events.RutaSelected -= RutaSelected;
    }
    public void RutaSelected(int routeID)
    {
        //Events.OnShowMap(false);
        Data.Instance.routes.RouteSelected(routeID);
        Data.Instance.levelsManager.LoadNextGame();
    }
    public void Next()
    {
        Data.Instance.GetComponent<MapInData>().mapID++;
        if (Data.Instance.GetComponent<MapInData>().mapID > 2)
            Data.Instance.GetComponent<MapInData>().mapID = 0;
        SetActiveMap();
    }
    public void Prev()
    {
        Data.Instance.GetComponent<MapInData>().mapID--;
        if (Data.Instance.GetComponent<MapInData>().mapID < 0)
            Data.Instance.GetComponent<MapInData>().mapID = 2;
        SetActiveMap();
    }
    void SetActiveMap()
    {
        SetButtonsState();

        map1.gameObject.SetActive(false);
        map2.gameObject.SetActive(false);
        map3.gameObject.SetActive(false);

        switch (Data.Instance.GetComponent<MapInData>().mapID)
        {
            case 0: map1.gameObject.SetActive(true); map1.Init(); break;
            case 1: map2.gameObject.SetActive(true); map2.Init(); break;
            case 2: map3.gameObject.SetActive(true); map3.Init(); break;
        }
    }
}
