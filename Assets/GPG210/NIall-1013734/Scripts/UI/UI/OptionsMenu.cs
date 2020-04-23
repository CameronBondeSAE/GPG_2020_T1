using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public PlayMenu playmenu;
    public CreditsMenu creditsMenu;
    public GameObject optionsMenu;
    


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
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
