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
    public class UnitTest : UnitBase
    {
        private void Start()
        {
            Initialize();
            usm = FindObjectOfType<UnitSelectionManager>();
           // health.deathEvent += Die(health);
        }

        public bool moving = false;
        public Vector3 target;
        public List<ISelectable> selctionGroup;
        public float HeightOffset;
        public UnitSelectionManager usm;

        public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
        {
            abilityController.ExecuteDefaultAbility();
        }
        
        void Update()
        {

            if (isServer)
            {
                RpcMove(transform.position);
            }
        }
        

        void Die( Health h)
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
