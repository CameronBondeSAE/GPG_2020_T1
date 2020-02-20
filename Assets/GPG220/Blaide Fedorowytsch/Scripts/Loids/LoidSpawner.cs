using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LoidSpawner : MonoBehaviour
{
    public GameObject loidPrefab;
    public int loidSpawnbuffer;
    public float timesinceLastSpawn = 0;
    public float spawnDelay;

    public List<GameObject> spawnedLoids;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loidSpawnbuffer > 0)
        {
            if (timesinceLastSpawn <= spawnDelay)
            {
                timesinceLastSpawn += Time.deltaTime;
            }
            else
            {
                timesinceLastSpawn = 0;
                spawnLoid();
                loidSpawnbuffer--;
            }
        }
    }

    [Button(Name = "SpawnLoid")]
    void spawnLoid()
    {
        float x, y, z;
        x = Random.Range(0, 360);
        y = Random.Range(0, 360);
        z = Random.Range(0, 360);
        
        Quaternion randomRotation = Quaternion.Euler(new Vector3(x,y,z));
        GameObject gameObject = Instantiate(loidPrefab, transform.position, randomRotation);
        spawnedLoids.Add(gameObject);

    }
    [Button(Style = ButtonStyle.FoldoutButton, Name = "spawn Multiple Loids")]
    void SpawnMultipleLoids(int ammount, float delay)
    {
        spawnDelay = delay;
        loidSpawnbuffer += ammount;

    }

    [Button(Name = "destroy all spawned Loids")]
    void destroyAllSpawnedLoids()
    {
        int loidCount = spawnedLoids.Count;
        for (int i = 0; i < loidCount; i++)
        {
            Destroy(spawnedLoids[i]);
        }
        spawnedLoids.Clear();
        
    }

}
