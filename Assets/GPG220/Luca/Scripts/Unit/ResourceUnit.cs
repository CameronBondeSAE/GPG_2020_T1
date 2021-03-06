﻿using System;
using System.Collections.Generic;
using System.Linq;
using GPG220.Luca.Scripts.Resources;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace GPG220.Luca.Scripts.Unit
{
    public class ResourceUnit : UnitBase
    {
        [NonSerialized] private Dictionary<ResourceType, int> _originalAmounts;

        public GameObject currentModel;

        // Hacky
        public struct VisualHealthState
        {
            [Range(0, 100)] public int fromPercentage;
            [Range(0, 100)] public int toPercentage;

            [PreviewField] public GameObject model;
        }

        public List<VisualHealthState> visualHealthStates;
        private VisualHealthState currentVisualHealthState;
        private float oldHealth = 0;

        [FoldoutGroup("Loot Options")] public bool
            lootOnDestroy =
                false; // Loot will be given out when it's fully destroyed otherwise gradually depending on damage.

        [FoldoutGroup("ResetOptions")] public float
            despawnTime = -1; // -1: Don't despawn, 0: Despawn immediately, >0: Despawn after x seconds (after dying)

        [FoldoutGroup("ResetOptions")] public float
            respawnTime = -1; // -1: Don't respawn, 0: Respawn immediately, >0: Respawn after x seconds (after dying)

        private float despawnCountdown = -1;
        private float respawnCountdown = -1;

        [FoldoutGroup("Hit Simulation")] public float damage = -999;

        [FoldoutGroup("Hit Simulation")] public UnitBase otherUnit;

        public ResourceType resourceType;
        public Inventory inventory;

        [FoldoutGroup("Hit Simulation"), Button("Simulate Hit")]
        public void SimulateHit()
        {
            if (otherUnit == null)
                return;

            unitStats.Health -= damage;

            HandleOnHit(otherUnit);
        }

        private void Start()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            inventory = GetComponent<Inventory>();
            if (visualHealthStates == null)
                visualHealthStates = new List<VisualHealthState>();
            //visualHealthStates = new Dictionary<KeyValuePair<int,int>, GameObject>();

            _originalAmounts = inventory.GetResourceQuantities();
            if (unitStats != null)
            {
                oldHealth = unitStats.Health;
                unitStats.onHealthChanged += HandleOnHealthChanged;
            }

            inventory.ResQuantityChangedEvent += CheckInventory;
            base.Initialize();
        }

        private void CheckInventory(Inventory _inventory, ResourceType _resourceType, int amount)
        {
            Debug.Log("Working");
            _inventory = inventory;
            _resourceType = resourceType;
            if (_inventory.GetResourceQuantity(_resourceType) <= 0)
            {
                health.deathEvent += Death;
                health.RpcInstaKill();
            }
        }

        private void Death(Health obj)
        {
			if (!isServer)
			{
				return;
			}
            health.deathEvent -= Death;
            Destroy(gameObject,0.1f);
        }

        private void OnDestroy()
        {
			if (!isServer)
			{
				return;
			}
            if (unitStats != null) unitStats.onHealthChanged -= HandleOnHealthChanged;
        }

        private void Update()
        {
            if (despawnTime > 0 && despawnCountdown > 0)
            {
                despawnCountdown -= Time.deltaTime;
                if (despawnCountdown <= 0)
                    Despawn();
            }

            if (respawnTime > 0 && respawnCountdown > 0)
            {
                respawnCountdown -= Time.deltaTime;
                if (respawnCountdown <= 0)
                    Respawn();
            }
        }

        private void HandleOnHealthChanged(UnitStats arg0, float oldHealth, float newHealth)
        {
            this.oldHealth = oldHealth;
            UpdateVisualHealthState();
        }

        // TODO Receive event from a damage handler
        private void HandleOnHit(UnitBase otherUnit)
        {
			if (!isServer)
			{
				return;
			}
			
            if (lootOnDestroy && unitStats.IsAlive())
                return;

            // Calculate the amount of resources to give to the unit
            var healthPercentChange = GetHealthPercentage(Mathf.Abs(oldHealth - unitStats.Health)) / 100f;

            var otherUnitInventory = otherUnit.inventory;

            if (otherUnitInventory == null)
                return;

            _originalAmounts.ForEach(kvp =>
            {
                // Calculate the amounts of resources to be transferred. Give all resources if the resource is "Dead". HACKY.
                var amount = unitStats.IsAlive()
                    ? ((int) (kvp.Value * healthPercentChange))
                    : inventory.GetResourceQuantity(kvp.Key);

                if (amount <= 0)
                    return;

                // Remove resource from this ResourceUnit
                amount = inventory.RemoveResources(kvp.Key, amount);

                // Try and add resources to the otherUnits' inventory
                var transferredAmount = otherUnitInventory.AddResources(kvp.Key, amount);

                // If the other unit couldn't pick up all resources they will be added back. TODO Delete these resources?
                if (transferredAmount < amount)
                    inventory.AddResources(kvp.Key, (amount - transferredAmount));
            });
        }

        void UpdateVisualHealthState()
        {
            var currentHealthPercentage = GetHealthPercentage(unitStats.Health);
            var vhs = visualHealthStates
                .FirstOrDefault(state =>
                    state.fromPercentage <= currentHealthPercentage && state.toPercentage >= currentHealthPercentage);

            if (vhs.Equals(currentVisualHealthState))
                return;

            currentVisualHealthState = vhs;

            if (vhs.model == null)
                return;

            // TODO: Use pooling? Or just (de-)activate models & always keep them alive?
            Destroy(currentModel);

            currentModel = Instantiate(vhs.model, transform);
        }

        public int GetHealthPercentage(float health)
        {
            if (unitStats.Health < 0)
                return 0;

            return (int) (health / unitStats.MaxHealth * 100);
        }

        public void Respawn()
        {
            // TODO
            // RESET INVENTORY
            // RESET HEALTH
            // RESET VisualHealthState
        }

        public void Despawn()
        {
            // TODO: Use pooling? Or just (de-)activate models & always keep them alive?
            Destroy(currentModel);
        }
    }
}