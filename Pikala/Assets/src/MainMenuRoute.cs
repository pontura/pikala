using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuRoute : MonoBehaviour {

    public MainMenuRoutePoint[] routePoints;
    public GameObject button;

    public void Init(int activeID, bool perfect)
    {
       // print("MAP : Init  " + activeID + "            perfect: " + perfect);
        routePoints[activeID].Init(perfect);
    }
    public void ResetPlayedPoints()
    {
        button.SetActive(true);
        foreach (MainMenuRoutePoint point in routePoints)
            point.SetUnPlayed();
    }
    public void SetOn(int activeID)
    {
        button.SetActive(true);
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Map")
        {
            int id = 0;
            foreach (MainMenuRoutePoint point in routePoints)
            {
                if (id < activeID)
                    point.SetPlayed();
                id++;
            }
        }

        gameObject.SetActive(true);
        foreach (MainMenuRoutePoint point in routePoints)
            point.SetOff();

        if (activeID > 0 && sceneName != "Map")
            routePoints[activeID - 1].SetOn();
    }
    public void SetOff()
    {
        button.SetActive(false);
        gameObject.SetActive(false);
    }
}
