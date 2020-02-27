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
        public Vector3 position;
        public Vector3 boundryCentre;
        public Vector3 boundrySize;
        public List<UnitBase> unitBases;
        public GameManager gameManager;
        public int spawnNumber = 60;

        public LayerMask SpawnableSurfaces;

        // Start is called before the first frame update
        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            
        }

        public void SpawnUnit(UnitBase unit, Vector3 position,Quaternion rotation)
        {
            Instantiate(unit.gameObject, position, rotation);
            
        }
        
        [Button (Name = "RandomSpawn")]
        public void RandomSpawns()
        {
            for (int i = 0; i < spawnNumber; i++)
            {
                int randIndex = Random.Range(0,unitBases.Count);

                position = RandomPointInBounds(unitExtents(unitBases[randIndex]));
                SpawnUnit( unitBases[randIndex], position, Quaternion.Euler(Vector3.forward));
            }
        }

        public Vector3 unitExtents(UnitBase unit)
        {
            return unit.GetComponent<Collider>().bounds.extents;
        }

        public Vector3 RandomPointInBounds(Vector3 Extents)
        {
            bool clear = false;
            int attempts = 0; 
            Vector3 p = boundryCentre;
            while (!clear && attempts <= 30 )
            {
                attempts++;
                float x = Random.Range(-boundrySize.x, boundrySize.x);
                float z = Random.Range(-boundrySize.z, boundrySize.z);
                Vector3 o = boundryCentre + new Vector3(x,boundrySize.y,z);
                Ray ray = new Ray(o,Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit,boundrySize.y*3,SpawnableSurfaces,QueryTriggerInteraction.Ignore))
                {
                    if (Physics.CheckBox(hit.point + new Vector3(0, + 30, 0),Extents/2))
                    {
                        p = hit.point +  new Vector3(0, 10, 0);
                        clear = true;
                    }
                }
            }
            return p;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(boundryCentre,boundrySize);
            
        }
    }
}
