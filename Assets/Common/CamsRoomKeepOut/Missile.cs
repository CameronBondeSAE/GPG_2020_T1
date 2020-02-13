using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public int damage = 10;

    private void OnCollisionEnter(Collision other)
    {
        // Does the other object even have a Health component?
        if (other.gameObject.GetComponent<Health>() != null)
        {
            // Do damage
            other.gameObject.GetComponent<Health>().ChangeHealth(-damage);
        }
    }
}
