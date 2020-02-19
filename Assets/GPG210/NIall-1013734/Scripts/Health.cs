﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class Health : MonoBehaviour
    {

        public int startingHealth;

        [SerializeField] public int currentHealth;

        UnityEvent deathEvent = new UnityEvent();

        public void Start()
        {
            deathEvent.AddListener(CheckForDeath);
        }

        private void Awake()
        {
            currentHealth = startingHealth;
        }

        public void ChangeHealth(int amount)
        {
            currentHealth += amount;
        }

        private void CheckForDeath()
        {
            if (currentHealth <= 0 && deathEvent != null)
            {
                deathEvent.Invoke();
            }

        }
    }
