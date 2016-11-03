using UnityEngine;
using System.Collections;

public class LevelsManager : MonoBehaviour {

    //comodin para los games que cargan la misma escena:
    public int vueltas = 0;

    public int monkeys = 0;
    public int frogger = 0;
    public int bridges = 0;

    public bool introPlayed;
    public bool frogger_IntroPlayed;
    public bool monkey_IntroPlayed;
    public bool bridge_IntroPlayed;
    public bool dolphin_IntroPlayed;

    void Start()
    {
        monkeys = PlayerPrefs.GetInt("monkeys", 0);
        frogger = PlayerPrefs.GetInt("frogger", 0);
        bridges = PlayerPrefs.GetInt("bridges", 0); 

        Events.OnLevelComplete += OnLevelComplete;
        Events.OnOkWord += OnOkWord;
        Events.ResetApp += ResetApp;

        int unlockedRoute = PlayerPrefs.GetInt("unlockedRoute", 1);
        if(unlockedRoute>1)
        {
            introPlayed = true;
            frogger_IntroPlayed = true;
            monkey_IntroPlayed = true;
            bridge_IntroPlayed = true;
            dolphin_IntroPlayed = true;
        }
    }
    void OnDestroy()
    {
        Events.OnLevelComplete -= OnLevelComplete;
        Events.OnOkWord -= OnOkWord;
        Events.ResetApp -= ResetApp;
    }
    void ResetApp()
    {
        vueltas = 0;
        monkeys = 0;
        frogger = 0;
        bridges = 0;

        introPlayed = false;
        frogger_IntroPlayed = false;
        bridge_IntroPlayed = false;
        dolphin_IntroPlayed = false;
        monkey_IntroPlayed = false;
    }
    void OnOkWord(GameData.types type)
    {
        vueltas++;
        switch (type)
        {
            case GameData.types.MONKEY: 
                monkeys++;
                if (monkeys >= GetComponent<TextsMonkeys>().vueltas.Count)
                    monkeys = 0;
                PlayerPrefs.SetInt("monkeys", monkeys); 
                break;
            case GameData.types.FROGGER:
               // frogger++;
               // if (frogger >= GetComponent<TextsFrogger>().vueltas.Count)
                 //   frogger = 0;
                PlayerPrefs.SetInt("frogger", frogger);
                break;
            case GameData.types.BRIDGE: 
                bridges++;
                if (bridges >= GetComponent<TextsBridge>().vueltas.Count)
                    bridges = 0;
                PlayerPrefs.SetInt("bridges", bridges); 
                break;
            case GameData.types.DOLPHIN: 
                break;
        }
    }
    void OnLevelComplete(GameData.types type, bool commitError)
    {
        if (commitError)
        {
            if(Random.Range(0,10)<5)
                Events.OnSoundFX("Luisa NO nivel completado");
            else
                Events.OnSoundFX("Daniel NO nivel completado");
        }
        else
        {
            if (Random.Range(0, 10) < 5)
                Events.OnSoundFX("Luisa nivel completado");
            else
                Events.OnSoundFX("Daniel nivel completado");
        }
        Data.Instance.routes.SetPerfect(!commitError);
        vueltas = 0;
        Data.Instance.routes.NextLevel();
        if (Data.Instance.routes.gameID >= 7)
            EndSceneOn();
        else
            LoadNextGame();
    }
    public void LoadNextGame()
    {
        Events.OnMusic("marimba");
       GameData gameData =  Data.Instance.routes.GetActualGame();
       bool showMap = true;
       if (Data.Instance.routes.gameID == 0)
           showMap = false;

       switch(gameData.type)
       {
           case GameData.types.BRIDGE:
                   Data.Instance.LoadLevel("Game_Bridge", showMap); 
               break;
           case GameData.types.DOLPHIN:
                   Data.Instance.LoadLevel("Game_Dolphin", showMap); 
               break;
           case GameData.types.MONKEY:
                   Data.Instance.LoadLevel("Game_Monkey", showMap); 
               break;
           case GameData.types.FROGGER:
                   Data.Instance.LoadLevel("Game_Frogger", showMap); 
               break;
       }
    }
    void EndSceneOn()
    {
        Data.Instance.LoadLevel("Ending" + Data.Instance.routes.routeID, true);
    }
}
