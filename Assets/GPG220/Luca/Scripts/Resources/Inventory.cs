using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Luca.Scripts.Resources
{
    public enum ResourceType {Wood, Gold}
    public class Inventory : MonoBehaviour
    {
        public Dictionary<ResourceType, int> resourcesMax;
        private Dictionary<ResourceType, int> resources;
        

        public int RemoveResources(ResourceType resType, int amount)
        {
            var amt = 0;

            if (!resources.ContainsKey(resType))
                return amt;

            if (resources[resType] >= amount)
            {
                amt = amount;
            }
            else
            {
                amt = resources[resType];
            }
            
            resources[resType] -= amt;
            return amt;
        }

        public int AddResources()
        {
            //TODO
            return 0;
        }
    }
}