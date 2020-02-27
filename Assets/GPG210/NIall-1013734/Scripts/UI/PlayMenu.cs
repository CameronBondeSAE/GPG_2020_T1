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
       SetupMenuUI.SetUpMenuUI.SetActive(true);
       playEvent.Invoke();
       
    }

    public void Exit()
    {
        Application.Quit();
    }
}
