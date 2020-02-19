using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Resources;
using GPG220.Luca.Scripts.Unit;
using Sirenix.Utilities;
using UnityEngine;

[RequireComponent(typeof(Inventory),typeof(SphereCollider), typeof(Rigidbody))]
public class ResourcePickUp : MonoBehaviour
{
    public Inventory inventory;

    public float despawnTime = -1f;
    public float despawnCountdown = -1f;
    
    public float pickupCooldown = 1f;
    private float remainingPickupCooldown = 0f;
    
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        var bc = GetComponent<SphereCollider>();
        if(bc != null)
            bc.isTrigger = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        remainingPickupCooldown = pickupCooldown;
        if (despawnTime > 0)
            despawnCountdown = despawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingPickupCooldown > 0)
                remainingPickupCooldown -= Time.deltaTime;

        if (despawnCountdown > 0)
        {
            despawnCountdown -= Time.deltaTime;
            if(despawnCountdown <= 0)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        HandleCollision(other.collider.gameObject);
    }

    private void HandleCollision(GameObject colliderObj)
    {
        if (remainingPickupCooldown > 0)
            return;

        var unit = colliderObj.GetComponent<UnitBase>();
        var otherInventory = colliderObj.GetComponent<Inventory>();
        if (otherInventory == null || unit == null) return;
        remainingPickupCooldown = pickupCooldown;
        otherInventory.GetResourceQuantities().ForEach(resType =>
        {
            var amtToTake = otherInventory.RemoveResources(resType.Key, resType.Value);
            var amtTaken = inventory.AddResources(resType.Key, amtToTake);

            if (amtTaken != amtToTake)
                otherInventory.AddResources(resType.Key, amtToTake-amtTaken);
        });
    }
}
