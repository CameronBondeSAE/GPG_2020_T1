using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class HealthbarViewModel : MonoBehaviour
{

    public HealthBar healthbar;
    public Health health;
	
	[SerializeField]
	[ReadOnly]
	private Transform target;

	public Vector3 offset;
	

	public void SetTarget(Transform _target)
	{
		target = _target;
		health = target.GetComponent<Health>();
		if (health != null)
		{
			healthbar.SetMaxHealth(health.startingHealth);
			health.healthChangedEvent += HealthOnhealthChangedEvent;
		}
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
