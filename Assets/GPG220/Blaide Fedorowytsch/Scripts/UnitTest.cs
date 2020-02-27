﻿using System;
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
        }

        public bool moving = false;
        public Vector3 target;
        public List<ISelectable> selctionGroup;
        public float HeightOffset;
        public UnitSelectionManager usm;
        
        public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
        {
            moving = true;
            target = worldPosition;
        }

        void Update()
        {

            if (isServer)
            {
                RpcMove(transform.position);
            }
            
            
        }

        [ClientRpc]
        public void RpcMove(Vector3 pos)
        {
            
            
            transform.position = pos;
        }

        public override void OnSelected()
        {
            selctionGroup = usm.selectedIselectables;
        }


        void Move(Vector3 v)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
        }

        Vector3 OffsetPosition()
        {
            Vector3 total = new Vector3();

            foreach (ISelectable s in selctionGroup)
            {
                total += ((MonoBehaviour) s).gameObject.transform.position;
            }

            return transform.position - total / selctionGroup.Count;
        }


        void FixedUpdate()
        {
            if (moving)
            {
                if (Vector3.Distance(this.gameObject.transform.position, target  + (Vector3.up *HeightOffset)  + OffsetPosition()) > 0.1f)
                {
                    Move(target + (Vector3.up *HeightOffset) + OffsetPosition());
                }
                else
                {
                    moving = false;
                }
            }
        }

    }
}
