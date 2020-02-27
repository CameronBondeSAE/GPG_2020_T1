using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    public TextMeshPro popupText;
    public GameObject popUpBox;
    

    public void PopUp(string text)
    {
        popUpBox.SetActive(true);
        popupText.text = text;
    }
}
