using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PicturesUI : MonoBehaviour {

    public Image imageContainer;

    public void Init(string imageName)
    {
        gameObject.SetActive(true);
        switch (imageName.ToLower())
        {
            case "montaña":
                imageName = "montana"; break;
            case "niño":
                imageName = "nino"; break;
            case "piña":
                imageName = "pina"; break;
            case "lamochilapequeña":
                imageName = "lamochilapequena"; break;
            case "laniñaalegre":
                imageName = "laninaalegre"; break;
        }
        string url = "pictures/picture_" + imageName.ToLower(); //.ToLower();
        print("PicturesUI " + url);
        Sprite sprite = Resources.Load<Sprite>(url);
        imageContainer.sprite = sprite;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Pressed()
    {
        Events.OnSayCorrectWord();
    }
}
