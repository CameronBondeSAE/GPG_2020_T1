using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class Health : MonoBehaviour
    {

        public int startingHealth;
        public int currentHealth;

        private UnityEvent deathEvent;

        public void Awake()
        {
            currentHealth = startingHealth;
            
        }


        public void Start()
        {
            if (deathEvent == null)
                deathEvent = new UnityEvent();
            
            deathEvent.AddListener(Death);
        }

        private void Death()
        {
            Debug.Log("Death");
        }


        private void Update()
        {
            if (currentHealth <= 0 && deathEvent != null)
            {
                deathEvent.Invoke();
            }

        }
    }
