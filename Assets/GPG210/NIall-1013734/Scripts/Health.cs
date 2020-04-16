using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int startingHealth;
   [SerializeField] public int CurrentHealth { get; set; }

     public event Action<Health> deathEvent;
     public static event Action<Health> deathStaticEvent;
     public event Action<Health, int> healthChangedEvent;


    private void Awake()
    {
        CurrentHealth = startingHealth;
        
    }

    public void ChangeHealth(int amount)
    {
        CurrentHealth += amount;
        healthChangedEvent?.Invoke(this, amount);
        CheckForDeath();
    }

    public void CheckForDeath()
    {
        if (CurrentHealth <= 0 && deathEvent != null)
        {
            deathEvent.Invoke(this);
            if (deathStaticEvent != null) deathStaticEvent.Invoke(this);
        }
    }
}