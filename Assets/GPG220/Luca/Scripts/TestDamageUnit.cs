using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class TestDamageUnit : MonoBehaviour
{
    [ReadOnly]
    public int currentHealth;
    private Health _health;
    public int damage = 10;
    
    [Button]
    public void DamageUnit()
    {
        _health.ChangeHealth(-damage);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = _health.CurrentHealth;
    }
}
