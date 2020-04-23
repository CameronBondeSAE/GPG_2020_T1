using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Blaide_Fedorowytsch.Scripts.PathFinding;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Abilities
{
    public class MoveWithPathFinding  : AbilityBase
    {
        private Rigidbody rb;
        private Vector3 target;
        public bool moving = false;
        private float heightOffset = 0.5f;
        private UnitBase ub;
        [HideInInspector]
        public PathFinding.SimplePathfinder simplePathfinder;
        [HideInInspector]
        public ProceduralMeshGenerator procMesh;
        public LayerMask ground;
        public List<Node> currentPath;
        public int currentPathNodeIndex;
        public float nodeDistanceMin = 1.5f;
        public float moveForce = 1000;
        public override bool SelectedExecute()
        {
            return true;
        }

        private void Awake()
        {
            abilityName = "Move PF";
            abilityDescription = "moves units, hopefully";
            rb = GetComponent<Rigidbody>();
            ub = GetComponent<UnitBase>();
            simplePathfinder = FindObjectOfType<SimplePathfinder>();
            procMesh = FindObjectOfType<ProceduralMeshGenerator>();
            targetRequired = true;
        }

        public override bool TargetExecute(Vector3 worldPos)
        {
            heightOffset = GetComponent<Collider>().bounds.extents.y;
            target = worldPos;
            moving = false;
           // currentPath = simplePathfinder.FindPath(transform.position, target);
            simplePathfinder.RequestPathFind(transform.position, target, SetPath);
            currentPathNodeIndex = 0;
            return true;
        }

        private void SetPath(List<Node> list)
        {
            currentPath = list;
            moving = true;
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                if (Vector3.Distance(this.gameObject.transform.position,target) > nodeDistanceMin)
                {
                    Vector3 nextPos = currentPath[currentPathNodeIndex].worldPosition;
                  //  nextPos = new Vector3(nextPos.x, procMesh.GetHeightAtPosition(new Vector2(nextPos.x, nextPos.z)) + 1, nextPos.z);
                    nextPos = new Vector3(nextPos.x,transform.position.y, nextPos.z);
                    if (Vector3.Distance(this.gameObject.transform.position,
                            nextPos) > nodeDistanceMin)
                    {
                        Move(nextPos);
                    }
                    else
                    {
                        if(currentPathNodeIndex < currentPath.Count -1)
                        currentPathNodeIndex += 1;
                    }
                }
                else
                {
                    moving = false;
                }
            }
        }

        void Move(Vector3 v)
        {
            //this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
            Ray ray = new Ray(transform.position,-transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 3f, ground, QueryTriggerInteraction.Ignore))
            {
                rb.AddForce(Vector3.ProjectOnPlane((v -transform.position),hit.normal) * moveForce);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (currentPath != null)
            {
                if (currentPath.Count != 0)
                {
                 Gizmos.DrawSphere(currentPath[currentPathNodeIndex].worldPosition,1);   
                }
            }
        }
    }
    
}
