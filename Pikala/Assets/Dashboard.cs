using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Dashboard : MainClass {

    public DashboardLine dashboardLine;
    public Transform content1;
    public Transform content2;
    public Transform content3;
    public Transform content4;

    public GameObject off1;
    public GameObject off2;
    public GameObject off3;

    void Start () {
        Data.Instance.GetComponent<Settings>().settingsButton.SetActive(false);
        ProgressData data = Data.Instance.GetComponent<ProgressData>();
        Create(content1, data.well1);
        Create(content2, data.well2);
        Create(content3, data.well3);
        Create(content4, data.well4);

        off1.SetActive(true);
        off2.SetActive(true);
        off3.SetActive(true);


        if (Data.Instance.GetComponent<Routes>().unlockedRoute >0)
            off1.SetActive(false);
        if (Data.Instance.GetComponent<Routes>().unlockedRoute >1)
            off2.SetActive(false);
        if (Data.Instance.GetComponent<Routes>().unlockedRoute >2)
            off3.SetActive(false);
    }
	void Create(Transform content, List<string> words)
    {
        foreach (string word in words)
        {
            DashboardLine newLine = Instantiate(dashboardLine);            
            newLine.transform.SetParent(content);
            newLine.Init(word.ToUpper());
            newLine.transform.localScale = Vector3.one;
        }
    }
    public void Close()
    {
        Data.Instance.GetComponent<Settings>().settingsButton.SetActive(true);
        Data.Instance.LoadLevel("MainMenu", false);
    }
}
