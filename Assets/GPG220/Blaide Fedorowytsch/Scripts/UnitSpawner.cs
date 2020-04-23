using System;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class UnitSpawner : NetworkBehaviour
	{
		public Vector3 boundrySize;


		[ReorderableList]
		public List<UnitBase> unitBases;
        public int spawnNumber = 1;
        public LayerMask SpawnableSurfaces;
		
		public void SpawnUnit(NetworkIdentity owner, UnitBase unit, Vector3 position, Quaternion rotation)
        {
            GameObject g = Instantiate(unit.gameObject, position, rotation);

            // Networking
			UnitBase uB = g.GetComponent<UnitBase>();
			uB.owner = owner;
			uB.ownerNetID = owner.netId;

			// Assign ownership of spawned units to client
            NetworkServer.Spawn(g, owner.gameObject);
			// g.GetComponent<NetworkIdentity>().AssignClientAuthority(playerBaseOwner.GetComponent<NetworkIdentity>().connectionToClient);
		}

		//[Button (Name = "RandomSpawn" )]
        public void RandomSpawns(NetworkIdentity owner)
        {
            Vector3 position;
            for (int i = 0; i < spawnNumber; i++)
            {
                int randIndex = Random.Range(0,unitBases.Count);

                position = RandomGroundPointInBounds(new Bounds(transform.position,boundrySize),unitExtents(unitBases[randIndex]));
                SpawnUnit( owner, unitBases[randIndex], position, Quaternion.Euler(Vector3.forward));
            }
        }

		public void SpawnOneOfEach(NetworkIdentity owner)
		{
			Vector3 position;
			for (int clones = 0; clones < spawnNumber; clones++)
			{
				for (int i = 0; i < unitBases.Count; i++)
				{
					position = RandomGroundPointInBounds(new Bounds(transform.position,boundrySize),unitExtents(unitBases[i]));
					SpawnUnit(owner, unitBases[i], position, Quaternion.identity);
				}
			}
		}

        public Vector3 unitExtents(UnitBase unit)
        {
            //TODO Figure out a nicer collision check.
           // return unit.gameObject.GetComponent<Collider>().bounds.extents; // this always returns vector3.zero
           return unit.gameObject.GetComponentInChildren<Renderer>().bounds.extents; // Not ideal as the physical collider could easily be different to the renderer Bounds... 
           
        }

        public Vector3 RandomGroundPointInBounds(Bounds spawnBounds,Vector3 unitExtents)
        {
            bool clear = false;
            int attempts = 0; 
            Vector3 p = transform.position;
            
            
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
                    Vector3 offsetPosition = hit.point + new Vector3(0,+unitExtents.y,0);
                    Bounds prespawnCheckBounds = new Bounds( offsetPosition,unitExtents);


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
