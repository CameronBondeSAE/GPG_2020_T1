using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.ProcGen
{
    public class ProceduralMeshGenerator : MonoBehaviour
    {
        public bool showVerts = false;
        private Vector3[] verticies;
        private int[] triangles;
		public Mesh mesh;
        public Vector2Int meshResolution = new Vector2Int(20,20);
        public Vector2 worldSize = new Vector2Int(20,20);
        public Vector2 seed = new Vector2(0.3145f,0.1232f);
        public float MeshConstructionDelay = 0.0001f;
        public float heightMultiplier;
        public float bumpiness;
        public MeshCollider collider;
        public GameManager gameManager;

        private bool meshgenerated = false;

        public bool DrawGizmos = false;
    
        private void Start()
        {
           // gameManager.startGameEvent += generateMesh;
            mesh = new Mesh();
            mesh.name = "generatedTerrain";
            GetComponent<MeshFilter>().mesh = mesh;
            GenerateMesh();

        }


        [Button(Name = "Regenerate Mesh")]
        void RegenerateMesh()
        {
            GenerateMesh();
        }
        void GenerateMesh()
        {
            verticies = new Vector3[(meshResolution.x + 1) * (meshResolution.y + 1)];

        
            for (int i = 0, y = 0; y <= meshResolution.y; y++)
            {
                for (int x = 0; x <= meshResolution.x; x++)
                {
                    float vertHeight = Mathf.PerlinNoise((seed.x + x) * bumpiness, (seed.y + y)* bumpiness) * heightMultiplier;
                
                    verticies[i] = new Vector3(x*((worldSize.x)/meshResolution.x),vertHeight,y * ((worldSize.y)/meshResolution.y));
                    i++;
                }
            }
            int vert = 0;
            int tris = 0;
            triangles = new int[meshResolution.x * meshResolution.y *6];
            for (int y = 0; y < meshResolution.y; y++)
            {
                for (int x = 0; x < meshResolution.x; x++)
                {
                    triangles[tris + 0] =  vert + 0;
                    triangles[tris + 1] =  vert + meshResolution.x + 1;
                    triangles[tris + 2] =  vert + 1;
                    triangles[tris + 3] =  vert + 1;
                    triangles[tris + 4] =  vert + meshResolution.x + 1;
                    triangles[tris + 5] =  vert + meshResolution.x + 2;
                    vert++;
                    tris += 6;
                }
                vert++;
            }
            UpdateMesh();
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
			mesh.RecalculateBounds();

			Vector2[] uvs = new Vector2[verticies.Length];
            int j = 0;
            while (j < uvs.Length) {
                uvs[j] = new Vector2(verticies[j].z/meshResolution.y, verticies[j].x / meshResolution.x);
                j++;
            }
            mesh.uv = uvs;
            // collider.sharedMesh = null;
            collider.sharedMesh = mesh;

        }

        private void OnDrawGizmos()
        {
            if (verticies != null && showVerts && DrawGizmos)
            {
                for (int i = 0; i < verticies.Length; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(transform.position + verticies[i], 2f);
                }
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position + new Vector3(worldSize.x ,20,worldSize.y)*0.5f, new Vector3(worldSize.x,30,worldSize.y));
        }
    }
}
