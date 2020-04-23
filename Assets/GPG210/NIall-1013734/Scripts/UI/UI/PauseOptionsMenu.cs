using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseOptionsMenu : MonoBehaviour
{
    public AudioMixerGroup MasterMixer;
    public AudioMixerGroup MusicMixer;
    public AudioMixerGroup SFXMixer;
    public PauseMenu pauseMenu;
    public GameObject pauseOptionsMenu;


    public void SetMasterVolume(float volume)
    {
        MasterMixer.audioMixer.SetFloat("Volume", volume);
    }

    public void SetMusicVolume(float musicvolume)
    {
        MusicMixer.audioMixer.SetFloat("MusicVolume", musicvolume);
    }

    public void SetSFXVolume(float sfxvolume)
    {
        SFXMixer.audioMixer.SetFloat("SFXVolume", sfxvolume);
    }

    public void Back()
    {
        pauseOptionsMenu.SetActive(false);
        pauseMenu.PauseMenuUI.SetActive(true);
    }
}