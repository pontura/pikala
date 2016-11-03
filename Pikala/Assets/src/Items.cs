using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Items : MonoBehaviour {

    public int unlockedItems;
    public List<Inventory> allItems;

    private int DefaultUnlockedItems = 3;

    [Serializable]
    public class Inventory
    {
        public AvatarCustomizator.partsType partType;
        public int id;
        public string audio;
    }

    public Data[] body;
    public Data[] hats;
    public Data[] glasses;

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
        unlockedItems = PlayerPrefs.GetInt("unlockedItems", DefaultUnlockedItems);
        SetLocks();
    }
    void OnDestroy()
    {
        Events.WonItem -= WonItem;
        Events.ResetApp -= ResetApp;
    }
    void ResetApp()
    {
        unlockedItems = DefaultUnlockedItems;
        unlockedItems = PlayerPrefs.GetInt("unlockedItems", unlockedItems);
        SetLocks();
    }
    void WonItem()
    {
        unlockedItems++;
        PlayerPrefs.SetInt("unlockedItems", unlockedItems);
        SetLocks();
    }
    void SetLocks()
    {
        int id = 0;
        foreach (Inventory data in allItems)
        {
            if (id < unlockedItems)
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
            default:
                 arr = glasses; 
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
