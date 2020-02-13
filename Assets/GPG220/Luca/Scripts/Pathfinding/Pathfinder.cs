using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        public bool goToNearestPossiblePos = true;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        /*public int gridSizeX = 0;
        public int gridSizeY = 0;
        public Node[,] nodes;*/
        public List<Node> worldNodes = new List<Node>();
        
        public GameObject ground;
        public float maxNodeDist = 1f;
        public LayerMask obstacleMasks;

        public bool doDebug = false;
        public GameObject debugPrefab;
        public Material debugGreenMat;
        public Material debugRedMat;
        public Material debugEndMat;
        public Material debugStartMat;
        
        public List<GameObject> debugGameObjects = new List<GameObject>();


        public GameObject testStartPos;
        public GameObject testEndPos;
        [Button("Find Path"), DisableInEditorMode]
        public void TestFindPath()
        {
            List<Vector3> waypoints = FindPath(testStartPos.transform.position, testEndPos.transform.position);
        }
        
        [Button("Calculate World Nodes"), DisableInEditorMode]
        public void CalculateWorldNodes()
        {
            worldNodes.Clear();

            var groundBounds = ground.GetComponent<MeshRenderer>()?.bounds ?? default;

            int rowsX = (int)(groundBounds.size.x / maxNodeDist) - 1;
            int rowsZ = (int)(groundBounds.size.z / maxNodeDist) - 1;
            
            var currentPos = ground.transform.position;
            currentPos.x -= groundBounds.extents.x + (maxNodeDist/2);
            var zOrigin = currentPos.z - groundBounds.extents.z + (maxNodeDist/2);
            currentPos.z = zOrigin;
            
            var nodes = new Node[rowsX,rowsZ];
            
            // Create Nodes
            for (var x = 0; x < rowsX; x++)
            {
                for (var z = 0; z < rowsZ; z++)
                {
                    if (!Physics.CheckSphere(currentPos, maxNodeDist, obstacleMasks))
                    {
                        Node node = new Node();
                        node.position = currentPos;
                        nodes[x,z] = node;

                        worldNodes.Add(node);
                    }

                    currentPos.z += maxNodeDist;
                }
                currentPos.z = zOrigin;
                currentPos.x += maxNodeDist;
            }
            
            // Add Neighbours
            for (var x = 0; x < rowsX; x++)
            {
                for (var z = 0; z < rowsZ; z++)
                {
                    Node node = nodes[x,z];
                    if (node != null)
                    {
                        if (x > 0 && nodes[x - 1, z] != null) // Left
                            node.neighbourNodes.Add(nodes[x - 1, z]);
                        if (x < rowsX-1 && nodes[x + 1, z] != null) // Right
                            node.neighbourNodes.Add(nodes[x + 1, z]);

                        if (z > 0)
                        {
                            if(nodes[x, z - 1] != null)
                                node.neighbourNodes.Add(nodes[x, z - 1]);// Bottom
                            if(x > 0 && nodes[x - 1, z - 1] != null)
                                node.neighbourNodes.Add(nodes[x - 1, z - 1]); // Bottom Left
                            if(x < rowsX-1 && nodes[x + 1, z - 1] != null)
                                node.neighbourNodes.Add(nodes[x + 1, z - 1]); // Bottom Right
                        }
                            
                        
                        if (z < rowsZ - 1) 
                        {
                            if(nodes[x, z + 1] != null)
                                node.neighbourNodes.Add(nodes[x, z + 1]);// Top
                            if(x > 0 && nodes[x - 1, z + 1] != null)
                                node.neighbourNodes.Add(nodes[x - 1, z + 1]); // Top Left
                            if(x < rowsX-1 && nodes[x + 1, z + 1] != null)
                                node.neighbourNodes.Add(nodes[x + 1, z + 1]); // Top Right
                        }
                    }
                }
            }
        }

        public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            debugGameObjects.ForEach(Destroy);
            debugGameObjects.Clear();
            var nodesCopy = new List<Node>(worldNodes);

            if (doDebug && nodesCopy.Count > 0 && debugPrefab != null)
            {
                foreach (var node in nodesCopy)
                {
                    Vector3 pos = node.position;
                    pos.y += maxNodeDist/2;
                    node.debugGO = Instantiate(debugPrefab, pos, Quaternion.identity);
                    node.debugGO.transform.localScale = new Vector3(node.debugGO.transform.localScale.x * maxNodeDist,1,node.debugGO.transform.localScale.z * maxNodeDist);
                    node.debugGORenderer = node.debugGO.GetComponent<Renderer>();
                    node.debugGO.SetActive(false);
                    debugGameObjects.Add(node.debugGO);
                }
            }
            
            var startNode = GetNearestNode(nodesCopy, startPos);
            var endNode = GetNearestNode(nodesCopy, targetPos);
            
            if (startNode == null || endNode == null)
                return null;
            
            startNode.GCost = 0;
            startNode.HCost = CalculateHCost(startNode, endNode);
            
            
            
            var greenNodes = new List<Node>(){startNode};
            var redNodes = new List<Node>();

            Node finalEndNode = null;
            while (true)
            {
                var currentNode = GetLowestFCostNode(greenNodes);
                
                if(currentNode == null)
                    break;

                greenNodes.Remove(currentNode);
                redNodes.Add(currentNode);

                if (doDebug && currentNode != null && debugStartMat != null && debugRedMat != null)
                {
                    currentNode.debugGORenderer.material = currentNode.lastNode == null ? debugStartMat : debugRedMat;
                    currentNode.debugGO.SetActive(true);
                }

                if (currentNode.position == endNode.position || currentNode.neighbourNodes == null || currentNode.neighbourNodes.Count == 0)
                {
                    if (goToNearestPossiblePos || currentNode.position == endNode.position)
                    {
                        finalEndNode = currentNode;
                        if (doDebug)
                        {
                            currentNode.debugGORenderer.material = debugEndMat;
                            currentNode.debugGO.SetActive(true);
                        }
                    }
                    break;
                }

                foreach (var neighbourNode in currentNode.neighbourNodes)
                {
                    if(neighbourNode == null || redNodes.Contains(neighbourNode))
                        continue;


                    var neighbourGCost = Vector3.Distance(neighbourNode.position, currentNode.position) +
                                         currentNode.GCost;
                    var neighbourHCost = CalculateHCost(neighbourNode, endNode);
                    var neighbourFCost = neighbourGCost + neighbourHCost;

                    if ((neighbourNode.fCost >= 0 && neighbourFCost < neighbourNode.fCost) || !greenNodes.Contains(neighbourNode))
                    {
                        neighbourNode.GCost = neighbourGCost;
                        neighbourNode.HCost = neighbourHCost;
                        neighbourNode.lastNode = currentNode;

                        if (!greenNodes.Contains(neighbourNode))
                        {
                            greenNodes.Add(neighbourNode);
                            if (doDebug && currentNode != null && debugStartMat != null && debugRedMat != null)
                            {
                                currentNode.debugGORenderer.material = debugGreenMat;
                                currentNode.debugGO.SetActive(true);
                            }
                        }
                    }

                }
                
                
            }

            if (doDebug)
            {
                MarkPath(finalEndNode, debugStartMat);
            }

            List<Vector3> waypoints = CreateWaypointsListFromNode(finalEndNode);

            return waypoints;
        }

        // Hacky debug functio nto mark the path
        private void MarkPath(Node endNode, Material mat)
        {
            if (endNode == null)
                return;
            
            if(endNode.debugGORenderer != null)
                endNode.debugGORenderer.material = mat;
            
            MarkPath(endNode.lastNode, mat);
        }
        

        public Node GetNearestNode(List<Node> nodes, Vector3 position)
        {
            Node nearestNode = null;
            var nearestNodeDist = 0f;

            if (nodes == null || nodes.Count <= 0) return null;
            foreach (var node in nodes)
            {
                var dist = Vector3.Distance(node.position, position);
                if (!(dist < nearestNodeDist) && nearestNode != null) continue;
                nearestNode = node;
                nearestNodeDist = dist;
            }

            return nearestNode;
        }

        private Node GetLowestFCostNode(List<Node> nodes)
        {
            Node lowestCostNode = null;
            foreach (var node in nodes)
            {
                if (lowestCostNode == null || node.fCost < lowestCostNode.fCost)
                {
                    lowestCostNode = node;
                }   
            }

            return lowestCostNode;
        }

        private List<Vector3> CreateWaypointsListFromNode(Node endNode)
        {
            var waypoints = new List<Vector3>();
            if (endNode == null)
                return waypoints;

            Node currentNode = endNode;
            while (currentNode.lastNode != null)
            {
                waypoints.Add(currentNode.position);
                currentNode = currentNode.lastNode;
            }

            return waypoints;
        }

        private float CalculateGCost(Node currentNode)
        {
            return (currentNode?.lastNode == null) ? 0 : Vector3.Distance(currentNode.position, currentNode.lastNode.position) + CalculateGCost(currentNode.lastNode);
        }

        private float CalculateHCost(Node currentNode, Node endNode)
        {
            return Vector3.Distance(currentNode.position, endNode.position);
        }
    }
}
