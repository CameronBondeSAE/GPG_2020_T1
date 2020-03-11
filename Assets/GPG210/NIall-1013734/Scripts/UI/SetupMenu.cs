using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupMenu : MonoBehaviour
{

    public GameObject SetUpMenuUI;
    public PlayMenu PlayMenuUI;

    public void Start()
    {
        PlayMenuUI = GetComponent<PlayMenu>();
    }


    public void SinglePlayer()
    {
        SetUpMenuUI.SetActive(false);
    }

    public void MultiPlayer()
    {
        SetUpMenuUI.SetActive(false);
    }

    public void ControlsGuide()
    {
        
    }

    public void Back()
    {
        SetUpMenuUI.SetActive(false);
        PlayMenuUI.PlayMenuUI.SetActive(true);
        
    }

}
