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
    public GameObject HowToHost;

    public GameManager gameManager;
    public RTSNetworkManager rtsNetworkManager;
    public string testServer;

    public void Start()
    {
        testServer = "14.201.222.143";
        PlayMenuUI = GetComponent<PlayMenu>();
    }


    public void HostGame()
    {
        multiplayerMenu.SetActive(false);
        HowToHost.SetActive(true);
    }


    public void JoinGame()
    {
        multiplayerMenu.SetActive(false);
        rtsNetworkManager.StartClient();
        gameManager.OnStartGameEvent();

    }


    public void Back()
    {
        multiplayerMenu.SetActive(false);
        PlayMenuUI.PlayMenuUI.SetActive(true);
    }

    public void SetIP(string IP)
    {
        rtsNetworkManager.SetHostname(IP);
    }

    public void TestServer()
    {
        rtsNetworkManager.SetHostname(testServer);
        multiplayerMenu.SetActive(false);
        rtsNetworkManager.StartClient();
        gameManager.OnStartGameEvent();
    }
}