using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MuteButton : MonoBehaviour
{
    [SerializeField] GameObject gameCamera;
    [SerializeField] TextMeshProUGUI muteButtonText;
    private bool isMuted = false;

    AudioSource backgroundMusic;

    public void Start()
    {
        AudioSource[] sounds = gameCamera.GetComponents<AudioSource>();
        backgroundMusic = sounds[0];
    }


    public void ButtonPress()
    {
        if (!isMuted)
        {
            isMuted = true;
            backgroundMusic.volume = 0;
            muteButtonText.text = "Unmute Music";
        }
        else
        {
            isMuted = false;
            backgroundMusic.volume = 0.5f;
            muteButtonText.text = "Mute Music";
        }
    }

}
