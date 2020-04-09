using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.PathFinding
{
    public class NodeGrid : MonoBehaviour
    {
        public Vector2 gridWorldSize;
        public float nodeRadius;
        private Node[,] grid;
        public LayerMask layerMask;
        

        private float nodeDiamater;

        private Vector2Int gridSize;
        
        public List<Node> path;
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    Gizmos.DrawCube(n.worldPosition,Vector3.one * (nodeDiamater-0.1f));
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = Color.green;
                        }
                    }
                }
            }
            
        }

        private void Start()
        {
            nodeDiamater = nodeRadius * 2;
            gridSize = new Vector2Int(Mathf.RoundToInt(gridWorldSize.x / nodeDiamater),Mathf.RoundToInt(gridWorldSize.y /nodeDiamater));
            CreatGrid();
        }

       public void UpdateGrid()
        {
            foreach (Node node in grid)
            {
                node.walkable = !Physics.CheckSphere(node.worldPosition, nodeRadius,layerMask);
            }
        }

        private void FixedUpdate()
        {
            UpdateGrid();
        }

        void CreatGrid()
        {
            grid = new Node[gridSize.x,gridSize.y];
            Vector3 worldBottomLeft = transform.position - Vector3.right *( gridWorldSize.x/2) -Vector3.forward * (gridWorldSize.y/2);
            
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiamater + nodeRadius) +
                                         Vector3.forward * (y * nodeDiamater + nodeRadius);
                    bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius,layerMask);

                    grid[x, y] = new Node(walkable, worldPoint,x,y);
                }  
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = node.gridPosition.x + x;
                    int checkY = node.gridPosition.y + y;


                    if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                    {
                        neighbours.Add(grid[checkX,checkY]);
                    }
                }
            }

            return neighbours;
        }

        public Node NodeFromWorldPoint(Vector3 worldPoint)
        {
            float percentX = (worldPoint.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPoint.z + gridWorldSize.y / 2) / gridWorldSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            
            int x = Mathf.RoundToInt((gridSize.x -1) * percentX);
            int y = Mathf.RoundToInt((gridSize.y -1) * percentY);
            
            return grid[x, y];
        }
    }
}
