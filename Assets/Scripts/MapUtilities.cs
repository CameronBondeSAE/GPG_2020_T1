using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapUtilities : MonoBehaviour
{
	public Vector3 boundrySize;
	public LayerMask SpawnableSurfaces;

	private void Start()
	{
		boundrySize = GetComponent<ProceduralMeshGenerator>().mesh.bounds.extents;
	}

	public Vector3 RandomGroundPointInBounds(Bounds spawnBounds, Vector3 unitExtents)
	{
		bool    clear    = false;
		int     attempts = 0;
		Vector3 p        = transform.position;


		while (!clear && attempts <= 30)
		{
			attempts++;
			float      randX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
			float      randZ = Random.Range(spawnBounds.min.z, spawnBounds.max.z);
			Vector3    o     = new Vector3(randX, spawnBounds.max.y, randZ);
			Ray        ray   = new Ray(o, Vector3.down);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, boundrySize.y * 3, SpawnableSurfaces, QueryTriggerInteraction.Ignore))
			{
				Vector3 offsetPosition      = hit.point + new Vector3(0, +unitExtents.y, 0);
				Bounds  prespawnCheckBounds = new Bounds(offsetPosition, unitExtents);


				while (prespawnCheckBounds.Contains(hit.point))
				{
					offsetPosition             += Vector3.up * 0.1f;
					prespawnCheckBounds.center =  offsetPosition;
				}

				if (!Physics.CheckBox(prespawnCheckBounds.center, prespawnCheckBounds.extents))
				{
					p     = offsetPosition;
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