﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneysUI : MonoBehaviour {

    private IEnumerator ResetRoutine;
    private string title;
    private string ok;

    public PicturesUI picturesUI;
    public PhrasesUI ui;
   
    private TextsMonkeys.Vuelta vuelta;
    private int okWords;
    private bool commitError;

    public Animator tutorialAnim;

    

    bool started;
    void Start()
    {
        picturesUI.gameObject.SetActive(false);
        Events.OnSayCorrectWord += OnSayCorrectWord;
        Events.OnSayCorrectWord_with_beep += OnSayCorrectWord_with_beep;
        EventsMonkey.OnGotBanana += OnGotBanana;
        Events.OnTutorialReady += OnTutorialReady;

        if (!Data.Instance.levelsManager.monkey_IntroPlayed)
        {
            ui.gameObject.SetActive(false);
            Data.Instance.levelsManager.monkey_IntroPlayed = true;
            GetComponent<Intro>().Init();
            tutorialAnim.Play("monkeyTutorial");
        }
        else
        {
            Invoke("DelayedOnTutorialReady", 0.5f);     
        }
        Invoke("Init", 0.1f);
    }
    void DelayedOnTutorialReady()
    {
        Events.OnTutorialReady();
    }
    void Init()
    {
        picturesUI.gameObject.SetActive(false);
        ui.gameObject.SetActive(true);
        ui.GetComponent<Animation>().Play("uiStart");
        vuelta = Data.Instance.GetComponent<TextsMonkeys>().GetVueltaByLevel();
        title = vuelta.title;
        ok = vuelta.ok;
        ReplaceOkWordWith("___");
    }
    public void SoundClicked()
    {
        Events.OnVoiceSay("palabras/" + vuelta.audio);
    }
    void OnDestroy()
    {
        Events.OnTutorialReady -= OnTutorialReady;
        Events.OnSayCorrectWord -= OnSayCorrectWord;
        Events.OnSayCorrectWord_with_beep -= OnSayCorrectWord_with_beep;
        EventsMonkey.OnGotBanana -= OnGotBanana;
    }
    void OnTutorialReady()
    {
        started = true;
         ui.gameObject.SetActive(true);
         StartCoroutine(WaitUntilReady());
	}
    IEnumerator WaitUntilReady()
    {
        while (Data.Instance.GetComponent<TextsMonkeys>().GetVueltaByLevel() == null)
            yield return new WaitForEndOfFrame();

        Init();
    }
    void ResetWord()
    {
        ReplaceOkWordWith("___");
    }
    private void OnGotBanana(string text)
    {
        print("OnGotBanana   " + text);

        if (ResetRoutine != null)
            StopCoroutine(ResetRoutine);

        if (text != ok)
        {
            if(ResetRoutine != null) StopCoroutine(ResetRoutine);
            ReplaceOkWordWith(text);
            ResetRoutine = ResetNextWord();
            StartCoroutine(ResetRoutine);
            ui.GetComponent<Animation>().Play("wrong");
            Events.OnSoundFX("mistakeWord");
            commitError = true;
            Events.OnAddWrongWord(title);
        }
        else
        {
            Events.OnAddWordToList(GameData.types.MONKEY, Data.Instance.levelsManager.monkeys);
            okWords++;
            ReplaceOkWordWith(text);
            ui.GetComponent<Animation>().Play("ok");
            Events.OnSoundFX("correctWord");
            Events.OnOkWord(GameData.types.MONKEY);            
            Events.OnVoiceSayFromList("felicitaciones", 0.8f);
            Invoke("DiceFrase", 2.2f);
        }
    }
    void OnSayCorrectWord_with_beep()
    {
        Events.OnVoiceSay("palabras/" + vuelta.audio + "BEEP");
    }
    void OnSayCorrectWord()
    {
        Events.OnVoiceSay("palabras/" + vuelta.audio);
    }
    void DiceFrase()
    {
        picturesUI.gameObject.SetActive(true);
        picturesUI.Init(vuelta.audio);

        OnSayCorrectWord();
        if (okWords < Data.Instance.routes.GetTotalWordsOfActiveGame())
        {
            Invoke("NextWord", 4);
        }
        else
        {
            print("OnLevelComplete " + commitError);
            if (!commitError)
            {
                
                Invoke("OnLevelComplete", 3);
            }
            else
            {
                Invoke("OnLevelComplete", 3);
            }
        }
    }
    void NextWord()
    {
        Init();
    }
    void OnLevelComplete()
    {

        if(!commitError)
            Events.OnPerfect();
        else
            Events.OnGood();

        Events.OnLevelComplete(GameData.types.MONKEY, commitError);
    }
    IEnumerator ResetNextWord()
    {
        yield return new WaitForSeconds(3);
        ResetWord();
        ui.GetComponent<Animation>().Play("uiStart");
    }
    private void ReplaceOkWordWith(string NewWord)
    {
        string[] all = title.Split(" "[0]);
        string titleReal = "";
        foreach (string word in all)
        {
            if (word == ok)
                titleReal += NewWord + " ";
            else
                titleReal += word + " ";
        }
        ui.Init(titleReal);
    }
    public void MainMenu()
    {
        Data.Instance.LoadLevel("MainMenu", false);
    }
}
