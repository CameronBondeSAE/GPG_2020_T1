using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.PathFinding;
using UnityEngine;

public class Node : IHeapItem<Node>
{
  

    public bool walkable;
    public Vector3 worldPosition;
    public Vector2Int gridPosition;

    public int gCost;
    public int hCost;

    public Node parent;
    private int heapIndex;
    
    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridPosition = new Vector2Int(_gridX,_gridY);
    }
    
    public int FCost
    {
        get { return 1; }
    }
    
    public int HeapIndex {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}
