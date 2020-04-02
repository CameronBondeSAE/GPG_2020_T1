using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnthonyStates : MonoBehaviour
{
    public Animator animator;

    public Health health;
    // Start is called before the first frame update
    void Start()
    {
        health.deathEvent += HealthOndeathEvent;
    }

    private void HealthOndeathEvent(Health obj)
    {
        animator.SetTrigger("Player Death");
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        animator.SetTrigger("Move player");
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetTrigger("I escaped");
    }
}
