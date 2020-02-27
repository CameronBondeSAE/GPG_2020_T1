using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool IsPaused = false;

    public GameObject PauseMenuUI;
    
    
    
    
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

   public  void Pause()
    {
        PauseMenuUI.SetActive(true);
        IsPaused = true;
    }

    public void LOadMenu()
    {
        Debug.Log("Loading Menu");
    }

    public void ExitGame()
    {
        Debug.Log("Exitting Game");
        Application.Quit();
    }

}
