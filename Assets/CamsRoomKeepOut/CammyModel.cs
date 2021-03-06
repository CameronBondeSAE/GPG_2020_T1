﻿using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

public class CammyModel : UnitBase
{
	public Animator animator;
	public Health health;

	private void Start()
	{
		health.deathEvent += HealthOndeathEvent;
	}

	private void HealthOndeathEvent(Health obj)
	{
		animator.SetTrigger("Die");
	}

	private void OnTriggerEnter(Collider other)
	{
		animator.SetTrigger("Something hit me");
	}

	private void OnTriggerExit(Collider other)
	{
		animator.SetTrigger("Something left my trigger");
	}
}
