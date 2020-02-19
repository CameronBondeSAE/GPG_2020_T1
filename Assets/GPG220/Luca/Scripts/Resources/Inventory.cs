
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Luca.Scripts.Resources
{
    
    /// <summary>
    /// This class serves to store resources. (Possible use: player, individual unit, treasure chests, farmable resources, ...)
    /// </summary>
    public class Inventory : SerializedMonoBehaviour
    {
        #region Events

        public delegate void OnResQuantityChangeDel(Inventory inventory, ResourceType resourceType, int amtChange);

        public event OnResQuantityChangeDel onResQuantityChanged;

        #endregion

        public float dropOutAcceleration = 4f;
        [Range(0,90)]
        public float dropOutAngle = 30;
        public int dropOutMaxStack = 5;
        public float timeBetweenDropOuts = 1;
        
        /// <summary>
        /// Stores the amounts of each resource in this inventory. If a resource isn't present in the dictionary
        /// it implies that the amount is 0.
        /// </summary>
        [ShowInInspector, OdinSerialize]
        private Dictionary<ResourceType, int> _resources;
        
        /// <summary>
        /// Resets <paramref name="resourcesMax"/> and fills it with all active resources. Default value is -1 (Unlimited amounts).
        /// </summary>
        [Button("Init Resources Max"), PropertyTooltip("Loads all resources into the dictionary. Deletes old values."), FoldoutGroup("Resource Maximums")]
        private void InitializeResourcesMax()
        {
            resourcesMax = new Dictionary<ResourceType, int>();
            
            ResourceType.GetAllResources().ForEach(res =>
            {
                resourcesMax.Add(res, -1);
            });
        }
        
        /// <summary>
        /// Defines maximum amount for specific resources. If a resource isn't present in the dictionary OR the stored value is -1 it implies
        /// that the maximum for this resources is infinite. 0 will block the inventory to store any of the given kind.
        /// </summary>
        [InfoBox("Here you can define maximum allowed quantities of different resources for this specific inventory.")]
        [ShowInInspector, OdinSerialize,FoldoutGroup("Resource Maximums")]
        public Dictionary<ResourceType, int> resourcesMax;
        

        private void Awake()
        {
            if(resourcesMax == null)
                resourcesMax = new Dictionary<ResourceType, int>();
            
            if(_resources == null)
                _resources = new Dictionary<ResourceType, int>();

            var health = GetComponent<Health>();
            // TODO Register to onDeath event
            /*if(health != null)
                health.deathEvent*/
        }

        [Button("Drop All Items")]
        public void DropAllItems()
        {
            StartCoroutine(DropItems());
        }

        private IEnumerator DropItems()
        {
            foreach (var resType in GetResourceQuantities())
            {
                var startPos = transform.position;
                startPos.y += 1.5f;

                var stacks = dropOutMaxStack <= 0 ? 1 : Mathf.CeilToInt((float)resType.Value / dropOutMaxStack);
                for (var i = 0; i < stacks; i++)
                {
                    var go = resType.Key.ResourcePrefab == null ? GameObject.CreatePrimitive(PrimitiveType.Cube) : Instantiate(resType.Key.ResourcePrefab);
                    go.transform.position = startPos;
                    
                    var amtToTake = RemoveResources(resType.Key, dropOutMaxStack, false);
                    var rpu = go.GetComponent<ResourcePickUp>() ?? go.AddComponent<ResourcePickUp>();
                    var rpuInventory = rpu?.inventory ?? rpu?.GetComponent<Inventory>();
                    var amtTaken = rpuInventory?.AddResources(resType.Key, amtToTake) ?? 0;

                    if (amtTaken < amtToTake)
                        AddResources(resType.Key, amtToTake - amtTaken);

                    var rb = go.GetComponent<Rigidbody>();
                    var dropOutForce = Quaternion.Euler(Random.Range(-dropOutAngle,dropOutAngle),0,Random.Range(-dropOutAngle,dropOutAngle)) * (dropOutAcceleration * rb.mass * Vector3.up);
                    rb.AddForce(dropOutForce, ForceMode.Impulse);
                    
                    yield return new WaitForSeconds(timeBetweenDropOuts);
                }

            }

            yield return 0;
        }


        /// <summary>
        /// Tries to remove a given amount of resources from the inventory.
        /// </summary>
        /// <param name="resourceType">Defines the type of resource you'd like to remove.</param>
        /// <param name="amount">The amount you would like to remove.</param>
        /// <param name="requireFullAmount">If set to true, resources will only be taken if the desired amount is
        /// available in the inventory.</param>
        /// <returns>Returns the amount of resources that have been taken. If you try to remove more resources than
        /// there are and <paramref name="requireFullAmount"/> is set to false, the method will return the available amount that has been deducted.</returns>
        public int RemoveResources(ResourceType resourceType, int amount, bool requireFullAmount = true)
        {
            var amt = 0;

            if (!_resources.ContainsKey(resourceType) || (requireFullAmount && _resources[resourceType] < amount))
                return amt;

            amt = _resources[resourceType] >= amount ? amount : _resources[resourceType];
            
            _resources[resourceType] -= amt;
            
            if(amt != 0)
                onResQuantityChanged?.Invoke(this,resourceType,-amt);
            
            return amt;
        }

        /// <summary>
        /// Tries to add a given amount of resources to the inventory.
        /// </summary>
        /// <param name="resourceType">The type of resources you're adding.</param>
        /// <param name="amount">The amount of the resource you're adding.'</param>
        /// <param name="requireFullAmount">If set to true, the resources will only be added if there is space
        /// for the full amount of the given resources.</param>
        /// <returns>Returns the final amount of resources that have been added. Note that this number could be lower than the given amount.</returns>
        public int AddResources(ResourceType resourceType, int amount, bool requireFullAmount = true)
        {
            var amt = 0;

            // Calculate available quantity
            var resMax = resourcesMax.ContainsKey(resourceType) ? resourcesMax[resourceType] : -1;
            _resources.TryGetValue(resourceType, out var storedQty);
            var availableQty = resMax - storedQty;
            availableQty = resMax == -1 ? -1 : (availableQty > 0 ? availableQty : 0);

            // Check if the given amount can be added
            if (availableQty != -1 && requireFullAmount && amount > availableQty)
                return 0;

            // Calculate the fraction of the given amount that can be added
            amt = (availableQty == -1 ? amount : availableQty);
            
            // Add resources
            if (!_resources.ContainsKey(resourceType))
                _resources.Add(resourceType, amt);
            else
                _resources[resourceType] += amt;
            
            if(amt != 0)
                onResQuantityChanged?.Invoke(this,resourceType,amt);
            
            return amt;
        }

        public bool IsEmpty()
        {
            return _resources.Sum(res => res.Value) == 0;
        }

        /// <summary>
        /// <para>Returns the current quantity of the given resource.</para>
        /// <para>Doesn't remove nor add any resources from the inventory.</para>
        /// </summary>
        /// <param name="resourceType">The resource of which you want to find out its quantity.</param>
        public int GetResourceQuantity(ResourceType resourceType)
        {
            return _resources.ContainsKey(resourceType) ? _resources[resourceType] : 0;
        }

        /// <summary>
        /// <para>Returns a dictionary containing information about the quantities of each resource present in this inventory.</para>
        /// <para>This method doesn't remove nor add any resources from the inventory. The returned dictionary is a copy of the original.</para>
        /// </summary>
        /// <returns></returns>
        public Dictionary<ResourceType, int> GetResourceQuantities()
        {
            
            return new Dictionary<ResourceType, int>(_resources);
        }
        
    }
}