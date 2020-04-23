using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{
    public GameObject multiplayerMenu;
    public PlayMenu PlayMenuUI;

    public GameManager gameManager;
    public RTSNetworkManager rtsNetworkManager;

    public void Start()
    {
        PlayMenuUI = GetComponent<PlayMenu>();
    }


    public void HostGame()
    {
        multiplayerMenu.SetActive(false);
        rtsNetworkManager.StartHost();
        gameManager.OnStartGameEvent();
    }


    public void JoinGame()
    {
        multiplayerMenu.SetActive(false);
        gameManager.OnStartGameEvent();
        rtsNetworkManager.StartClient();
    }


    public void Back()
    {
        multiplayerMenu.SetActive(false);
        PlayMenuUI.PlayMenuUI.SetActive(true);
    }
}