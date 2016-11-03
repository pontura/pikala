using UnityEngine;
using System.Collections;

public class VoicesManager : MonoBehaviour
{
    public AudioClip[] felicitaciones;
    public AudioSource audioSource;
    public float volume;

    public void Awake()
    {
        volume = PlayerPrefs.GetFloat("SFXVol", 1);
        OnSoundsVolumeChanged(volume);
        Events.OnVoiceSay += OnVoiceSay;
        Events.OnVoiceSayFromList += OnVoiceSayFromList;
    }
    void OnDestroy()
    {
        Events.OnVoiceSay -= OnVoiceSay;
        Events.OnVoiceSayFromList -= OnVoiceSayFromList;
    }
    void OnSoundsVolumeChanged(float value)
    {
        audioSource.volume = value;
        volume = value;

        if (value == 0 || value == 1)
            PlayerPrefs.SetFloat("SFXVol", value);
    }
    AudioClip sayInDelay = null;
    void OnVoiceSayFromList(string listName, float delay)
    {
        if (listName == "")
            audioSource.Stop();
        else
        {
            AudioClip[] arr = null;
            switch (listName)
            {
                case "felicitaciones": arr = felicitaciones; break;
            }
            if (arr != null)
            {
                sayInDelay = felicitaciones[Random.Range(0, felicitaciones.Length)];
                Invoke("SayInDelay", delay);
            }
        }
    }
    void SayInDelay()
    {
        audioSource.PlayOneShot(sayInDelay);
    }
    void OnVoiceSay(string soundName)
    {
        switch (soundName.ToLower())
        {
            case "palabras/montana":
                soundName = "palabras/montana"; break;
            case "palabras/niño":
                soundName = "palabras/nino"; break;
            case "palabras/piña":
                soundName = "palabras/pina"; break;
        }

        if (soundName == "")
            audioSource.Stop();
        else
            audioSource.PlayOneShot(Resources.Load("SFX/" + soundName) as AudioClip);
        print("Dice: " + soundName);
    }
}
