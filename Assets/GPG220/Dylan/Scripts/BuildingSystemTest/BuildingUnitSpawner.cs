using System;
using System.Collections.Generic;
using System.Collections;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using Mirror.Examples.Chat;
using Sirenix.OdinInspector.Editor.Drawers;
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
        public float spawnRadius = 4;
        public LayerMask groundMask;

        public HumanPlayer player;

        public bool isSelected;
        private float spawnHeight = 2f;

        public void Init()
        {
            StartCoroutine("Delay");
            isSelected = true;
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
            gameManager.startGameEvent += Init;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetCentreOfCamera();
            }
        }

        private void OnGUI()
        {
            if (player != null && (isSelected && player.isLocalPlayerMine))
            {
                for (int i = 0; i < unitSpawner.unitBases.Count; i++)
                {
                    //GUI Buttons
                    if (GUI.Button(new Rect(Screen.width / 20, Screen.height / 15 + Screen.height / 10 * i, 100, 30),
                        unitSpawner.unitBases[i].name))
                    {
                        SpawnUnit(player, unitSpawner.unitBases[i], GetRandomSpawnPoint(unitSpawner.unitBases[i]));
                    }
                }
            }
        }

        Vector3 GetRandomSpawnPoint(UnitBase Ub)
        {
            UnitBase[] unitBases = FindObjectsOfType<UnitBase>();

            Vector3 position = Vector3.zero;

            foreach (var unitBase in unitBases)
            {
                // check Unitbase position distance to center of screen. If close enough then spawn
                if (Vector3.Distance(unitBase.transform.position, GetCentreOfCamera()) >
                    gameManager.unitBuildDistanceThreshold)
                {
                    Bounds b = new Bounds(unitBase.transform.position,new Vector3(spawnRadius,5,spawnRadius));

                    position = unitSpawner.RandomGroundPointInBounds(b, Ub.GetComponent<Collider>().bounds.size);
                    
                    return position;
                }
            }
            


            // float randomPosX = Random.Range(transform.position.x - spawnRadius,
            //     transform.position.x + spawnRadius);
            // float randomPosY = transform.position.y;
            // float randomPosZ = Random.Range(transform.position.z - spawnRadius,
            //     transform.position.z + spawnRadius);

            Vector3 randomPos = new Vector3(position.x, position.y, position.z);
            // return randomPos;
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

        void SpawnUnit(HumanPlayer localPlayer, UnitBase unitToSpawn, Vector3 locationToSpawn)
        {
            // bool localPlayer = FindObjectOfType<PlayerBase>().isLocalPlayer;
            unitSpawner.SpawnUnit(localPlayer.netIdentity, unitToSpawn, locationToSpawn, Quaternion.identity);
        }
    }
}