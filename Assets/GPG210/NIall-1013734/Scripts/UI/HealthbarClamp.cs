using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarClamp : MonoBehaviour
{

    public Slider healthbar;
    void Update()
    {
        Vector3 barPos = Camera.main.WorldToScreenPoint(this.transform.position);
        healthbar.transform.position = barPos;
    }
}
