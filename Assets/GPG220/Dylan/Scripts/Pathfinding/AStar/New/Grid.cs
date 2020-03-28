using System;
using System.Collections.Generic;
using Pathfinding.New;
using UnityEngine;

namespace Pathfinding.AStar.New
{
    public class Grid : MonoBehaviour
    {
        [Header("Pathfinding Settings")]
        public bool displayGridGizmos;
        public LayerMask unWalkableMask;
        public Vector2 gridWorldSize;
        public float nodeRadius;
        private Node[,] grid;
        [Range(2, 5)] 
        public int blurPenaltyMultiplier;
        public TerrainType[] walkableRegions;
        private LayerMask walkableMask;
        Dictionary<int,int> walkableRegionsDictionary = new Dictionary<int, int>();

        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        private void Awake()
        {
            InitializeWorld();
        }

        public void InitializeWorld()
        {
            walkableRegionsDictionary.Clear();
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

            foreach (TerrainType region in walkableRegions)
            {
                walkableMask.value |= region.terrainMask.value;
                walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value,2),region.terrainPenalty);
            }
            
            CreateGrid();
        }


        public int MaxSize
        {
            get { return gridSizeX * gridSizeY; }
        }

        private void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottemLeft =
                transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottemLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                         Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unWalkableMask));

                    int movePenalty = 0;
                    
                    //check for movement penalty
                    if (walkable)
                    {
                        //ensure that this ray right below this is lower than the maxDistance for the physics raycast under it
                        Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 100, walkableMask))
                        {
                            walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movePenalty);
                        }
                    }
                    
                    grid[x, y] = new Node(walkable, worldPoint, x, y,movePenalty);
                }
            }
            
            BlurPenaltyMap(blurPenaltyMultiplier);
        }

        void BlurPenaltyMap(int blurSize)
        {
            int kernalSize = blurSize * 2 + 1;
            int kernalExtents = kernalSize - 1 / 2;
            

            int[,] penaltiesHorizontalPass = new int[gridSizeX,gridSizeY];
            int[,] penaltiesVerticalPass = new int[gridSizeX,gridSizeY];

            for (int y = 0; y < gridSizeY; y++)
            {
                for (int x = -kernalExtents; x <= kernalExtents; x++)
                {
                    int sampleX = Mathf.Clamp(x, 0, kernalExtents);
                    penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
                }

                for (int x = 1; x < gridSizeX; x++)
                {
                    int removeIndex =  Mathf.Clamp(x - kernalExtents - 1,0,gridSizeX);
                    int addIndex = Mathf.Clamp(x + kernalExtents, 0, gridSizeX - 1);

                    penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
                }
            }
            
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = -kernalExtents; y <= kernalExtents; y++)
                {
                    int sampleY = Mathf.Clamp(y, 0, kernalExtents);
                    penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
                }

                for (int y = 1; y < gridSizeY; y++)
                {
                    int removeIndex =  Mathf.Clamp(y - kernalExtents - 1,0,gridSizeY);
                    int addIndex = Mathf.Clamp(y + kernalExtents, 0, gridSizeY - 1);

                    penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x , y -1] - penaltiesHorizontalPass[x,removeIndex] + penaltiesHorizontalPass[x,addIndex];
                    int blurredPenalty =
                        Mathf.RoundToInt((float) penaltiesVerticalPass[x, y] / (kernalExtents * kernalExtents));
                    grid[x, y].movementPenalty = blurredPenalty;
                }
                
            }
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return grid[x, y];
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

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null && displayGridGizmos)
            {
                foreach (Node node in grid)
                {
                    Gizmos.color = (node.isWalkable) ? Color.white : Color.red;

                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        
        
    }

    [Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}