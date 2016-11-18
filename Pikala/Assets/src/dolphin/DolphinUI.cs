using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DolphinUI : MonoBehaviour
{
    public DolphinGameManager dolphinGameManager;
    public Image filledImage;

    public PhrasesUI UI_Phrases;
    public PicturesUI UI_Pictures;
    public WordUI UI_Word;
    public WordsUsed.Word activeWord;

    public int okWords;
    private IEnumerator ResetRoutine;
    public string title;
    public string toSay;
    public string ok;
    public List<string> wrongWords;
    public DolphinGame dolphinGame;

    private bool commitError;

    public states state;
    public enum states
    {
        PLAYING,
        WAITING
    }

    void Start()
    {
        filledImage.fillAmount = 0;
        okWords = 0;
        Events.OnGotWord += OnGotWord;
        Events.OnSayCorrectWord += OnSayCorrectWord;
        Events.OnSayCorrectWord_with_beep += OnSayCorrectWord_with_beep;
        Events.OnTutorialReady += OnTutorialReady;

        if (!Data.Instance.levelsManager.dolphin_IntroPlayed)
        {
            Data.Instance.levelsManager.dolphin_IntroPlayed = true;
            GetComponent<Intro>().Init();
        }
        else
        {
            Invoke("DelayedOnTutorialReady", 0.1f);
        }
    }
    void DelayedOnTutorialReady()
    {
        GetComponent<Animation>().Stop();
        GetComponent<Animation>().enabled = false;
        Events.OnTutorialReady();
    }
    void OnTutorialReady()
    {
        Events.StartGame();
        Invoke("GetNextWord", 1);
    }
    public void GetNextWord()
    {
        state = states.PLAYING;
        title = "";
        activeWord = Data.Instance.wordsUsed.GetNextWord();
        TextsBridge textsBridge = Data.Instance.GetComponent<TextsBridge>();

        wrongWords.Clear();

        if (activeWord.gameType == GameData.types.BRIDGE)
        {
            UI_Phrases.gameObject.SetActive(false);
            UI_Pictures.gameObject.SetActive(true);
            UI_Word.gameObject.SetActive(false);
            string word = textsBridge.vueltas[activeWord.id].palabra.ToUpper();
            ok = textsBridge.GetPalabraReal(word);
            toSay = textsBridge.GetPalabraReal(textsBridge.vueltas[activeWord.id].palabra);
            foreach (TextsBridge.Vuelta vuelta in textsBridge.vueltas)
            {
                string realWord = textsBridge.GetPalabraReal(vuelta.ok);
                if (realWord != ok)
                    wrongWords.Add(realWord);
            }

            UI_Pictures.Init(ok);
        }
        else if (activeWord.gameType == GameData.types.FROGGER)
        {
            UI_Phrases.gameObject.SetActive(false);
            UI_Pictures.gameObject.SetActive(false);
            UI_Word.gameObject.SetActive(true);

            TextsFrogger.Vuelta vueltaReal = Data.Instance.GetComponent<TextsFrogger>().vueltas1[0];

            foreach( string wrongWord in vueltaReal.wrong)
                wrongWords.Add(wrongWord);

            foreach (TextsFrogger.Vuelta vuelta in Data.Instance.GetComponent<TextsFrogger>().vueltas1)
                if(vuelta.id == activeWord.id)
                    vueltaReal = vuelta;
            foreach (TextsFrogger.Vuelta vuelta in Data.Instance.GetComponent<TextsFrogger>().vueltas2)
                if (vuelta.id == activeWord.id)
                    vueltaReal = vuelta;
            foreach (TextsFrogger.Vuelta vuelta in Data.Instance.GetComponent<TextsFrogger>().vueltas3)
                if (vuelta.id == activeWord.id)
                    vueltaReal = vuelta;

            ok = vueltaReal.ok.ToUpper();
            toSay = vueltaReal.ok;

            string lastTwoChars = ok.Substring(ok.Length - 2);

            UI_Word.Init(lastTwoChars.ToUpper());
        }
        else
        {
            UI_Phrases.gameObject.SetActive(true);
            UI_Pictures.gameObject.SetActive(false);
            UI_Word.gameObject.SetActive(false);
            ok = Data.Instance.GetComponent<TextsMonkeys>().vueltas[activeWord.id].ok.ToUpper();
            title = Data.Instance.GetComponent<TextsMonkeys>().vueltas[activeWord.id].title.ToUpper();
            toSay = Data.Instance.GetComponent<TextsMonkeys>().vueltas[activeWord.id].audio;
            foreach (string wrongWord in Data.Instance.GetComponent<TextsMonkeys>().vueltas[activeWord.id].wrong)
                wrongWords.Add(wrongWord);

            UI_Phrases.Init(ok);
            ReplaceOkWordWith("___");
        }       
    }
    public void SoundClicked()
    {
        Events.OnVoiceSay(ok);
    }
    void OnDestroy()
    {
        Events.OnTutorialReady -= OnTutorialReady;
        Events.OnGotWord -= OnGotWord;
        Events.OnSayCorrectWord -= OnSayCorrectWord;
        Events.OnSayCorrectWord_with_beep -= OnSayCorrectWord_with_beep;
    }
    void ResetWord()
    {
        ReplaceOkWordWith("___");
    }
    private void OnGotWord(string text)
    {
        text = text.ToUpper();
        if (ResetRoutine != null)
            StopCoroutine(ResetRoutine);

        if (text != ok)
        {
            commitError = true;
            if (activeWord.gameType == GameData.types.MONKEY)
                UI_Phrases.GetComponent<Animation>().Play("wrong");
            else if (activeWord.gameType == GameData.types.FROGGER)
                UI_Word.GetComponent<Animation>().Play("wrong");
            else if (activeWord.gameType == GameData.types.BRIDGE)
                UI_Pictures.GetComponent<Animation>().Play("wrong");

            if (ResetRoutine != null) StopCoroutine(ResetRoutine);
            ReplaceOkWordWith(text);
            ResetRoutine = ResetNextWord();
            StartCoroutine(ResetRoutine);
            Events.OnSoundFX("mistakeWord");
        }
        else
        {
            if (activeWord.gameType == GameData.types.MONKEY)
                UI_Phrases.GetComponent<Animation>().Play("ok");
            else if (activeWord.gameType == GameData.types.FROGGER)
                UI_Word.GetComponent<Animation>().Play("ok");
            else if (activeWord.gameType == GameData.types.BRIDGE)
                UI_Pictures.GetComponent<Animation>().Play("ok");

            okWords++;
            ReplaceOkWordWith(text);
            Events.OnSoundFX("correctWord");
            Events.OnVoiceSayFromList("felicitaciones", 0.5f);
                    
            if (okWords >= Data.Instance.wordsUsed.words.Count)
            {
                dolphinGameManager.LevelComplete();
                Data.Instance.wordsUsed.Empty();
            }
            Events.OnOkWord(GameData.types.DOLPHIN);
            StartCoroutine(Next());

            float totalWordsUsed = (float)Data.Instance.wordsUsed.words.Count;
            float actualWord = (float)Data.Instance.wordsUsed.GetActualID() +1;

            float progress = actualWord / totalWordsUsed;
            filledImage.fillAmount = progress;

            print("________________" + totalWordsUsed + " " + actualWord + " progress: " + progress);
        }
    }
    void OnShowPicture()
    {
        UI_Phrases.gameObject.SetActive(false);
        UI_Word.gameObject.SetActive(false);

        OnSayCorrectWord();
        UI_Pictures.Init(toSay);        
    }
    IEnumerator Next()
    {
        state = states.WAITING;
        yield return new WaitForSeconds(2.1f);        

        if (okWords >= Data.Instance.wordsUsed.words.Count)
        {
            yield return new WaitForSeconds(2f);    
            LevelComplete();
        }
        else
        {
            OnShowPicture();
            yield return new WaitForSeconds(3);
            UI_Pictures.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            GetNextWord();
        }
    }
    void OnSayCorrectWord_with_beep()
    {
        print("__________OnSayCorrectWord " + toSay.ToLower());
        Events.OnVoiceSay("palabras/" + toSay.ToLower() + "BEEP");
    }
    void OnSayCorrectWord()
    {
		print("__________OnSayCorrectWord " + toSay.ToLower());
        Events.OnVoiceSay("palabras/" + toSay.ToLower());
		
		//if(UI_Pictures.gameObject)
			//UI_Pictures.Init(toSay);
    }
    void LevelComplete()
    {
        if (!commitError)
            Events.OnPerfect();
        else
            Events.OnGood();
        Events.OnLevelComplete(GameData.types.DOLPHIN, false);
    }
    IEnumerator ResetNextWord()
    {
        yield return new WaitForSeconds(3);
        ResetWord();
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
        UI_Phrases.Init(titleReal);
    }
    public void MainMenu()
    {
        Data.Instance.LoadLevel("MainMenu", false);
    }
}
