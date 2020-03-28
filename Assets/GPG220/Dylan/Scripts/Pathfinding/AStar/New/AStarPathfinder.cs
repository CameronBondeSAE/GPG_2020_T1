using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;
using Grid = Pathfinding.AStar.New.Grid;

namespace Pathfinding.New
{
    public class AStarPathfinder : MonoBehaviour
    {
        private PathRequestManager requestManager;
        private Grid grid;

        private void Awake()
        {
            grid = GetComponent<Grid>();
            requestManager = GetComponent<PathRequestManager>();
        }


        public void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
        {
            StartCoroutine(FindPath(pathStart, pathEnd));
        }

        IEnumerator FindPath(Vector3 startPos, Vector3 endPos)
        {
            Vector3[] wayPoints = new Vector3[0];
            bool pathSuccess = false;

            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(endPos);

            if (startNode.isWalkable && targetNode.isWalkable)
            {
                Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
                HashSet<Node> closeSet = new HashSet<Node>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closeSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        
                        pathSuccess = true;

                        break;
                    }

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.isWalkable || closeSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementConstToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                        if (newMovementConstToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementConstToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                            else
                            {
                                openSet.UpdateItem(neighbour);
                            }
                        }
                    }
                }
            }

            yield return null;
            if (pathSuccess)
            {
                wayPoints = RetracePath(startNode, targetNode);
            }

            requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
        }

        Vector3[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
        }

        Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew =
                    new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].worldPosition);
                }

                directionOld = directionNew;
            }

            return waypoints.ToArray();
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int disX = Math.Abs(nodeA.gridX - nodeB.gridX);
            int disY = Math.Abs(nodeA.gridY - nodeB.gridY);

            if (disX > disY)
            {
                return 14 * disY + 10 * (disX - disY);
            }

            return 14 * disX + 10 * (disY - disX);
        }
    }
}