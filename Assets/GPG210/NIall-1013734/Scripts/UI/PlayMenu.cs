using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayMenu : MonoBehaviour
{

    public event Action playEvent;
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
       playEvent.Invoke();
       
    }

    public void Options()
    {
        
    }

    public void Setup()
    {
        SetupMenuUI.SetUpMenuUI.SetActive(true);
        PlayMenuUI.SetActive(false);
    }
    

    public void Exit()
    {
        Application.Quit();
    }
}
