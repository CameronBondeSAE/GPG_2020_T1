using System;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using Mirror;
using UnityEngine;

namespace GPG220.Dylan.Scripts
{
    public class ResourceSpawner : MonoBehaviour
    {
        public MapUtilities mapUtilities;
        public ProceduralMeshGenerator proceduralMeshGenerator;
        public GameManager gameManager;
        public float amountToSpawn;
        public GameObject resourcePrefab;

        public void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            mapUtilities = FindObjectOfType<MapUtilities>();
            proceduralMeshGenerator = FindObjectOfType<ProceduralMeshGenerator>();
            gameManager.startGameEvent += gameManagerOnstartGameEvent;
            // gameManager.GameOverEvent += gameManagerOnstartGameEvent;
        }


        public void SpawnResources(float amountToSpawn)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                var tempResource = Instantiate(resourcePrefab,
                    mapUtilities.RandomGroundPointInBounds(proceduralMeshGenerator.mesh.bounds, new Vector3(1, 1, 1)),
                    Quaternion.identity);
                NetworkServer.Spawn(tempResource);
            }

            // gameManager.startGameEvent -= gameManagerOnstartGameEvent;
        }

        void gameManagerOnstartGameEvent() => SpawnResources(amountToSpawn);
    }
}