using System;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using UnityEngine;
using ISelectable = GPG220.Blaide_Fedorowytsch.Scripts.Interfaces.ISelectable;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    /// <summary>
    /// A basic test unit, action lerps to the returned world position.
    /// 
    /// </summary>
    public class TestUnit : UnitBase
    {
        private void Start()
        {
            Initialize();
            health.deathEvent += Die;
        }

        void Update()
        {
            if (isServer)
            {
                RpcMove(transform.position);
            }
        }

        public void Die(Health h)
        {
            Destroy(gameObject);
        }
        
        [ClientRpc]
        public void RpcMove(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}
