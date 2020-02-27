using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{

    public GameObject PlayMenuUI;
    public SetupMenu SetupMenuUI;
    
    
    public void Awake()
    {
        PlayMenuUI.SetActive(true);
        SetupMenuUI = GetComponent<SetupMenu>();
    }


    public void Play()
    {
       PlayMenuUI.SetActive(false);
       SetupMenuUI.SetUpMenuUI.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
