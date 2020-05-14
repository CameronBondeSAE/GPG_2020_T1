using System;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using Mirror;
using UnityEngine;

namespace GPG220.Dylan.Scripts
{
    public class ResourceSpawner : NetworkBehaviour
    {
        public MapUtilities mapUtilities;
        public ProceduralMeshGenerator proceduralMeshGenerator;
		public GameManager gameManager;
		public RTSNetworkManager rtsNetworkManager;
        public float amountToSpawn;
        public GameObject resourcePrefab;

        public void Awake()
        {
            mapUtilities = FindObjectOfType<MapUtilities>();
            proceduralMeshGenerator = FindObjectOfType<ProceduralMeshGenerator>();

			// gameManager = FindObjectOfType<GameManager>();
			// gameManager.ServerHostStartedEvent += gameManagerOnstartGameEvent;
			// gameManager.GameOverEvent += gameManagerOnstartGameEvent;

			rtsNetworkManager = FindObjectOfType<RTSNetworkManager>();
			rtsNetworkManager.StartedHostEvent += gameManagerOnstartGameEvent;
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

		void gameManagerOnstartGameEvent()
		{
			
			// TODO: Networking
			// if (Network)
			{
				SpawnResources(amountToSpawn);
			}
		}
	}
}