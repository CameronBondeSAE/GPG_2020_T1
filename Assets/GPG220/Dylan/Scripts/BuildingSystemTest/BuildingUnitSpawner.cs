using System;
using System.Collections.Generic;
using System.Collections;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using Mirror.Examples.Chat;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = UnityEngine.Random;


namespace GPG220.Dylan.Scripts.BuildingSystemTest
{
    public class BuildingUnitSpawner : MonoBehaviour
    {
        private PlayMenu playMenu;
        private UnitSpawner unitSpawner;
        private GameManager gameManager;
        // public float spawnRadius = 4;
        public LayerMask groundMask;

        public HumanPlayer player;

        public bool isSelected;
        
        public void Init()
        {
            StartCoroutine("Delay");
        }

        public IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.5f);
            unitSpawner = FindObjectOfType<UnitSpawner>();
            player = FindObjectOfType<HumanPlayer>();
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            playMenu = FindObjectOfType<PlayMenu>();
            playMenu.playEvent += Init;
        }

        private void OnGUI()
        {
            if (isSelected && player.isLocalPlayerMine)
            {
                for (int i = 0; i < unitSpawner.unitBases.Count; i++)
                {
                    //GUI Buttons
                    if (GUI.Button(new Rect(Screen.width / 20, Screen.height / 15 + Screen.height / 10 * i, 100, 30),
                        unitSpawner.unitBases[i].name))
                    {
                        SpawnUnit(player, unitSpawner.unitBases[i], GetRandomSpawnPoint());
                    }
                }
            }
        }

        Vector3 GetRandomSpawnPoint()
        {
            UnitBase[] unitBases = FindObjectsOfType<UnitBase>();

            Vector3 position = Vector3.zero;
            
            foreach (var unitBase in unitBases)
            {
                if (unitBase.netIdentity == gameManager.localPlayer.netIdentity)
                {
                    // check Unitbase position distance to center of screen. If close enough then spawn
                    if (Vector3.Distance(unitBase.transform.position, GetCentreOfCamera()) > gameManager.unitBuildDistanceThreshold)
                    {
                        position = unitBase.transform.position;
                        return position;
                    }
                }
            }
            return position;
            
            // float randomPosX = Random.Range(transform.position.x - spawnRadius,
            //     transform.position.x + spawnRadius);
            // float randomPosY = transform.position.y;
            // float randomPosZ = Random.Range(transform.position.z - spawnRadius,
            //     transform.position.z + spawnRadius);
            //
            // Vector3 randomPos = new Vector3(randomPosX, randomPosY, randomPosZ);
            // return randomPos;
        }

        public Vector3 GetCentreOfCamera()
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, groundMask))
            {
                Debug.DrawLine(ray.origin,hit.point,Color.green,10f);
                // return hit.point;
            }

            return hit.point;
        }

        void SpawnUnit(HumanPlayer localPlayer, UnitBase unitToSpawn, Vector3 locationToSpawn)
        {
            // bool localPlayer = FindObjectOfType<PlayerBase>().isLocalPlayer;
            unitSpawner.SpawnUnit(localPlayer.netIdentity, unitToSpawn, locationToSpawn, Quaternion.identity);
        }
    }
}