using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class NodeClasswork : MonoBehaviour
    {
        public int gCost; //path take so far
        public int hCost; //distance to end
        public int fCost; // total cost (gCost+hCost)
        public GameObject previousNode;
        public Vector3 nodePos;
        public List<NodeClasswork> neighbours = new List<NodeClasswork>();
        public bool isWalkable;
        private void Awake()
        {
            nodePos = transform.position;
        }
    }
}
