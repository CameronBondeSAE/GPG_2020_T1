using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{

    public GameObject PlayMenuUI;
    
    public void Awake()
    {
        PlayMenuUI.SetActive(true);
    }


    public void Play()
    {
       PlayMenuUI.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
