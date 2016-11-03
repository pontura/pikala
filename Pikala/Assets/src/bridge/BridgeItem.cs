using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BridgeItem : MonoBehaviour {

    public int id;
    public string letter;
    public TextMesh textMesh;
    public Slot slotAttached;
    public Vector3 originalPosition;

    public void Init( string letter)
    {
        this.letter = letter;
        string realLetter = Data.Instance.GetComponent<TextsBridge>().GetLetterReal(letter);
        textMesh.text = realLetter;
        this.originalPosition = transform.position;
    }
    public void SetActualSize()
    {
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }
    public void SetWrong(bool isWrong)
    {
        GetComponent<Animation>().enabled = isWrong;
        SetActualSize();
    }
 
 }
