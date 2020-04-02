using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Blaide_Fedorowytsch.Scripts.ProcGen
{
    public class ProceduralObstacleSystem : MonoBehaviour
    {
        [HideInInspector]
        public bool[,] obstacleBoolGrid;
        [HideInInspector]
        public bool[,] lastObstacleBoolGrid;
        [HideInInspector]
        public GameObject[,] obstacleObjectGrid;

        
        
        public Vector2Int gridSize = new Vector2Int(1,1);
        public Vector2 worldSize;

        private Vector2Int gridSizeLocker;

        public float obstacleHeight;
        public Vector2 obstacleGenerationDensity;
        public Vector2 seed;
        [Range(0.0f,1f)]
        public float obstacleThreshold;
        public float obstacleThresholdLast;


        public bool growObstacles;
        
        public float baseGrowRate;
        public AnimationCurve growcurve;
        public float roundTime;
        public float timer;
        

        public GameObject obstaclePrefab;
        public GameObject ObstacleHolder;

        public GameManager gameManager;
        public bool applyOverrideGrid = false;

        public List<Vector2Int> closedEdges;
        public List<Vector2Int> openEdges;
        
        private bool gameStarted = false;

        // Start is called before the first frame update
        void Start()
        {
            //gameManager.startGameEvent += GenerateAll;
                gridSizeLocker = gridSize;
                GenerateAll();
        }

        void GenerateAll()
        {
            GenerateBoolGrid();
            GenerateObjectGrid();
            gameStarted = true;
        }

        // Update is called once per frame
        void Update()
        {


            if (Math.Abs(obstacleThreshold - obstacleThresholdLast) > 0.001f ) // only update boolGrids With Perlin if the threshold Changes.
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
                    if (obstacleBoolGrid[x,y])
                    {
                        foreach (Vector2Int gridposition in GetNeighbourPositions(new Vector2Int(x, y)))
                        {
                            if (obstacleBoolGrid[gridposition.x, gridposition.y] == false)
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
                    if (obstacleBoolGrid[g.x, g.y] == false)
                    {
                        openEdges.Add(g);
                    }
                }
            }
            
        }

        public void GrowObstacleBoolGrid()
        {
            List<Vector2Int> temp = new List<Vector2Int>();
            foreach (Vector2Int gridPosition in openEdges)
            {
                if (Random.Range(1,100) < 20 && obstacleBoolGrid[gridPosition.x, gridPosition.y] == false)
                {
                    temp.Add(gridPosition);
                    obstacleBoolGrid[gridPosition.x, gridPosition.y] = true;
                }
            }

            foreach (Vector2Int gridPosition in temp)
            {
                openEdges.Remove(gridPosition);
                closedEdges.Add(gridPosition);
                foreach (Vector2Int neighbourPosition in GetNeighbourPositions(gridPosition))
                {
                    if (obstacleBoolGrid[neighbourPosition.x, neighbourPosition.y] == false)
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
                        neighbours.Add( new Vector2Int(checkX,checkY));
                    }
                }
            }

            return neighbours;

        }
        
       
        
        


        void GenerateBoolGrid()
        {
            obstacleBoolGrid = new bool[gridSize.x,gridSize.y];
            lastObstacleBoolGrid = new bool[gridSize.x,gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (PerlinThresholdCheck(obstacleThreshold, obstacleGenerationDensity, new Vector2Int(x, y)))
                    {
                        obstacleBoolGrid[x, y] = true;

                    }
                    else
                    {
                        obstacleBoolGrid[x, y] = false;
                    }
                }   
            }
        }

        void GenerateObjectGrid()
        {
            obstacleObjectGrid = new GameObject[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    obstacleObjectGrid[x, y] = Instantiate(obstaclePrefab,transform.position + new Vector3((x) *0.01f* (worldSize.x/(gridSize.x*0.01f)), obstacleHeight/2, (y) *0.01f* (worldSize.y/(gridSize.y*0.01f))),transform.rotation);
                    obstacleObjectGrid[x, y].transform.localScale = new Vector3(worldSize.x*(gridSize.x/100)*0.01f,obstacleHeight,worldSize.y* (gridSize.y/100)*0.01f);


                    if (ObstacleHolder)
                    {
                        obstacleObjectGrid[x, y].transform.parent = ObstacleHolder.transform;
                    }
                    else
                    {
                       obstacleObjectGrid[x, y].transform.parent = transform; 
                    }

                    
                    
                    
                    if (PerlinThresholdCheck(obstacleThreshold, obstacleGenerationDensity, new Vector2Int(x, y)))
                    {
                        obstacleObjectGrid[x,y].SetActive(true);
                    }
                    else
                    {
                        obstacleObjectGrid[x,y].SetActive(false);
                    }
                }   
            }
        }


        List<Vector2Int> CompareBoolGrids()
        {
            List<Vector2Int> list = new List<Vector2Int>();
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (obstacleBoolGrid[x,y] != lastObstacleBoolGrid[x,y])
                    {
                        list.Add(new Vector2Int(x,y));
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
                    lastObstacleBoolGrid[x, y] = obstacleBoolGrid[x, y];
                    if (PerlinThresholdCheck(obstacleThreshold, obstacleGenerationDensity, new Vector2Int(x, y)))
                    {
                        obstacleBoolGrid[x, y] = true;
                    }
                    else
                    {
                        obstacleBoolGrid[x, y] = false;
                    }
                }  
            }
        }
        
        
        void UpdateObstacleGrid()
        {
            List<Vector2Int> gridPositions = CompareBoolGrids() ;

            foreach (Vector2Int gridPosition in gridPositions)
            {
                if (obstacleBoolGrid[gridPosition.x, gridPosition.y])
                {
                    obstacleObjectGrid[gridPosition.x,gridPosition.y].SetActive(true); 
                }
                else
                {
                    obstacleObjectGrid[gridPosition.x,gridPosition.y].SetActive(false);
                }

                
            }
        }

        public bool PerlinThresholdCheck(float threshold, Vector2 density, Vector2Int gridPosition)
        {
            if (Mathf.PerlinNoise((gridPosition.x + seed.x )* density.x, (gridPosition.y + seed.y) * density.y ) >= threshold -0.1f) // HACK  > 0.1f TODO Figure out why perlin is somehow returning lower than 0.
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
            foreach (Vector2Int v in openEdges)
            {
                Gizmos.DrawSphere(transform.position + new Vector3(v.x*(worldSize.x*0.01f),8,v.y*(worldSize.y*0.01f)), 2f);
            }
        }
    }
    
    
}
