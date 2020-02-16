using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.WSA;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class PathFinderOne : SerializedMonoBehaviour
    {
        public Vector2Int gridSize = new Vector2Int(2, 2);
        
        [HideInInspector] public PathNode[,] nodeGrid;
        [HideInInspector] public GameObject[,] displayObjects;
        
        public GameObject displayPrefab;
        public bool drawGrid;
        public Material openMaterial;
        public Material closedMaterial;
        public Material obstructedMaterial;
        
        public Transform oppositeCornerTransform;
        public Vector2 tileSize;
        public Vector3 tileSizeDefault;


        [HideInInspector] public List<PathNode> closedNodes;
        [HideInInspector] public List<PathNode> openNodes;
        [HideInInspector] public List<PathNode> newlyOpenedNodes;

        public LayerMask obstructionlayerMask;
        public Transform tileHolder;


        public Transform startWorldTransform;
        public Transform goalWorldTransform;


        [Button("PathFindTest")]
        void PathFindTest()
        {
            Vector2Int startGridPosition = WorldPositionToGridPosition(startWorldTransform.position);
            Vector2Int goalGridPosition = WorldPositionToGridPosition(goalWorldTransform.position);

            PathNode startNode = nodeGrid[startGridPosition.x, startGridPosition.y];
            PathNode goalNode = nodeGrid[goalGridPosition.x, goalGridPosition.y];

            FindPath(startNode,goalNode);
        }


        // Start is called before the first frame update
        void Start()
        {
            UpdateGrid();
        }

        [Button("UodateGrid")]
        public void UpdateGrid()
        {
            nodeGrid = new PathNode[gridSize.x,gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector2Int gridPos = new Vector2Int(x,y);
                    bool obstruction = !Physics.CheckBox(GridPositionToWorldPosition(gridPos),new Vector3(tileSize.x/2,1,tileSize.y/2),transform.rotation,obstructionlayerMask);
                    nodeGrid[x,y] = new PathNode(obstruction,GridPositionToWorldPosition(gridPos));
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
            
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    
                    displayObjects[x,y] = Instantiate(displayPrefab, GridPositionToWorldPosition(new Vector2Int(x, y)), transform.rotation,tileHolder);
                    displayObjects[x,y].transform.localScale = new Vector3(tileSize.x *tileSizeDefault.x,tileSizeDefault.y,tileSize.y * tileSizeDefault.z);
                    if (!nodeGrid[x,y].obstruction)
                    {
                        ColourizeTile(new Vector2Int(x,y),obstructedMaterial);
                    }
                }
            }
        }

        public void ColourizeTile(Vector2Int gridPosition, Material m)
        {
            MeshRenderer meshRenderer = displayObjects[gridPosition.x, gridPosition.y].GetComponent<MeshRenderer>();

            meshRenderer.sharedMaterial = m;
        }
        
        public List<PathNode> SearchAroundNode(PathNode queryNode,PathNode startNode, PathNode goalNode)
        {
                foreach (PathNode surroundingNode in SurroundingNodes(queryNode))
                {
                    float gCostAddition;
                    if (surroundingNode.gridPosition.y != startNode.gridPosition.y &&
                        surroundingNode.gridPosition.x != startNode.gridPosition.x)
                    {
                        
                        gCostAddition = 14; //this would be a diagonal move.
                    }
                    else
                    {
                        gCostAddition = 10; //this would be a non-diagonal move.
                    }

                    if (surroundingNode.gCost < Mathf.Infinity &&
                        surroundingNode.gCost > queryNode.gCost + gCostAddition)
                    {

                    }
                    else
                    {
                        if (surroundingNode.parentNode != null)
                        {
                            surroundingNode.gCost = gCostAddition + surroundingNode.parentNode.gCost;
                        }
                        else
                        {
                            //surroundingNode.
                        }
                    }
                    newlyOpenedNodes.Add(surroundingNode);
                }

                return newlyOpenedNodes;
        }

        public List<PathNode> SurroundingNodes(PathNode queryNode)
        {
            List<PathNode> surroundingNodes = new List<PathNode>(8);

            for (int x= 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if ((x>= 0 && y >= 0)&&(x<=gridSize.x && y <= gridSize.y))
                    {
                        if (!nodeGrid[x, y].obstruction)
                        {
                            surroundingNodes.Add(nodeGrid[x,y]);
                        }
                    }
                }
            }

            return surroundingNodes;
        }


        public void FindPath(PathNode startNode, PathNode goalNode)
        {
            openNodes = new List<PathNode>();
            closedNodes = new List<PathNode>();
            newlyOpenedNodes = new List<PathNode>();
            
            bool findingPath = true;
            int loopCount = 0;
            openNodes.Add(startNode);
            PathNode lowestNode = startNode;
            startNode.parentNode = startNode;


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
        
        public bool obstruction;
        public Vector3 worldPosition;

       public PathNode(bool _obstruction, Vector3 _worldPosition)
       {
           obstruction = _obstruction;
           worldPosition = _worldPosition;
       }
    }
}