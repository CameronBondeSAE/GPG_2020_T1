using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	public int startingHealth;

	[SerializeField]
	private int currentHealth = 100;
	
	public int CurrentHealth
	{
		get => currentHealth;
		set => currentHealth = value;
	}

	public event Action<Health> deathEvent;
    public static event Action<Health> deathStaticEvent;

	// Note: I'm
	public delegate void HealthChangedDelegate(Health _health, int _amountChanged);
	public event HealthChangedDelegate healthChangedEvent;


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

	public void InstaKill()
	{
		deathEvent?.Invoke(this);
		deathStaticEvent?.Invoke(this);
	}

    public void CheckForDeath()
    {
        if (CurrentHealth <= 0)
        {
            deathEvent?.Invoke(this);
            deathStaticEvent?.Invoke(this);
        }
    }
}