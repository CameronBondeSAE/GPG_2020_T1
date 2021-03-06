﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Health : NetworkBehaviour
{	
	[SyncVar]
	public int startingHealth;
	
	
	[SerializeField]
	[SyncVar]
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
		if (isServer)
		{
			RpcChangeHealth(amount);
		}
    }

    
    [ClientRpc]
    public void RpcChangeHealth(int amount)
    {
	    CurrentHealth += amount;
	    healthChangedEvent?.Invoke(this, amount);
	    CheckForDeath();
    }
	
	public void CheckForDeath()
	{
		if (CurrentHealth <= 0)
		{
			deathEvent?.Invoke(this);
			deathStaticEvent?.Invoke(this);
		}
	}
	
	public void InstaKill()
	{
		if (isServer)
		{
			RpcInstaKill();
		}
	}
	
	[ClientRpc]
	public void RpcInstaKill()
	{
		deathEvent?.Invoke(this);
		deathStaticEvent?.Invoke(this);
	}

	// If the whole GO gets Unity Destroyed, fire off death (for things like healthbars listening)
	private void OnDestroy()
	{
		deathEvent?.Invoke(this);
		deathStaticEvent?.Invoke(this);
	}
}