using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Pathfinding
{
    public class AStarPathfindingClasswork : MonoBehaviour
    {
        [SerializeField] private List<NodeClasswork> OpenList = new List<NodeClasswork>();
        private List<NodeClasswork> CloseList = new List<NodeClasswork>();
        
        private NodeClasswork currentNode;
        public NodeClasswork startNode;
        public NodeClasswork targetNode;

        public NodeClasswork[,] grid;

        public GameObject tilePrefab;
        
        
        private void Awake()
        {
            currentNode = GetComponent<NodeClasswork>();
            CreateGrid(2,2);
        }

        private void CreateGrid(int x, int y)
        {
            // Vector2 size = grid[x, y];
            
            
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    
                }
            }
        }

        private void Update()
        {
            // for (int i = 0; i < UPPER ; i++)
            // {
            //     //add in all nodes in grid
            // }


            if (currentNode.fCost == targetNode.fCost)
            {
                //path found
                return;
            }

            foreach (NodeClasswork neighbour in currentNode.neighbours)
            {
                // if (neighbour.isWalkable == false || neighbour == CloseList.Find(neighbour))
                {
                    
                }
                
                // if()
            }
            
            
            
            
            
        }
    }
}
