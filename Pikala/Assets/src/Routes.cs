using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Routes : MonoBehaviour {

    public int unlockedRoute;
    public int actualGameData;
    public int gameID;
    public int routeID;

    public List<GameData> route1;
    public List<GameData> route2;
    public List<GameData> route3;

    void Start()
    {
        Events.UnlockNextRoute += UnlockNextRoute;
        unlockedRoute = PlayerPrefs.GetInt("unlockedRoute", 1);
        int id = 0;
        foreach (GameData gameData in route1)
        {
            if (PlayerPrefs.GetInt("level_1_" + id) == 1)
                gameData.perfect = true;
            id++;
        }
        id = 0;
        foreach (GameData gameData in route2)
        {
            if (PlayerPrefs.GetInt("level_2_" + id) == 1)
                gameData.perfect = true;
            id++;
        }
        id = 0;
        foreach (GameData gameData in route3)
        {
            if (PlayerPrefs.GetInt("level_3_" + id) == 1)
                gameData.perfect = true;
            id++;
        }
        Events.ResetApp += ResetApp;
    }
    void OnDestroy()
    {
        Events.ResetApp -= ResetApp;
        Events.UnlockNextRoute -= UnlockNextRoute;
    }
    void UnlockNextRoute()
    {
        unlockedRoute++;
        if(unlockedRoute<4)
            PlayerPrefs.SetInt("unlockedRoute", unlockedRoute);
    }
    void ResetApp()
    {
        unlockedRoute = 1;
        actualGameData = 0;
        ResetMap();

        foreach (GameData gameData in route1)
            gameData.perfect = false;
        foreach (GameData gameData in route2)
            gameData.perfect = false;
        foreach (GameData gameData in route3)
            gameData.perfect = false;
    }
    public void SetPerfect(bool isPerfect)
    {
        print("SetPerfect " + isPerfect);
        if (isPerfect)
        {
            print("New Perfect:   " + "level_" + routeID + "_" + gameID);
            PlayerPrefs.SetInt("level_" + routeID + "_" + gameID, 1);
            GetActualGame().perfect = isPerfect;
        }
    }
    public void RouteSelected(int routeID)
    {
        gameID = 0;
        this.routeID = routeID;
    }
    public void NextLevel()
    {
        gameID++;
    }
    public GameData GetActualGame()
    {
        switch (routeID)
        {
            case 1: return route1[gameID];
            case 2: return route2[gameID];
            default: return route3[gameID];
        }
    }
    public int GetTotalWordsOfActiveGame()
    {
        return GetActualGame().wordsQty;
    }
    public void ResetMap()
    {
        routeID = 0;
        gameID = 1;
    }
    public float GetTotalPerfectAmount()
    {
        int totalData = 0;
        int totalPerfect = 0;
        foreach (GameData data in route1 )
        {
            totalData++;
            if (data.perfect)
                totalPerfect++;
        }
        foreach (GameData data in route2)
        {
            totalData++;
            if (data.perfect)
                totalPerfect++;
        }
        foreach (GameData data in route3)
        {
            totalData++;
            if (data.perfect)
                totalPerfect++;
        }
       // print("totalPerfect: " + totalPerfect + "totalData: " + totalData);
        return (float)totalPerfect / (float)totalData;
    }
}
