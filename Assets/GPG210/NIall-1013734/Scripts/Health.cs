using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	public float health = 50.0f;

    public int startingHealth;

	[ProgressBar("Health", 100, EColor.Red)]
	private int currentHealth;
	
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

    public void CheckForDeath()
    {
        if (CurrentHealth <= 0 && deathEvent != null)
        {
            deathEvent.Invoke(this);
            if (deathStaticEvent != null) deathStaticEvent.Invoke(this);
        }
    }
}