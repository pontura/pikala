using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Items : MonoBehaviour {
    
    public int unlockedItems_1;
    public int unlockedItems_2;
    public int unlockedItems_3;

    public List<Inventory> allItems;

    public List<Inventory> items_1;
    public List<Inventory> items_2;
    public List<Inventory> items_3;


    private int DefaultUnlockedItems = 3;

    [Serializable]
    public class Inventory
    {
        public AvatarCustomizator.partsType partType;
        public int id;
        public string audio;
        public bool isHe;
    }

    public Data[] body;
    public Data[] hats;
    public Data[] glasses;
    public Data[] specials;

    [Serializable]
    public class Data
    {
        public int id;
        public bool hasToSides;
        public bool locked;
             
    }
    void Start()
    {
        Events.WonItem += WonItem;
        Events.ResetApp += ResetApp;
        unlockedItems_1 = PlayerPrefs.GetInt("unlockedItems_1", 0);
        unlockedItems_2 = PlayerPrefs.GetInt("unlockedItems_2", 0);
        unlockedItems_3 = PlayerPrefs.GetInt("unlockedItems_3", 0);
        SetLocks();
    }
    void OnDestroy()
    {
        Events.WonItem -= WonItem;
        Events.ResetApp -= ResetApp;
    }
    void ResetApp()
    {
        PlayerPrefs.SetInt("unlockedItems_1", 0);
        PlayerPrefs.SetInt("unlockedItems_2", 0);
        PlayerPrefs.SetInt("unlockedItems_3", 0);
        unlockedItems_1 = 0;
        unlockedItems_2 = 0;
        unlockedItems_3 = 0;
        SetLocks();
    }
    void WonItem(int routeID)
    {
        switch(routeID)
        {
            case 1: unlockedItems_1++; PlayerPrefs.SetInt("unlockedItems_1", unlockedItems_1); break;
            case 2: unlockedItems_2++; PlayerPrefs.SetInt("unlockedItems_2", unlockedItems_2);  break;
            case 3: unlockedItems_3++; PlayerPrefs.SetInt("unlockedItems_3", unlockedItems_3);  break;
        }
        SetLocks();
    }
    void SetLocks()
    {
        int id = 0;
        foreach (Inventory data in items_1)
        {
            if (id < unlockedItems_1)
                GetItemData(data.partType, data.id).locked = false;
            else
                GetItemData(data.partType, data.id).locked = true;
            id++;
        }
        id = 0;
        foreach (Inventory data in items_2)
        {
            if (id < unlockedItems_2)
                GetItemData(data.partType, data.id).locked = false;
            else
                GetItemData(data.partType, data.id).locked = true;
            id++;
        }
        id = 0;
        foreach (Inventory data in items_3)
        {
            if (id < unlockedItems_3)
                GetItemData(data.partType, data.id).locked = false;
            else
                GetItemData(data.partType, data.id).locked = true;
            id++;
        }
    }
    public Data GetItemData(AvatarCustomizator.partsType partType, int id)
    {
        Data[] arr;
        switch (partType)
        {
            case AvatarCustomizator.partsType.BODY: 
                arr = body; 
                break;
            case AvatarCustomizator.partsType.HATS: 
                arr = hats; 
                break;
            case AvatarCustomizator.partsType.GLASSES:
                arr = glasses; 
                 break;
            default:
                arr = specials;
                break;
        }
        foreach (Data data in arr)
        {
            if (id == data.id)
                return data;
        }
        return null;
    }
}
