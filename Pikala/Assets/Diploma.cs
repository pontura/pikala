using UnityEngine;
using System.Collections;

public class Diploma : MonoBehaviour
{
    public int diploma;
    public GameObject panel;

    void Start()
    {
        SetOff();
        Events.WinDiploma += WinDiploma;
        diploma = PlayerPrefs.GetInt("diploma", 0);
    }

    void OnDestroy()
    {
        Events.WinDiploma -= WinDiploma;
    }
    void WinDiploma()
    {
        if(diploma == 0)
        PlayerPrefs.SetInt("diploma", 1);
        diploma = 1;
        panel.SetActive(true);
        Invoke("SetOff", 4);
    }
    void SetOff()
    {
        panel.SetActive(false);
    }
}
