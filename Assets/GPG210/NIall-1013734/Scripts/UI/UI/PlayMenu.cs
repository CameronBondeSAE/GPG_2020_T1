using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem.Editor;

public class PlayMenu : MonoBehaviour
{
	public GameObject PlayMenuUI;
    public MultiplayerMenu multiplayerMenu;
    public OptionsMenu optionsMenu;

	public GameManager gameManager;

    public void Awake()
    {
        PlayMenuUI.SetActive(true);
        multiplayerMenu = GetComponent<MultiplayerMenu>();
    }


    public void Play()
    {
       PlayMenuUI.SetActive(false);
	   gameManager.OnStartGameEvent();
	}

    public void Options()
    {
        PlayMenuUI.SetActive(false);
        optionsMenu.optionsMenu.SetActive(true);
    }

    public void Setup()
    {
        multiplayerMenu.multiplayerMenu.SetActive(true);
        PlayMenuUI.SetActive(false);
    }
    

    public void Exit()
    {
        Application.Quit();
    }
}
