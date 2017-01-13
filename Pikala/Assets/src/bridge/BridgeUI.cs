using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BridgeUI : MonoBehaviour
{
    public Text field;
    public PicturesUI pictureUI;
    public Animation anim;
    private string word;
    private string lastWord;

    public Animator tutorialAnim;

    private TextsBridge textsBridge;
    public int totalWordsPlayed;

    void Awake()
    {
        textsBridge = Data.Instance.GetComponent<TextsBridge>();
    }
    void Start()
    {
       
        totalWordsPlayed = 0;
        Events.OnOkWord += OnOkWord;
        Events.OnGameReady += OnGameReady;
        Events.OnSayCorrectWord += OnSayCorrectWord;
        Events.OnTutorialReady += OnTutorialReady;

        if (!Data.Instance.levelsManager.bridge_IntroPlayed)
        {
            Data.Instance.levelsManager.bridges = 1;
            //pictureUI.gameObject.SetActive(false);
            Data.Instance.levelsManager.bridge_IntroPlayed = true;
            GetComponent<Intro>().Init();
        }
        else
        {
            pictureUI.gameObject.SetActive(false);
            Invoke("DelayedOnTutorialReady", 0.1f);
        }
    }
    void DelayedOnTutorialReady()
    {
        Events.OnTutorialReady();
    }
    void OnDestroy()
    {
        Events.OnSayCorrectWord -= OnSayCorrectWord;
        Events.OnOkWord -= OnOkWord;
        Events.OnGameReady -= OnGameReady;
        Events.OnTutorialReady -= OnTutorialReady;
    }
    void OnTutorialReady()
    {
       
        Invoke("Delayed", 0.1f);
        tutorialAnim.Stop();
    }
    string realWord;
    string lastRealWord;
    void Delayed()
    {
        anim.Play("uiStart");
        int vuelta = Data.Instance.levelsManager.bridges;
        word = Data.Instance.GetComponent<TextsBridge>().vueltas[vuelta].ok;
        lastWord = word;

        realWord = textsBridge.GetPalabraReal(Data.Instance.GetComponent<TextsBridge>().vueltas[vuelta].palabra);
        lastRealWord = realWord;
        field.text = realWord;
        Invoke("DelayToWord", 1.5f);      
    }
    void DelayToWord()
    {
        pictureUI.gameObject.SetActive(true);
        pictureUI.Init(realWord);

        if (totalWordsPlayed < 4)
            OnSayCorrectWord();
    }
    void OnGameReady()
    {
        gameObject.SetActive(false);
    }
    private void OnOkWord(GameData.types type)
    {
        totalWordsPlayed++;
        pictureUI.Hide();
       // lastWord = word;
        anim.Play("ok");
        Events.OnSoundFX("correctWord");
        StartCoroutine(SayWords());
    }
    IEnumerator SayWords()
    {
       // yield return new WaitForSeconds(2);
        int id = 0;
        while (id < lastWord.Length)
        {
            yield return new WaitForSeconds(0.55f);
            string letter = lastWord.Substring(id, 1).ToLower();
            Debug.Log("letter: " + letter);
            
            if (letter == "ñ")
                letter = "enie";

            Events.OnSoundFX("letras/" + letter);
            id++;
        }
        yield return new WaitForSeconds(0.5f);
        OnSayCorrectWord();

        Invoke("Delayed", 1);

        yield return null;        
    }
    void OnSayCorrectWord()
    {
        string realWord = textsBridge.GetPalabraReal(lastRealWord);
        Events.OnVoiceSay("palabras/" + realWord);
    }
    public void MainMenu()
    {
        Data.Instance.LoadLevel("MainMenu", false);
    }
}
