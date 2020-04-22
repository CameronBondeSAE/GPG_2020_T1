using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.PathFinding
{
    public class SimplePathfinder : MonoBehaviour
    {
        private NodeGrid grid;
        public Transform seeker, target;
        

        private void Awake()
        {
            grid = GetComponent<NodeGrid>();
            

        }

        private void Update()
        {
           // FindPath(seeker.position,target.position);
        }

        [Button(ButtonStyle.FoldoutButton)]
        public void TestPathFind()
        {
            FindPath(seeker.position,target.position);
        }

        public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
        {
/*

            startPos = transform.InverseTransformPoint(startPos);
            targetPos = transform.InverseTransformPoint(targetPos);*/
            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(targetPos);

            if (targetNode.walkable == false)
            {
                return new List<Node>();
            }

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost< currentNode.hCost ))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(startNode,targetNode);
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistantce(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistantce(neighbour, targetNode);
                        neighbour.parent = currentNode;
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
            return new List<Node>();
        }

        List<Node> RetracePath(Node startNode,Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode;
            currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse(); 
            grid.path = path;
           return path;
        }
        int GetDistantce(Node nodeA, Node nodeB)
        {
            int distx = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
            int disty = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

            if (distx > disty)
            {
                return 14 * disty + 10 * (distx - disty);
            }
            return 14 * distx + 10 * (disty - distx);


        }

    }
}
