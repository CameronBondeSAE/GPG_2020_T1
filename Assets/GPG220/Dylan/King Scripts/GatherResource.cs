using System;
using GPG220.Luca.Scripts.Resources;
using UnityEngine;

namespace GPG220.Dylan.King_Scripts
{
    public class GatherResource : MonoBehaviour
    {
        public Inventory inventory;

        public void Start()
        {
            inventory = GetComponent<Inventory>();
        }
        
        
    }
}
