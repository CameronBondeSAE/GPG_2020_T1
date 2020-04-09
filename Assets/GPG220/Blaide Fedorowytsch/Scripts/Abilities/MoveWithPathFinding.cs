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
        public PathFinding.SimplePathfinder simplePathfinder;
        public ProceduralMeshGenerator procMesh;
        public List<Node> currentPath;
        public int currentPathNodeIndex;
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
        }

        public override bool TargetExecute(Vector3 worldPos)
        {
            heightOffset = GetComponent<Collider>().bounds.extents.y;
            target = worldPos;
            moving = true;
            currentPath = simplePathfinder.FindPath(transform.position, target);
            currentPathNodeIndex = 0;
            return true;
        }

        private void FixedUpdate()
        {
            if (moving)
            {
                if (Vector3.Distance(this.gameObject.transform.position,target) < 0.1f)
                {
                    Vector3 nextPos = currentPath[currentPathNodeIndex].worldPosition;
                    nextPos = new Vector3(nextPos.x, procMesh.GetHeightAtPosition(new Vector2(nextPos.x, nextPos.z)), nextPos.z);
                    
                    if (Vector3.Distance(this.gameObject.transform.position,
                            nextPos +
                            OffsetPosition(ub.currentSelectionGroup)) > 0.1f)
                    {
                        Move(nextPos +
                             OffsetPosition(ub.currentSelectionGroup));
                    }
                    else
                    {
                        if(currentPathNodeIndex < currentPath.Count)
                        currentPathNodeIndex += 1;

                    }
                }
                else
                {
                    moving = false;
                }
            }
        }
        Vector3 OffsetPosition(List<ISelectable> selectionGroup)
        {
            Vector3 total = new Vector3();
            foreach (ISelectable s in selectionGroup)
            {
                total += ((MonoBehaviour) s).gameObject.transform.position;
            }
            return transform.position - total / selectionGroup.Count;
        }

        void Move(Vector3 v)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
        }

    }
}
