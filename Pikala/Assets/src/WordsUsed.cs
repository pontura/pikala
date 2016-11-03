﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WordsUsed : MonoBehaviour {

    public List<Word> words;
    public List<Word> shuffledWords;
    int lastWordSendedID = -1;

    [Serializable]
    public class Word
    {       
        public GameData.types gameType;
        public int id;       
    }
	void Start () {
        Events.OnAddWordToList += OnAddWordToList;
	}
    void OnDestroy()
    {
        Events.OnAddWordToList -= OnAddWordToList;
    }
    public void Empty()
    {
        words.Clear();
        lastWordSendedID = -1;
    }
    void OnAddWordToList(GameData.types gameType, int id)
    {
        Word newWord = new Word();
        newWord.gameType = gameType;
        newWord.id = id;
        words.Add(newWord);
    }
    Word lastWordAdded;
    public void Shuffle()
    {
        for (int a=0; a<100; a++)
        {
            int rand = UnityEngine.Random.Range(0, words.Count - 1);
            Word word1 = words[rand];
            Word word2 = words[0];
            words[0] = word1;
            words[rand] = word2;
        }
        lastWordAdded = null;
        foreach(Word word in words)
        {
            if (lastWordAdded == null || word.gameType != lastWordAdded.gameType)
                shuffledWords.Add(word);
        }
    }
    
    public Word GetNextWord()
    {
        if (lastWordSendedID < words.Count) lastWordSendedID++;
        return words[lastWordSendedID];        
    }
    public int GetActualID()
    {
        if (lastWordSendedID == -1) return 0;
        return lastWordSendedID;
    }
}
