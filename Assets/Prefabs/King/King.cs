
using System;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

public class King : UnitBase
{
	public AudioSource audioSource;
	public AudioClip clip;

	private void Awake()
	{
		Initialize();

		audioSource = GetComponent<AudioSource>();
		
		GetComponent<Health>().healthChangedEvent += OnhealthChangedEvent;
		GetComponent<Health>().deathEvent += OndeathEvent;
	}

	private void OndeathEvent(Health _health)
	{
		audioSource.pitch = 1f;
		audioSource.PlayOneShot(clip);	
		Destroy(gameObject);
	}

	private void OnhealthChangedEvent(Health _health, int _amountchanged)
	{
		audioSource.pitch = 0.5f;
		audioSource.Play();	
	}
}
