using UnityEngine;
using System.Collections;

public class Perfect : MonoBehaviour {

    public GameObject panel;

	void Start () {
        SetOff();
        Events.OnPerfect += OnPerfect;
	}
	
	void OnDestroy () {
        Events.OnPerfect -= OnPerfect;
	}
    void OnPerfect()
    {
        print("_________________perfect");
        panel.SetActive(true);
        Invoke("SetOff", 4);
    }
    void SetOff()
    {
        panel.SetActive(false);
    }
}
