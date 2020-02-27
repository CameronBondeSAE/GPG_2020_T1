using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool IsPaused = false;

    public GameObject PauseMenuUI;
    public PlayMenu PlaymenuUI;
    
    
    
    
    
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

    public void LoadMenu()
    {
        Debug.Log("Loading Menu");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        PlaymenuUI.PlayMenuUI.SetActive(true);
    }

}
