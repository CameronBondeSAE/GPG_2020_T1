using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.WSA;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class PathFinder : SerializedMonoBehaviour
    {
        public Vector2Int gridSize = new Vector2Int(2, 2);
        [HideInInspector] public PathNode[,] nodeGrid;
        [HideInInspector] public GameObject[,] displayObjects;
        public GameObject displayPrefab;
        public bool drawGrid;
        public Material openMaterial;
        public Material closedMaterial;
        public Material obstructedMaterial;
        

        public List<PathNode> closedNodes;
        public List<PathNode> openNodes;
        public List<PathNode> newlyOpenedNodes;

        public Transform oppositeCornerTransform;


        public Vector2 tileSize;


        [Button("PathFindTest")]
        void PathFindTest()
        {
            if (startNodePosition.x > gridSize.x)
            {
                startNodePosition.x = gridSize.x;
            }
            if (startNodePosition.y > gridSize.y)
            {
                startNodePosition.y = gridSize.y;
            }
            
            PathNode startNode = new PathNode();
            PathNode goalNode = new PathNode();
            startNode.gridPosition = startNodePosition;
            goalNode.gridPosition = goalNodePosition;
            FindPath(startNode,goalNode);



        }

        public Vector2Int startNodePosition;
        public Vector2Int goalNodePosition;
        

        private BoxCollider boxCollider;
        // Start is called before the first frame update
        void Start()
        {
            
            UpdateGrid();

        }

        // Update is called once per frame
        void Update()
        {
            
        }
        [Button("UodateGrid")]
        public void UpdateGrid()
        {
            nodeGrid = new PathNode[gridSize.x,gridSize.y];
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    nodeGrid[i,j] = new PathNode();
                }
            }
            
            float gridWidth = Mathf.Abs(transform.position.x - oppositeCornerTransform.position.x);
            float gridHeight = Mathf.Abs(transform.position.z - oppositeCornerTransform.position.z);
            
            
            tileSize = new Vector2(gridWidth / gridSize.x,gridHeight / gridSize.y);
            if (drawGrid)
            {
                DrawGrid();
            }
        }
        /// <summary>
        /// The world position at the centre of the grid square.
        /// </summary>
        /// <param name="gridPosition"></param>
        /// <returns></returns>
        public Vector3 GridPositionToWorldPosition(Vector2Int gridPosition)
        {
            //return new Vector3(gridPosition.x + transform.position.x + tileSize.x/2,transform.position.y,gridPosition.y + transform.position.z + tileSize.y/2);
            
            return new Vector3(gridPosition.x * tileSize.x +transform.position.x + tileSize.x/2,transform.position.y,gridPosition.y * tileSize.y +transform.position.z +tileSize.y/2);
        }
        
        /// <summary>
        /// the grid square that contains the closest point on the grid to that world position.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector2Int WorldPositionToGridPosition(Vector3 worldPosition)
        {
            return new Vector2Int(Mathf.RoundToInt((worldPosition.x - tileSize.x/2) - transform.position.x),Mathf.RoundToInt((worldPosition.y - tileSize.y/2)-transform.position.y));
        }


        public void CheckForObstructions()
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                   // boxCollider.center = blah.
                   //boxCollider.


                }
            }


        }


        public void DrawGrid()
        {
            if (displayObjects != null)
            {
                if (displayObjects.GetLength(0) > 0 || displayObjects.GetLength(1) > 0)
                {
                    for (int i = 0; i < displayObjects.GetLength(0); i++)
                    {
                        for (int j = 0; j < displayObjects.GetLength(1); j++)
                        {
                            Destroy(displayObjects[i, j]);
                        }
                    }
                }
            }

            displayObjects = new GameObject[gridSize.x,gridSize.y];
            
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    
                    displayObjects[i,j] = Instantiate(displayPrefab, GridPositionToWorldPosition(new Vector2Int(i, j)), transform.rotation);
                    displayObjects[i,j].transform.localScale = new Vector3(tileSize.x *0.8f,0.1f,tileSize.y * 0.8f);
                }
            }
        }

        public void ColourizeTile(Vector2Int gridPosition, Material m)
        {
            MeshRenderer meshRenderer = displayObjects[gridPosition.x, gridPosition.y].GetComponent<MeshRenderer>();

            meshRenderer.material = m;
        }


        public void SearchAroundNode(PathNode queryNode,PathNode startNode, PathNode goalNode)
        {
            if (nodeGrid[queryNode.gridPosition.x, queryNode.gridPosition.y].obstruction == false)
            {
                List<PathNode> surroundingNodes = new List<PathNode>(8);

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((i >= 0 && j >= 0)&&(i<=gridSize.x && j <= gridSize.y))
                        {
                            if (!nodeGrid[i, j].obstruction)
                            {
                                surroundingNodes.Add(nodeGrid[i,j]);
                            }
                        }
                    }
                }
                foreach (PathNode pathNode in surroundingNodes)
                {
                    pathNode.gCost = Vector2.Distance(pathNode.gridPosition, startNode.gridPosition);
                    pathNode.hCost = Vector2.Distance(pathNode.gridPosition, goalNode.gridPosition);
                    pathNode.fCost = pathNode.gCost + pathNode.hCost;

                    float gCostAddition;
                
                    if (pathNode.gridPosition.y != startNode.gridPosition.y && pathNode.gridPosition.x != startNode.gridPosition.x)
                
                    {
                        //this would be a diagonal move.
                        gCostAddition = 14;
                    }
                    else
                    {
                        //this would be a non-diagonal move.
                        gCostAddition = 10;
                    } 
                    
                    if (pathNode.gCost < Mathf.Infinity && pathNode.gCost > queryNode.gCost + gCostAddition)
                    {
                        
                    }
                    else
                    {
                        pathNode.gCost = gCostAddition + pathNode.parentNode.gCost; 
                    }
                    
                    
                    
                    
                    
                    newlyOpenedNodes.Add(pathNode);
                }
            }
        }
    

        public void FindPath(PathNode startNode, PathNode goalNode)
        {
            openNodes = new List<PathNode>();
            
            bool findingPath = true;
            int loopCount = 0;
            openNodes.Add(startNode);
            PathNode lowestNode = startNode;
            startNode.parentNode = startNode;
            SearchAroundNode(startNode, startNode, goalNode);
            
            while (findingPath || loopCount> gridSize.x*gridSize.y)
            { 
                foreach (PathNode node in newlyOpenedNodes)
                {
                    openNodes.Add(node);
                }
                
                foreach (PathNode openNode in openNodes)
                {
                    if (openNode == goalNode)
                    {
                        findingPath =false;
                    }
                    else
                    {
                        if (openNode.hCost < lowestNode.hCost)
                        {
                            lowestNode = openNode;
                        }
                    }
                }
                SearchAroundNode(lowestNode, startNode, goalNode);
                
            }
        }
    }

    public class PathNode
    {
        public Vector2Int gridPosition;
        //distance from start node.
        public float gCost = Mathf.Infinity;
        public float hCost = Mathf.Infinity;
        public float fCost = Mathf.Infinity;
        public PathNode parentNode;
        public bool obstruction = false;
        public Vector3 worldPosition;
    }
}