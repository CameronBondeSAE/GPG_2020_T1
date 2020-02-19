using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class Node
    {
        
        private float _gCost = 0f; // Dist to start
        public float GCost
        {
            get => _gCost;
            set
            {
                _gCost = value;
                fCost = GCost + HCost;
            }
        }

        private float _hCost = 0f; // Dist to end
        public float HCost
        {
            get => _hCost;
            set
            {
                _hCost = value;
                fCost = HCost + GCost;
            }
        }

        public float fCost = -1; // gCost + hCost

        public List<Node> neighbourNodes = new List<Node>();

        public Node lastNode;
        public Vector3 position;

        public GameObject debugGO;
        public Renderer debugGORenderer;

        public Node()
        {
            
        }
    }
}