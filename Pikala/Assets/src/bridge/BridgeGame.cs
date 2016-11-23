using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BridgeGame : MainClass
{
    public BridgeScenary bridgeScenary;
    public float bridgeScenary_x_separation;
    public BridgeScenary bridgeScenaryActive;
    public Transform bridgeScenaryActiveTransform;
    public Transform slotsContainer;
    public Transform itemsContainer;
    public Transform gameContainer;
    public Camera cam;
    public float cam_speed;

    public states state;
    public enum states
    {   
        LOADING,
        INTRO,
        PLAYING,
        READY,
        JUMPING,
        SCROLL
    }
    public Slot slot;

    public BridgeItem item;

    public int addLetters;

    public string[] letters;
    public string palabra;
    public string word;
    public List<string> wordsToUse;
    private AvatarsManager avatarsManager;
    private float speedJump = 7;
    private float timeBetweenJumps = 0.6f;
    private bool commitError;
    public int wordID;
    private float _x = -5.4f;

    public Animation anim;

    void Start()
    {
        avatarsManager = GetComponent<AvatarsManager>();
        letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V" };
        switch (Data.Instance.userdata.dificult)
        {
            case UserData.dificults.EASY:
                addLetters = 0; break;
            case UserData.dificults.MEDIUM:
                addLetters = 1; break;
            case UserData.dificults.HARD:
                addLetters = 2; break;
        }
        AddNewScene();
        Invoke("Restart", 0.1f);

        GetComponent<DragManager>().enabled = false;
        Events.OnTutorialReady += OnTutorialReady;
    }
    void OnDestroy()
    {
        Events.OnTutorialReady -= OnTutorialReady;
    }
    void OnTutorialReady()
    {
        GetComponent<DragManager>().enabled = true;
        anim.Stop();
    }
    void AddNewScene()
    {
        bridgeScenaryActive = Instantiate(bridgeScenary);
        bridgeScenaryActive.transform.SetParent(gameContainer);
        bridgeScenaryActive.transform.position = new Vector2(wordID * bridgeScenary_x_separation, 0);
    }
    void Restart()
    {
        int vueltaID = Data.Instance.levelsManager.bridges;
        TextsBridge.Vuelta vuelta = Data.Instance.GetComponent<TextsBridge>().vueltas[vueltaID];
        word = vuelta.ok;
        palabra = vuelta.palabra;
        print(word + " " + palabra);
        avatarsManager.Walk();
        Delayed();
    }
    void Delayed()
    {
        state = states.INTRO;
        wordsToUse.Clear();

        slotsContainer = bridgeScenaryActive.SlotContainer;
        itemsContainer = bridgeScenaryActive.itemsContainer;

        int totalSlots = word.Length;
        float separation = 9 / (float)totalSlots;
        float scales = 0.8f;
        for (int a = 0; a < totalSlots; a++)
        {
            Slot newSlot = Instantiate(slot);
            newSlot.transform.SetParent(slotsContainer);
            newSlot.transform.localPosition = new Vector2(separation*a, 0);
            newSlot.transform.localEulerAngles = new Vector3(0,0,Random.Range(0,6)-3);
            newSlot.transform.localScale = new Vector3(scales, scales, scales);
            newSlot.id = a + 1;
            wordsToUse.Add("" + word[a]);
        }

        for (int a = 0; a<addLetters; a++)
            wordsToUse.Add(letters[Random.Range(0,letters.Length)]);

        Utils.ShuffleListTexts(wordsToUse);

        separation = 9 / (float)wordsToUse.Count;

        for (int a = 0; a < wordsToUse.Count; a++)
        {
            BridgeItem newItem = Instantiate(item);
            newItem.transform.SetParent(itemsContainer);
            newItem.transform.localPosition = new Vector2(separation * a, 0);
            newItem.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 10) - 5);
            newItem.SetActualSize();
            newItem.id = a + 1;
            newItem.Init(wordsToUse[a]);
        }
    }
    public bool CheckToFill(BridgeItem item)
    {
        float dist = 1000;
        Slot snapSlot = null;
        foreach (Slot slot in bridgeScenaryActive.SlotContainer.GetComponentsInChildren<Slot>())
        {

            float newDist = Vector2.Distance(slot.transform.position, item.transform.position);
            //print(newDist + " " + slot.letter);
            if (newDist < 1 && (slot.letter == null || slot.letter == ""))
            {
                print("ENTRA");
                dist = newDist;
                snapSlot = slot;
            }
        }

        if (snapSlot)
        {
            item.slotAttached = snapSlot;
            item.transform.position = snapSlot.transform.position;
            item.transform.localEulerAngles = snapSlot.transform.localEulerAngles;
            snapSlot.SetLetter(item.letter);
            CheckGameStatus();
            return true;
        }
        else
            return false;
    }
    void StartGame()
    {
        state = states.PLAYING;
        avatarsManager.Idle();
    }
    void CheckGameStatus()
    {
        bool wrong = false;
        int id = 0;
        int totalWordsInGame = 0;
        foreach (Slot slot in bridgeScenaryActive.SlotContainer.GetComponentsInChildren<Slot>())
        {
            if (slot.letter != "") totalWordsInGame++;
            if (slot.letter != "" + word[id])
            {
                if (slot.letter != "")
                {
                    print("ERROR " + slot.letter  + "  Deberia ser: " + word[id]);
                    //commitError = true;
                   // TakeItemOutFromSlot(slot);
                }
                wrong = true;    
            }
            id++;
        }
        if (!wrong)
        {
            Events.OnAddWordToList(GameData.types.BRIDGE, Data.Instance.levelsManager.bridges);
            wordID++;
            Events.OnOkWord(GameData.types.BRIDGE);          

            StartCoroutine(Jumps());
            return;
        }
        else
        {
            Events.OnAddWrongWord(palabra);
        }
        if (totalWordsInGame == word.Length)
        {
            if (wrong)
            {
                commitError = true;
            }
            id = 0;
            foreach (Slot slot in bridgeScenaryActive.SlotContainer.GetComponentsInChildren<Slot>())
            {
                if (slot.letter != "" + word[id])
                {
                    GetItemInSlot(slot).SetWrong(true);
                }
                id++;
            }
        }
    }
    void TakeItemOutFromSlot(Slot slot)
    {
        foreach (BridgeItem item in bridgeScenaryActive.itemsContainer.GetComponentsInChildren<BridgeItem>())
        {
            if (item.slotAttached && item.slotAttached.id == slot.id)
            {
                slot.EmptyLetters();
                item.slotAttached = null;
                item.transform.position = item.originalPosition;
            }
        }
    }
    private BridgeItem GetItemInSlot(Slot slot)
    {
        foreach (BridgeItem item in bridgeScenaryActive.itemsContainer.GetComponentsInChildren<BridgeItem>())
        {
            if (item.slotAttached && item.slotAttached.id == slot.id)
            {
                return item;
            }
        }
        return null;
    }
    
    IEnumerator Jumps()
    {
        yield return new WaitForSeconds(0.5f);        
        state = states.JUMPING;
        foreach (Slot slot in bridgeScenaryActive.SlotContainer.GetComponentsInChildren<Slot>())
        {
            _x = slot.transform.position.x-0.4f;
            print(_x);
            avatarsManager.Jump();
            yield return new WaitForSeconds(timeBetweenJumps);
        }
        _x += 1.7f;
        avatarsManager.Jump();

        if (Data.Instance.levelsManager.vueltas == Data.Instance.routes.GetTotalWordsOfActiveGame())
            Events.OnGameReady();

        yield return new WaitForSeconds(0.2f);
        _x = 13.7f + (bridgeScenary_x_separation * (wordID-1));
        avatarsManager.Run();
        if (Data.Instance.levelsManager.vueltas == Data.Instance.routes.GetTotalWordsOfActiveGame())
        {
            Events.OnVoiceSayFromList("felicitaciones", 0.8f);
            Invoke("LevelComplete", 3);
            state = states.JUMPING;
            _x += 4f;
        }
        else
        {
            AddNewScene();
            state = states.SCROLL;
        }
        yield return null;
    }
    void LevelComplete()
    {
        if (!commitError)
            Events.OnPerfect();
        else
            Events.OnGood();
        Events.OnLevelComplete(GameData.types.BRIDGE, commitError);
    }
    void Update()
    {
        if (state == states.INTRO)
        {
            Vector2 pos = avatarsManager.nene.transform.position;
            if (pos.x < _x)
                pos.x += Time.deltaTime * (speedJump/2);
            else
                StartGame();
            UpdatePositions(pos);
        } else
        if (state == states.SCROLL)
        {
            if (cam.transform.position.x < bridgeScenary_x_separation * wordID)
                cam.transform.position = new Vector3(cam.transform.position.x + (Time.deltaTime * cam_speed), 0, -10);
            else
            {
                Restart();
            }
        } 
        if (state == states.JUMPING || state == states.SCROLL)
        {
            Vector2 pos = avatarsManager.nene.transform.position;
            if (pos.x < _x)
                pos.x += Time.deltaTime * speedJump;

            UpdatePositions(pos);
        }
    }
    void UpdatePositions(Vector2 pos)
    {
        avatarsManager.nene.transform.position = pos;
        pos.y = avatarsManager.nena.transform.position.y;
        pos.x += 0.8f;
        avatarsManager.nena.transform.position = pos;
    }
}
