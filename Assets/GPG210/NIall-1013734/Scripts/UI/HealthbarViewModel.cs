using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarViewModel : MonoBehaviour
{

    public HealthBar healthbar;
    public Health health;
	public Transform target;
	public Vector3 offset;


	private void Awake()
    {
        healthbar.SetMaxHealth(health.startingHealth);
        health.healthChangedEvent += HealthOnhealthChangedEvent;
    }

	private void Update()
	{
		transform.position = target.position + offset;
	}

	private void HealthOnhealthChangedEvent(Health arg1, int arg2)
    {
         healthbar.SetHealth(health.CurrentHealth);
    }
    

}
