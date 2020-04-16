using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Experimental.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxHealth(int currenthealth)
    {
        slider.maxValue = currenthealth;
        slider.value = currenthealth;
    }

    public void SetHealth(int currenthealth)
    {
        slider.value = currenthealth;
    }

}
