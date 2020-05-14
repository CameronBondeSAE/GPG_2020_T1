using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToHostMenu : MonoBehaviour
{


    public GameObject howToHost;
    public GameObject multiplayerMenu;

    public RTSNetworkManager rtsNetworkManager;
    public GameManager gameManager;


    public void Host()
    {
        howToHost.SetActive(false);
        multiplayerMenu.SetActive(false);
        rtsNetworkManager.StartHost();
        gameManager.OnStartGameEventInvocation();
        
    }

    public void Back()
    {
        howToHost.SetActive(false);
        multiplayerMenu.SetActive(true);
    }

}
