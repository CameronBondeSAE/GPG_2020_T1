using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseOptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public PauseMenu pauseMenu;
    public GameObject pauseOptionsMenu;


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void Back()
    {
        pauseOptionsMenu.SetActive(false);
        pauseMenu.PauseMenuUI.SetActive(true);
    }
}