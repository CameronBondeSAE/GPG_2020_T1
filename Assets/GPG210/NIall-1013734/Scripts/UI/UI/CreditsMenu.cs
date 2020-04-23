using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{

    public PlayMenu playMenu;
    public GameObject creditsMenu;

    public void Back()
    {
        playMenu.PlayMenuUI.SetActive(true);
        creditsMenu.SetActive(false);
    }
}
