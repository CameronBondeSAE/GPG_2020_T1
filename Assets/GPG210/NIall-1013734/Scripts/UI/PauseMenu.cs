using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool IsPaused = false;

    public GameObject PauseMenuUI;
    public PlayMenu PlayMenuUI;
    
    
    
    
    
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

   public void ExitGame()
    {
        Debug.Log("Exiting Game");
        PlayMenuUI.PlayMenuUI.SetActive(true);
        PauseMenuUI.SetActive(false);
        
    }

}
