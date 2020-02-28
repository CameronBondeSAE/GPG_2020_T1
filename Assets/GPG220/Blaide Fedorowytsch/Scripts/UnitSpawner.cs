using System;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class UnitSpawner : MonoBehaviour
    {
        Vector3 position;
        public Vector3 boundrySize;
        public List<UnitBase> unitBases;
        public GameManager gameManager;
        public int spawnNumber = 60;

        public LayerMask SpawnableSurfaces;

        // Start is called before the first frame update
        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.startGameEvent += RandomSpawns;
        }

        public void SpawnUnit(UnitBase unit, Vector3 position,Quaternion rotation)
        {
            Instantiate(unit.gameObject, position, rotation);
        }
        
        //[Button (Name = "RandomSpawn" )]
        public void RandomSpawns()
        {
            for (int i = 0; i < spawnNumber; i++)
            {
                int randIndex = Random.Range(0,unitBases.Count);

                position = RandomGroundPointInBounds(unitExtents(unitBases[randIndex]));
                SpawnUnit( unitBases[randIndex], position, Quaternion.Euler(Vector3.forward));
            }
        }

        public Vector3 unitExtents(UnitBase unit)
        {
            //TODO Figure out a nicer collision check.
           // return unit.gameObject.GetComponent<Collider>().bounds.extents; // this always returns vector3.zero
           return unit.gameObject.GetComponent<Renderer>().bounds.extents; // Not ideal as the physical collider could easily be different to the renderer Bounds... 
           
        }

        public Vector3 RandomGroundPointInBounds(Vector3 Extents)
        {
            bool clear = false;
            int attempts = 0; 
            Vector3 p = transform.position;
            
            Bounds spawnBounds = new Bounds(transform.position,boundrySize);
            while (!clear && attempts <= 30 )
            {
                attempts++;
                float randX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
                float randZ = Random.Range(spawnBounds.min.z, spawnBounds.max.z);
                Vector3 o = new Vector3(randX,spawnBounds.max.y,randZ);
                Ray ray = new Ray(o,Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit,boundrySize.y*3,SpawnableSurfaces,QueryTriggerInteraction.Ignore))
                {
                    Vector3 offsetPosition = hit.point + new Vector3(0,+Extents.y,0);
                    Bounds prespawnCheckBounds = new Bounds( offsetPosition,Extents);


                    while (prespawnCheckBounds.Contains(hit.point))
                    {
                        offsetPosition += Vector3.up *0.1f; 
                        prespawnCheckBounds.center = offsetPosition;
                    }

                    if (!Physics.CheckBox(prespawnCheckBounds.center, prespawnCheckBounds.extents))
                    {
                        p = offsetPosition;
                        clear = true;  
                    }

                }
            }
            return p;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position,boundrySize);
            
        }
    }
}
