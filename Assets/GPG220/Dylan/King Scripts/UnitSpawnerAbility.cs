using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Resources;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using Random = UnityEngine.Random;

namespace GPG220.Dylan.King_Scripts
{
    public class UnitSpawnerAbility : AbilityBase
    {
        public int unitPrefabIndex;
        private UnitSpawner unitSpawner;
        private GameManager gameManager;
        public float spawnRadius = 4;
        public LayerMask groundMask;
        public PlayerBase player;
        public Inventory inventory;
        public ResourceType resourceType;
        public int spawnCost;

        public void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            inventory = GetComponent<Inventory>();
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
            // if (inventory.GetResourceQuantity(resourceType) >= spawnCost)
            {
                // inventory.RemoveResources(resourceType, spawnCost);
                CmdSpawnUnit(player.netIdentity, unitPrefabIndex, GetRandomSpawnPoint());
            }

            return true;
        }

        [Command]
        void CmdSpawnUnit(NetworkIdentity playerOwner, int unitToSpawnIndex, Vector3 locationToSpawn)
        {
            unitSpawner.SpawnUnit(playerOwner, unitSpawner.unitBases[unitToSpawnIndex], locationToSpawn,
                Quaternion.identity);
        }


        Vector3 GetRandomSpawnPoint()
        {
            Vector3 position = transform.position;

            float randomPosX = Random.Range(position.x - spawnRadius,
                position.x + spawnRadius);
            float randomPosY = position.y;
            float randomPosZ = Random.Range(position.z - spawnRadius,
                position.z + spawnRadius);

            Vector3 randomPos = new Vector3(randomPosX, randomPosY, randomPosZ);
            return randomPos;
        }
    }
}