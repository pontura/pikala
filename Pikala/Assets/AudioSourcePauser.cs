﻿using UnityEngine;
using System.Collections;

public class AudioSourcePauser : MonoBehaviour {

    bool pausedByTimeScale;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
	void Update () {
        if (Time.timeScale == 0 && audioSource.isPlaying)
        {
            pausedByTimeScale = true;
            audioSource.Pause();
        } else if (Time.timeScale > 0 && pausedByTimeScale)
        {
            pausedByTimeScale = false;
            audioSource.Play();
        }
    }
}
