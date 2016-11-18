using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

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
        OnShare();
    }
    void OnShare()
    {
        print("OnEnable");

        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        string destination = Path.Combine(Application.persistentDataPath, "diploma.png");
        Debug.Log(destination);

        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);

        File.WriteAllBytes(destination, bytes);

        Data.Instance.GetComponent<NativeShare>().Share("Pikala" + "the game", destination, destination, "");

        Invoke("SetOff", 4);
    }
    void SetOff()
    {
        panel.SetActive(false);
    }
}
