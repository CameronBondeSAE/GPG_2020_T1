using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixerGroup MasterMixer;
    public AudioMixerGroup MusicMixer;
    public AudioMixerGroup SFXMixer;
    public PlayMenu playmenu;
    public CreditsMenu creditsMenu;
    public GameObject optionsMenu;


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

    public void Credits()
    {
        creditsMenu.creditsMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void Back()
    {
        playmenu.PlayMenuUI.SetActive(true);
        optionsMenu.SetActive(false);
    }
}