using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Websocket;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Blaide_Fedorowytsch.Scripts.ProcGen
{
    public class ProceduralGrowthSystem : MonoBehaviour
    {
        [HideInInspector] public bool[,] BoolGrid;
        [HideInInspector] public bool[,] lastBoolGrid;
        [HideInInspector] public GameObject[,] ObjectGrid;

        private int obstaclecount;

        public bool gizmos;

        public Vector2Int gridSize = new Vector2Int(1, 1);
        public Vector2 worldSize;

        private Vector2Int gridSizeLocker;

        public float obstacleHeight;
        public Vector2 obstacleGenerationDensity;
        public Vector2 seed;
        [Range(0.0f, 1f)] public float obstacleThreshold;
        public float obstacleThresholdLast;


        public bool growObstacles;
        [Range(0.0f, 1f)] public float baseGrowRate;
        [Range(0.0f, 1f)] public float perlinWeight;
        [Range(0.0f, 1f)] public float perlinGrowthRate;

        private float perlinGrowth;

        //public AnimationCurve growcurve;
        public float roundTime;
        public float timer;


        public GameObject obstaclePrefab;
        public GameObject ObstacleHolder;

        public GameManager gameManager;
        public bool applyOverrideGrid = false;

        public List<Vector2Int> closedEdges;
        public List<Vector2Int> openEdges;

        private bool gameStarted = false;

        public event Action onUpdateProceduralGrowth;


        // Start is called before the first frame update
        void Start()
        {
            //gameManager.startGameEvent += GenerateAll;

        }

        private void Awake()
        {
            gameManager.startGameEvent += StartGrowth;
            growObstacles = false;
            gridSizeLocker = gridSize;
            GenerateBoolGrid();
        }

        void GenerateAll()
        {
            GenerateBoolGrid();
            GenerateObjectGrid();
            gameStarted = true;
        }

        public void StartGrowth()
        {
            FindEdgeObstacles();
            GenerateObjectGrid();
            gameStarted = true;
            growObstacles = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Math.Abs(obstacleThreshold - obstacleThresholdLast) > 0.001f
            ) // only update boolGrids With Perlin if the threshold Changes.
            {
                gridSize = gridSizeLocker;
                if (gameStarted)
                {
                    UpdateBoolGridsWithPerlin();
                    UpdateObstacleGrid();
                    FindEdgeObstacles();
                }

                obstacleThresholdLast = obstacleThreshold;
            }

            if (growObstacles)
            {
                if (timer < roundTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0f;
                    GrowObstacleBoolGrid();
                    UpdateObstacleGrid();
                    updateBoolGrid();
                    

                }
            }
        }

        private void FindEdgeObstacles()
        {
            openEdges.Clear();
            closedEdges.Clear();
            List<Vector2Int> edges = new List<Vector2Int>();
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (BoolGrid[x, y])
                    {
                        foreach (Vector2Int gridposition in GetNeighbourPositions(new Vector2Int(x, y)))
                        {
                            if (BoolGrid[gridposition.x, gridposition.y] == false)
                            {
                                edges.Add(new Vector2Int(x, y));
                                break;
                            }
                        }
                    }
                }
            }

            closedEdges = edges;

            foreach (Vector2Int gridPosition in closedEdges)
            {
                foreach (Vector2Int g in GetNeighbourPositions(gridPosition))
                {
                    if (BoolGrid[g.x, g.y] == false && !openEdges.Contains(g))
                    {
                        openEdges.Add(g);
                    }
                }
            }
        }

        public void GrowObstacleBoolGrid()
        {
            List<Vector2Int> temp = new List<Vector2Int>(gridSize.x * gridSize.y);
            
            
            baseGrowRate -= perlinGrowthRate;
            
            
            foreach (Vector2Int gridPosition in openEdges)
            {
                float perlinChance = ((Mathf.PerlinNoise((gridPosition.x + seed.x) * obstacleGenerationDensity.x,
                                           (gridPosition.y + seed.y) * obstacleGenerationDensity.y) - 0.1f) *
                                      perlinWeight);
                float growChance = +(Random.Range(0f, 1f));
                
                if (growChance * Mathf.Clamp(perlinChance, 0.1f, 1f) > baseGrowRate &&
                    BoolGrid[gridPosition.x, gridPosition.y] == false)
                {
                    temp.Add(gridPosition);
                    BoolGrid[gridPosition.x, gridPosition.y] = true;
                }
            }

            foreach (Vector2Int gridPosition in temp)
            {
                openEdges.Remove(gridPosition);
                closedEdges.Add(gridPosition);
                foreach (Vector2Int neighbourPosition in GetNeighbourPositions(gridPosition))
                {
                    if (BoolGrid[neighbourPosition.x, neighbourPosition.y] == false &&
                        !openEdges.Contains(neighbourPosition))
                    {
                        openEdges.Add(neighbourPosition);
                    }
                }
            }
        }

        public List<Vector2Int> GetNeighbourPositions(Vector2Int gridPosition)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = gridPosition.x + x;
                    int checkY = gridPosition.y + y;


                    if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                    {
                        neighbours.Add(new Vector2Int(checkX, checkY));
                    }
                }
            }

            return neighbours;
        }


        void GenerateBoolGrid()
        {
            BoolGrid = new bool[gridSize.x, gridSize.y];
            lastBoolGrid = new bool[gridSize.x, gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (PerlinThresholdCheck(obstacleThreshold, obstacleGenerationDensity, new Vector2Int(x, y)))
                    {
                        BoolGrid[x, y] = true;
                    }
                    else
                    {
                        BoolGrid[x, y] = false;
                    }
                }
            }
        }

        void GenerateObjectGrid()
        {
            ObjectGrid = new GameObject[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    ObjectGrid[x, y] = Instantiate(obstaclePrefab,
                        transform.position + new Vector3((x) * 0.01f * (worldSize.x / (gridSize.x * 0.01f)),
                            obstacleHeight / 2, (y) * 0.01f * (worldSize.y / (gridSize.y * 0.01f))),
                        transform.rotation);
                    ObjectGrid[x,y].GetComponent<Wall>().gridPos = new Vector2Int(x,y);
                    // ObjectGrid[x, y].transform.localScale = new Vector3(worldSize.x * (gridSize.x / 100) * 0.01f,
                        // obstacleHeight, worldSize.y * (gridSize.y / 100) * 0.01f);

					// Networking
					NetworkServer.Spawn(ObjectGrid[x,y]);
						
						
                    if (ObstacleHolder)
                    {
                        ObjectGrid[x,y].transform.parent = ObstacleHolder.transform;
                    }
                    else
                    {
                        ObjectGrid[x,y].transform.parent = transform;
                    }

                    if (BoolGrid[x,y])
                    {
                        ObjectGrid[x,y].GetComponent<ObstacleSpawnNotifier>().OnAppear();
                    }
                    else
                    {
                       // ObjectGrid[x,y].GetComponent<ObstacleSpawnNotifier>().OnDisappear();
                    }
                }
            }
        }

        public void SetBoolGridPosition(Vector2Int pos, bool setValue)
        {
            BoolGrid[pos.x, pos.y] = setValue;
            openEdges.Add(pos);
            updateBoolGrid();
        }

        List<Vector2Int> CompareBoolGrids()
        {
            List<Vector2Int> list = new List<Vector2Int>();
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (BoolGrid[x, y] != lastBoolGrid[x, y])
                    {
                        list.Add(new Vector2Int(x, y));
                    }
                }
            }

            

            return list;
        }

        public void UpdateBoolGridsWithPerlin()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    lastBoolGrid[x, y] = BoolGrid[x, y];
                    if (PerlinThresholdCheck(obstacleThreshold, obstacleGenerationDensity, new Vector2Int(x, y)))
                    {
                        BoolGrid[x, y] = true;
                    }
                    else
                    {
                        BoolGrid[x, y] = false;
                    }
                }
            }
        }

        public void updateBoolGrid()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    lastBoolGrid[x, y] = BoolGrid[x, y];
                }
            }

        }


        void UpdateObstacleGrid()
        {
            List<Vector2Int> gridPositions = CompareBoolGrids();

            foreach (Vector2Int gridPosition in gridPositions)
            {
                //ObjectGrid[gridPosition.x, gridPosition.y].SetActive(BoolGrid[gridPosition.x, gridPosition.y]);//
                if (BoolGrid[gridPosition.x, gridPosition.y])
                {
                    ObjectGrid[gridPosition.x, gridPosition.y].GetComponent<ObstacleSpawnNotifier>().OnAppear();
                }
                else
                {
                    ObjectGrid[gridPosition.x, gridPosition.y].GetComponent<ObstacleSpawnNotifier>().OnDisappear();
                }
            }
        }

        public bool PerlinThresholdCheck(float threshold, Vector2 density, Vector2Int gridPosition)
        {
            if (Mathf.PerlinNoise((gridPosition.x + seed.x) * density.x, (gridPosition.y + seed.y) * density.y) >=
                threshold - 0.1f) // HACK  > 0.1f TODO Figure out why perlin is somehow returning lower than 0.
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (gizmos)
            {
                foreach (Vector2Int v in openEdges)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(
                        transform.position + new Vector3(v.x * (worldSize.x * 0.01f), 8, v.y * (worldSize.y * 0.01f)),
                        2f);
                }

                foreach (Vector2Int v in closedEdges)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(
                        transform.position + new Vector3(v.x * (worldSize.x * 0.01f), 8, v.y * (worldSize.y * 0.01f)),
                        2f);
                }
            }
        }
    }
}