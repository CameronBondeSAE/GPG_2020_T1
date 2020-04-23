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
    public SetupMenu SetupMenuUI;
    public OptionsMenu optionsMenu;

	public GameManager gameManager;
	public RTSNetworkManager rtsNetworkManager;

    public void Awake()
    {
        PlayMenuUI.SetActive(true);
        SetupMenuUI = GetComponent<SetupMenu>();
    }


    public void Play()
    {
       PlayMenuUI.SetActive(false);
	   rtsNetworkManager.StartHost();
	   gameManager.OnStartGameEvent();
	}

    public void Options()
    {
        PlayMenuUI.SetActive(false);
        optionsMenu.optionsMenu.SetActive(true);
    }

    public void Setup()
    {
        SetupMenuUI.SetUpMenuUI.SetActive(true);
        PlayMenuUI.SetActive(false);
    }
    

    public void Exit()
    {
        Application.Quit();
    }
}
