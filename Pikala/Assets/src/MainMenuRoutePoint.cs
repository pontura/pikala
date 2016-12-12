using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuRoutePoint : MonoBehaviour {

    public GameObject doneImage;
    public Sprite imageNotPerfectSymbol;
    public Sprite imagePerfectSymbol;
    public GameObject imagePlayed;

    bool isPerfect;

    void Start()
    {
        if (imagePlayed != null)
        {
            imagePlayed.SetActive(false);
            imagePlayed.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        }
    }
	public void Init(bool isPerfect) {

        this.isPerfect = isPerfect;
        if (isPerfect)
        {
            doneImage.SetActive(true);
            doneImage.GetComponent<Image>().sprite = imagePerfectSymbol;
            GetComponent<Animator>().enabled = false;
        }
        else
        {
            doneImage.GetComponent<Image>().sprite = imageNotPerfectSymbol;
            SetOff();
        }        
        
    }
    public void SetPlayed()
    {
        if (imagePlayed != null)
            imagePlayed.SetActive(true);
    }
    public void SetUnPlayed()
    {
        if (imagePlayed != null)
            imagePlayed.SetActive(false);
    }
    public void SetOn()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("point_on");
    }
    public void SetOff()
    {
        if (isPerfect) return;
        GetComponent<Animator>().Play("point_off");
    }
}
