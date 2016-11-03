using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapMainMenu : MainClass
{
    public GameObject button2;
    public GameObject button3;

    public Map map;

    void Start()
    {

        Events.RutaSelected += RutaSelected;
        map.Init();

        if(Data.Instance.routes.unlockedRoute<2)
        {
            Lock(button2);
        }
        if(Data.Instance.routes.unlockedRoute<3)
        {
            Lock(button3);
        }
    }
    void Lock(GameObject button)
    {
        button.GetComponent<Button>().interactable = false;
        button.GetComponent<Animator>().enabled = false;
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
}
