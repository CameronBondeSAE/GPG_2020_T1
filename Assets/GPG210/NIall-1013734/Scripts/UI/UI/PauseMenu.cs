using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;

    public GameObject PauseMenuUI;
    public PlayMenu PlayMenuUI;
    public PauseOptionsMenu pauseOptionsMenu;

    private RTSNetworkManager rtsNetworkManager;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        IsPaused = false;
    }

    public void Options()
    {
        PauseMenuUI.SetActive(false);
        pauseOptionsMenu.pauseOptionsMenu.SetActive(true);
    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        IsPaused = true;
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        PlayMenuUI.PlayMenuUI.SetActive(true);
        PauseMenuUI.SetActive(false);
        Application.LoadLevel(Application.loadedLevel);
        IsPaused = false;
        
        
        //TODO: Client disconnects only disconnects client from server / host disconnects so server disconnects along side host client.

        if (NetworkClient.isLocalClient == true)
        {
            rtsNetworkManager.isNetworkActive = true;
        }

        else
        {
            rtsNetworkManager.isNetworkActive = false;
        }
    }
}