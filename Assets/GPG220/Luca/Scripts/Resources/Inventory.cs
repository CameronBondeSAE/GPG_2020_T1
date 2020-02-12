using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.Events;

namespace GPG220.Luca.Scripts.Resources
{
    
    /// <summary>
    /// This class serves to store resources. (Possible use: player, individual unit, treasure chests, farmable resources, ...)
    /// </summary>
    public class Inventory : SerializedMonoBehaviour
    {
        #region Events

        public delegate void OnResQuantityChangeDel(Inventory inventory, Resource resource, int amtChange);

        public event OnResQuantityChangeDel onResQuantityChanged;

        #endregion
        
        
        /// <summary>
        /// Stores the amounts of each resource in this inventory. If a resource isn't present in the dictionary
        /// it implies that the amount is 0.
        /// </summary>
        [ShowInInspector, OdinSerialize]
        private Dictionary<Resource, int> _resources;
        
        /// <summary>
        /// Resets <paramref name="resourcesMax"/> and fills it with all active resources. Default value is -1 (Unlimited amounts).
        /// </summary>
        [Button("Init Resources Max"), PropertyTooltip("Loads all resources into the dictionary. Deletes old values."), FoldoutGroup("Resource Maximums")]
        private void InitializeResourcesMax()
        {
            resourcesMax = new Dictionary<Resource, int>();
            
            Resource.GetAllResources().ForEach(res =>
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
        public Dictionary<Resource, int> resourcesMax;
        

        private void Awake()
        {
            if(resourcesMax == null)
                resourcesMax = new Dictionary<Resource, int>();
            
            if(_resources == null)
                _resources = new Dictionary<Resource, int>();
        }


        /// <summary>
        /// Tries to remove a given amount of resources from the inventory.
        /// </summary>
        /// <param name="resource">Defines the type of resource you'd like to remove.</param>
        /// <param name="amount">The amount you would like to remove.</param>
        /// <param name="requireFullAmount">If set to true, resources will only be taken if the desired amount is
        /// available in the inventory.</param>
        /// <returns>Returns the amount of resources that have been taken. If you try to remove more resources than
        /// there are and <paramref name="requireFullAmount"/> is set to false, the method will return the available amount that has been deducted.</returns>
        public int RemoveResources(Resource resource, int amount, bool requireFullAmount = true)
        {
            var amt = 0;

            if (!_resources.ContainsKey(resource) || (requireFullAmount && _resources[resource] < amount))
                return amt;

            amt = _resources[resource] >= amount ? amount : _resources[resource];
            
            _resources[resource] -= amt;
            
            if(amt != 0)
                onResQuantityChanged?.Invoke(this,resource,-amt);
            
            return amt;
        }

        /// <summary>
        /// Tries to add a given amount of resources to the inventory.
        /// </summary>
        /// <param name="resource">The type of resources you're adding.</param>
        /// <param name="amount">The amount of the resource you're adding.'</param>
        /// <param name="requireFullAmount">If set to true, the resources will only be added if there is space
        /// for the full amount of the given resources.</param>
        /// <returns>Returns the final amount of resources that have been added. Note that this number could be lower than the given amount.</returns>
        public int AddResources(Resource resource, int amount, bool requireFullAmount = true)
        {
            var amt = 0;

            // Calculate available quantity
            var resMax = resourcesMax.ContainsKey(resource) ? resourcesMax[resource] : -1;
            _resources.TryGetValue(resource, out var storedQty);
            var availableQty = resMax - storedQty;
            availableQty = resMax == -1 ? -1 : (availableQty > 0 ? availableQty : 0);

            // Check if the given amount can be added
            if (availableQty != -1 && requireFullAmount && amount > availableQty)
                return 0;

            // Calculate the fraction of the given amount that can be added
            amt = (availableQty == -1 ? amount : availableQty);
            
            // Add resources
            if (!_resources.ContainsKey(resource))
                _resources.Add(resource, amt);
            else
                _resources[resource] += amt;
            
            if(amt != 0)
                onResQuantityChanged?.Invoke(this,resource,amt);
            
            return amt;
        }

        /// <summary>
        /// <para>Returns the current quantity of the given resource.</para>
        /// <para>Doesn't remove nor add any resources from the inventory.</para>
        /// </summary>
        /// <param name="resource">The resource of which you want to find out its quantity.</param>
        public int GetResourceQuantity(Resource resource)
        {
            return _resources.ContainsKey(resource) ? _resources[resource] : 0;
        }

        /// <summary>
        /// <para>Returns a dictionary containing information about the quantities of each resource present in this inventory.</para>
        /// <para>This method doesn't remove nor add any resources from the inventory. The returned dictionary is a copy of the original.</para>
        /// </summary>
        /// <returns></returns>
        public Dictionary<Resource, int> GetResourceQuantities()
        {
            
            return new Dictionary<Resource, int>(_resources);
        }
        
    }
}