using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    public int startingHealth;

    public int CurrentHealth { get; set; }

     public event Action deathEvent;
     public static event Action deathStaticEvent;

    private void Awake()
    {
        CurrentHealth = startingHealth;
    }

    public void ChangeHealth(int amount)
    {
        CurrentHealth += amount;
        CheckForDeath();
    }

    public void CheckForDeath()
    {
        if (CurrentHealth <= 0 && deathEvent != null)
        {
            deathEvent.Invoke();
            deathStaticEvent.Invoke();
        }
    }
}