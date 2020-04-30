using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Unit;
using Random = UnityEngine.Random;

namespace GPG220.Dylan.King_Scripts
{
    public class UnitSpawnerAbility : AbilityBase
    {
        public UnitBase unitPrefab;

        private UnitSpawner unitSpawner;
        private GameManager gameManager;
        public float spawnRadius = 4;
        public LayerMask groundMask;

        public PlayerBase player;


        public void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            // gameManager.startGameEvent += Init;
			Init();
		}

        public void Init()
        {
            unitSpawner = gameManager.unitSpawner;
            player = gameManager.localPlayer;
        }

        public override bool SelectedExecute()
		{
			base.SelectedExecute();
			
            SpawnUnit(player, unitPrefab, GetRandomSpawnPoint());
            return true;
        }

        void SpawnUnit(PlayerBase localPlayer, UnitBase unitToSpawn, Vector3 locationToSpawn)
        {
            unitSpawner.SpawnUnit(localPlayer.netIdentity, unitToSpawn, locationToSpawn, Quaternion.identity);
        }

        Vector3 GetRandomSpawnPoint()
        {
			Vector3 position = transform.position;

            float randomPosX = Random.Range(position.x - spawnRadius,
                position.x + spawnRadius);
            float randomPosY = position.y;
            float randomPosZ = Random.Range(position.z - spawnRadius,
                position.z + spawnRadius);

            Vector3 randomPos = new Vector3(position.x, position.y, position.z);
            return randomPos;
        }

        public Vector3 GetCentreOfCamera()
        {
            Vector3 cameraPos = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Ray ray = Camera.main.ScreenPointToRay(cameraPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, groundMask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green, 10f);
                // return hit.point;
            }

            return hit.point;
        }
    }
}