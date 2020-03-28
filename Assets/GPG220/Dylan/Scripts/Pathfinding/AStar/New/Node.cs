using UnityEngine;

namespace Pathfinding.New
{
    public class Node : IHeapItem<Node>
    {
        public bool isWalkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridY;
        public int movementPenalty;

        public int gCost;
        public int hCost;
        public Node parent;
        private int heapIndex;

        public Node(bool walkable, Vector3 worldPos, int gridXPos, int gridYPos, int penalty)
        {
            isWalkable = walkable;
            worldPosition = worldPos;
            gridX = gridXPos;
            gridY = gridYPos;
            movementPenalty = penalty;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }

        public int HeapIndex
        {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }

            return -compare;
        }
    }
}