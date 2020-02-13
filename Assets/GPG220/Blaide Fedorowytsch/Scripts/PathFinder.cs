using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public int gridSizeX;
    public int gridSizeY;
    public PathNode[,] nodeGrid;
    // Start is called before the first frame update
    void Start()
    {
        nodeGrid = new PathNode[gridSizeX,gridSizeY];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SearchAroundNode(int gridX,int gridY)
    {
        if (nodeGrid[gridX, gridY].obstruction == false)
        {
            List<PathNode> surroundingNodes;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if ((i >= 0 && j >= 0)&&(i<=gridSizeX && j <= gridSizeY))
                    {
                        

                    }
                }
                
            }



        }
    }
}

public class PathNode
{
    public int gCost;
    public int hCost;
    public int fCost;
    public PathNode parentNode;
    public bool obstruction;
    public Vector3 worldPosition;
}