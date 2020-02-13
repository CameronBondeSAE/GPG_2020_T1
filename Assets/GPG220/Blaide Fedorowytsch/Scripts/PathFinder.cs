using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public int gridSizeX;
    public int gridSizeY;
    public PathNode[,] nodeGrid;

    public List<PathNode> closedNodes;
    public List<PathNode> openNodes;
    public List<PathNode> newlyOpenedNodes;
    // Start is called before the first frame update
    void Start()
    {
        nodeGrid = new PathNode[gridSizeX,gridSizeY];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchAroundNode(PathNode queryNode,PathNode startNode, PathNode goalNode)
    {
        if (nodeGrid[queryNode.x, queryNode.y].obstruction == false)
        {
            List<PathNode> surroundingNodes = new List<PathNode>(8);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if ((i >= 0 && j >= 0)&&(i<=gridSizeX && j <= gridSizeY))
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
                /*if (pathNode.y != startNode.y && pathNode.x != startNode.x)
                {
                    //this would be a diagonal move.
                    pathNode.gCost = 14 + pathNode.parentNode.gCost;
                }
                else
                {
                    //this would be a non-diagonal move.
                    pathNode.gCost = 10 + pathNode.parentNode.gCost;
                }*/
                newlyOpenedNodes.Add(pathNode);
            }
        }
    }
    

    public void FindPath(PathNode startNode, PathNode goalNode)
    {
        bool findingPath = true;
        int loopCount = 0;
        openNodes.Add(startNode);
        PathNode lowestNode = startNode;
        
        while (findingPath || loopCount> gridSizeX*gridSizeY)
        { 
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
    public int x;
    public int y;
    public Vector2 gridPosition;
    //distance from start node.
    public float gCost;
    public float hCost;
    public float fCost;
    public PathNode parentNode;
    public bool obstruction;
    public Vector3 worldPosition;
}