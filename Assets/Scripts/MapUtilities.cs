using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.ProcGen;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapUtilities : MonoBehaviour
{
	public Vector3   boundrySize;
	public LayerMask SpawnableSurfaces;
	public LayerMask EmptySpaceSurfacesToIgnore;

	public bool debugEmptySpace = false;

	private void Start()
	{
		boundrySize = GetComponent<ProceduralMeshGenerator>().mesh.bounds.extents;
	}

	private void Update()
	{
		if (debugEmptySpace)
		{
			TestRandomGroundPointInBounds();
		}
	}

	private void TestRandomGroundPointInBounds()
	{
		for (int i = 0; i < 1000; i++)
		{
			Vector3 pos = RandomGroundPointInBounds(GetComponent<ProceduralMeshGenerator>().mesh.bounds, new Vector3(1f,1f,1f));
		}
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
			Vector3    o     = transform.position + new Vector3(randX, spawnBounds.max.y, randZ);
			Ray        ray   = new Ray(o, Vector3.down);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, boundrySize.y + 10f, SpawnableSurfaces, QueryTriggerInteraction.Ignore))
			{
				Vector3 offsetPosition      = hit.point + new Vector3(0, +unitExtents.y, 0);
				// Bounds  prespawnCheckBounds = new Bounds(offsetPosition, unitExtents);


				// while (prespawnCheckBounds.Contains(hit.point))
				// {
					// offsetPosition             += Vector3.up * 0.1f;
					// prespawnCheckBounds.center =  offsetPosition;
				// }

				// if (!Physics.CheckBox(prespawnCheckBounds.center, prespawnCheckBounds.extents))
				if (!Physics.CheckBox(offsetPosition, unitExtents, Quaternion.identity, EmptySpaceSurfacesToIgnore))
				{
					p     = offsetPosition;
					clear = true;
					if (debugEmptySpace)
					{
						Debug.DrawLine(ray.origin, ray.origin + Vector3.up*10f, Color.green);
					}
				}
				else
				{
					if (debugEmptySpace)
					{
						Debug.DrawLine(ray.origin, ray.origin + Vector3.up * 10f, Color.red);
					}
				}
			}
		}

		return p;
	}
}