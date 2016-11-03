﻿using UnityEngine;
using System.Collections;
using System;

public class DolphinsLevelManager : MonoBehaviour {

    public DolphinObject DolphinWord;
    public DolphinObstacle DolphinBarrel;
    public DolphinObstacle DolphinFoca;
    public DolphinObstacle DolphinRock1;
    public DolphinObstacle DolphinRock2;

    public int activeGroupId = 0;
    private float startingGroupDistance;
    public Lanes lanes;
    public DolphinLevel StartingLevel;

   // public Group[] groups;

    public DolphinLevel activeLevel;
    private int nextLevelDistance;
    private int offset = 15;
    public DolphinUI ui;

    public void Init()
    {

        activeLevel = StartingLevel;
        nextLevelDistance = StartingLevel.length;
        activeGroupId = -1;
        Invoke("Continue", 0.2f);
        CheckForNewLevel(0);
    }
    public void Continue()
    {
        activeLevel = StartingLevel;
        nextLevelDistance = StartingLevel.length;
        activeGroupId = 0;        
    }
    public void CheckForNewLevel(float distance)
    {
        distance += offset;    

        if (distance < nextLevelDistance) return;


        if ((int)distance > (int)DolphinGame.Instance.areasManager.areaSets[activeGroupId].distance)
        {
            startingGroupDistance += distance;
            if (activeGroupId < DolphinGame.Instance.areasManager.areaSets[activeGroupId].levels.Length-1)
                activeGroupId++;
           // print("_ cambia grupo " + activeGroupId + " startingGroupDistance: " + startingGroupDistance + " distanc: " + distance);
        }

        int rand = UnityEngine.Random.Range(0, DolphinGame.Instance.areasManager.areaSets[activeGroupId].levels.Length);
        activeLevel = DolphinGame.Instance.areasManager.areaSets[activeGroupId].levels[rand];

        print("nextLevelDistance " + nextLevelDistance + " distance " + distance + " activeGroupId: " + activeGroupId + " activeLevel.length " + activeLevel.length + "  activeLevel.NAME " + activeLevel.name);
        LoadLevelAssets(nextLevelDistance);
        
        nextLevelDistance += activeLevel.length;
        
    }
    
    private void LoadLevelAssets(int nextLevelDistance)
    {
        Lanes laneData = activeLevel.GetComponent<Lanes>();
        //  lanes.AddBackground(laneData.vereda, nextLevelDistance, activeLevel.length);


        float lastTimeCorrectWord = 0;
        foreach (Lane lane in laneData.all)
        {
            Transform[] allObjectsInLane = lane.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in allObjectsInLane)
            {
                DolphinObjectSettings settings = new DolphinObjectSettings();
               

                DolphinObject obj = null;
                //switch (t.gameObject.name)
                //{
                //    case "word":
                //        obj = DolphinWord;
                //        break;
                //    case "barrel":
                //        obj = DolphinBarrel;
                //        break;
                //}
                switch (t.gameObject.name)
                {
                    case "word":
                        int rand = UnityEngine.Random.Range(0, 100);                       
                        if (ui.state == DolphinUI.states.PLAYING)
                        {
                            if (rand < 30)
                            {
                                switch (Data.Instance.routes.routeID)
                                {
                                    case 1:
                                        obj = DolphinBarrel; break;
                                    case 2:
                                        obj = DolphinFoca; break;
                                    default:
                                        if (UnityEngine.Random.Range(0, 100) < 50)
                                            obj = DolphinRock1;
                                        else
                                            obj = DolphinRock2;
                                        break;
                                }
                            }
                            else
                            {
                                obj = DolphinWord;
                                if (rand < 60 && Time.time > lastTimeCorrectWord +4)
                                {
                                    settings.word = ui.ok.ToUpper();
                                    settings.isCorrect = true;
                                    lastTimeCorrectWord = Time.time;
                                }
                                else
                                {
                                    settings.word = GetRandomWrongWord().ToUpper();
                                    settings.isCorrect = false;
                                }
                                settings.speed = 0;
                            }
                        }
                        break;
                }

                if (obj != null)
                {
                    DolphinObject newObj = Instantiate(obj);
                    lanes.AddObjectToLane(newObj, lane.id, (int)(nextLevelDistance + t.transform.localPosition.x), settings);
                }
            }
        }

        
    }
	public void LoadNext () {
	    
	}
    string GetRandomWrongWord()
    {
       // TextsMonkeys.Vuelta vuelta = Data.Instance.GetComponent<TextsMonkeys>().vueltas[UnityEngine.Random.Range(0, Data.Instance.GetComponent<TextsMonkeys>().vueltas.Count - 1)];

        string wrongWord = ui.wrongWords[UnityEngine.Random.Range(0, ui.wrongWords.Count)];
        if (wrongWord != ui.ok)
            return wrongWord;
        else
            return GetRandomWrongWord();
    }
}
