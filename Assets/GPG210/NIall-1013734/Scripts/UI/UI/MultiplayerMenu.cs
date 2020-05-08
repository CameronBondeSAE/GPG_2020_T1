using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
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
    [SerializeField]
    public string testServer = "14.201.222.143";

    public void Start()
    {
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
		Debug.Log("isClient???????? : "+NetworkClient.isConnected);
        rtsNetworkManager.StartClient();
		Debug.Log("2 isClient???????? : "+NetworkClient.isConnected);
        gameManager.OnStartGameEventInvocation();

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
        gameManager.OnStartGameEventInvocation();
    }
}