using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Health : MonoBehaviour
    {

        public int startingHealth;
        public int currentHealth;


        public void Awake()
        {
            currentHealth = startingHealth;
            
        }
    }
