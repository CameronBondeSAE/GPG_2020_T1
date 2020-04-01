using System;
using System.Collections.Generic;
using UnityEngine;

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

        public GameObject obstaclePrefab;
        public GameObject ObstacleHolder;

        public GameManager gameManager;

        private bool gameStarted = false;
        
        // Start is called before the first frame update
        void Start()
        {
            gameManager.startGameEvent += GenerateAll;
                gridSizeLocker = gridSize;
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
            gridSize = gridSizeLocker;
            if (gameStarted)
            {
                UpdateGrids();
            }
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

        public void UpdateGrids()
        {
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

            foreach (Vector2Int gridPosition in CompareBoolGrids())
            {
                if (obstacleBoolGrid[gridPosition.x, gridPosition.y])
                {
                    obstacleObjectGrid[gridPosition.x,gridPosition.y].SetActive(true); 
                }
                else
                {
                    obstacleObjectGrid[gridPosition.x,gridPosition.y].SetActive(false);
                }

                lastObstacleBoolGrid[gridPosition.x, gridPosition.y] = obstacleBoolGrid[gridPosition.x, gridPosition.y];
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
        
    }
    
}
