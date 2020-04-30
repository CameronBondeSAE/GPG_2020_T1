using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.PathFinding
{
	public class SimplePathfinder : MonoBehaviour
	{
		private NodeGrid  grid;
		public  Transform seeker, target;
		private object    gridlocker   = new object();
		private object    paranoidLock = new object();

		public delegate void pathFindingCallBack(List<Node> list);

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
			//   FindPath(seeker.position,target.position);
		}

		public async void RequestPathFind(Vector3 startPos, Vector3 targetPos, pathFindingCallBack callBack)
		{
			// Debug.Log("Main thread: StartPos = " + startPos + " : TargetPos = " + targetPos);
			Node startNode  = grid.NodeFromWorldPoint(startPos);
			Node targetNode = grid.NodeFromWorldPoint(targetPos);

			List<Node> path = await Task.Run(() => FindPath(startNode, targetNode));
			// List<Node> path = FindPath(startNode, targetNode);

			// Debug.Log("DONE! Nodes = " + path.Count);
			// Debug.Log("		ID = " + Thread.CurrentThread.ManagedThreadId);
			callBack(path);
		}


		public List<Node> FindPath(Node startNode, Node targetNode)
		{
			lock (paranoidLock)
			{
				// Debug.Log("Start pathing");
				// Debug.Log("		ID = " + Thread.CurrentThread.ManagedThreadId);
/*           

            startPos = transform.InverseTransformPoint(startPos);
            targetPos = transform.InverseTransformPoint(targetPos);*/


				if (targetNode.walkable == false)
				{
					// Debug.Log("Not walkable");
					// Debug.Log("		ID = " + Thread.CurrentThread.ManagedThreadId);

					return null; // new List<Node>();
				}

				List<Node>    openSet   = new List<Node>();
				HashSet<Node> closedSet = new HashSet<Node>();

				openSet.Add(startNode);


				// Debug.Log("While starting");
				// Debug.Log("		ID = " + Thread.CurrentThread.ManagedThreadId);
				while (openSet.Count > 0)
				{
					Node currentNode = openSet[0];
					for (int i = 1; i < openSet.Count; i++)
					{
						if (openSet[i].FCost < currentNode.FCost ||
							(openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
						{
							currentNode = openSet[i];
						}
					}

					openSet.Remove(currentNode);
					closedSet.Add(currentNode);

					if (currentNode == targetNode)
					{
						// Debug.Log("RetracePath");
						// Debug.Log("		ID = " + Thread.CurrentThread.ManagedThreadId);

						return RetracePath(startNode, targetNode);
					}

					List<Node> neighbours;

					lock (gridlocker)
					{
						neighbours = grid.GetNeighbours(currentNode);
					}

					foreach (Node neighbour in neighbours)
					{
						if (!neighbour.walkable || closedSet.Contains(neighbour))
						{
							continue;
						}

						int newMovementCostToNeighbour = currentNode.gCost + GetDistantce(currentNode, neighbour);
						if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
						{
							neighbour.gCost  = newMovementCostToNeighbour;
							neighbour.hCost  = GetDistantce(neighbour, targetNode);
							neighbour.parent = currentNode;
							if (!openSet.Contains(neighbour))
							{
								openSet.Add(neighbour);
							}
						}
					}
				}

				// Debug.Log("Pathing ended : No Nodes");
				// Debug.Log("		ID = " + Thread.CurrentThread.ManagedThreadId);

				// return new List<Node>();
				return null;
			}
		}

		List<Node> RetracePath(Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node       currentNode;
			currentNode = endNode;
			// CAM: HACK: Thread is nulling when two are running here EVEN THOUGH ENDNODE ISN'T NULL

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				try
				{
					currentNode = currentNode.parent;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}

			path.Reverse();
			lock (gridlocker)
			{
				grid.path = path;
			}

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