using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralMeshGenerator : MonoBehaviour
{
    public bool showVerts = false;
    private Vector3[] verticies;
    private int[] triangles;
    private Mesh mesh;
    public Vector2Int meshSize = new Vector2Int(20,20);
    public Vector2 seed = new Vector2(0.3145f,0.1232f);
    public float MeshConstructionDelay = 0.0001f;
    public float heightMultiplier;
    public float bumpiness;
    
    private void Start()
    {
        mesh = new Mesh();
        mesh.name = "generatedTerrain";
        GetComponent<MeshFilter>().mesh = mesh;
        StartCoroutine(generateMesh());
    }

    private void Update()
    {
        UpdateMesh();
    }
    [Button(Name = "Regenerate Mesh")]
    void RegenerateMesh()
    {
        StartCoroutine(generateMesh());
    }

    IEnumerator generateMesh()
    {
        verticies = new Vector3[(meshSize.x + 1) * (meshSize.y + 1)];

        
        for (int i = 0, y = 0; y <= meshSize.y; y++)
        {
            for (int x = 0; x <= meshSize.x; x++)
            {
                float vertHeight = Mathf.PerlinNoise((seed.x + x) * bumpiness, (seed.y + y)* bumpiness) * heightMultiplier;
                
                verticies[i] = new Vector3(x,vertHeight,y);
                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        triangles = new int[meshSize.x * meshSize.y *6];
        for (int y = 0; y < meshSize.y; y++)
        {
            for (int x = 0; x < meshSize.x; x++)
            {
                triangles[tris + 0] =  vert + 0;
                triangles[tris + 1] =  vert + meshSize.x + 1;
                triangles[tris + 2] =  vert + 1;
                triangles[tris + 3] =  vert + 1;
                triangles[tris + 4] =  vert + meshSize.x + 1;
                triangles[tris + 5] =  vert + meshSize.x + 2;
                vert++;
                tris += 6;
            }
            vert++;
            yield return  new WaitForSeconds(MeshConstructionDelay);
        }
        
    }

    public float GetHeightAtPosition(Vector2 v)
    {
        return Mathf.PerlinNoise((seed.x + v.x) * bumpiness, (seed.y + v.y)* bumpiness) * heightMultiplier;
    }
    
    [Button (Name = "test GetHeightAtPosition", Style = ButtonStyle.FoldoutButton)]
    void TestHeightAtPosition(Vector2 v)
    {
        Debug.Log("Height At position: " + v + " = " + GetHeightAtPosition(v));
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        Vector2[] uvs = new Vector2[verticies.Length];
        int j = 0;
        while (j < uvs.Length) {
            uvs[j] = new Vector2(verticies[j].z/meshSize.y, verticies[j].x / meshSize.x);
            j++;
        }
        mesh.uv = uvs;

    }

    private void OnDrawGizmos()
    {
        if (verticies != null && showVerts)
        {
            for (int i = 0; i < verticies.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(verticies[i], 0.1f);
            }
        }
    }
}
