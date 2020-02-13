using System;
using UnityEngine;

namespace BuildingSystemTest
{
    public class BuildingSystemTest : MonoBehaviour
    {
        [SerializeField] private GameObject[] placeableObjectPrefabs;
        private int currentPlaceablePrefab;

        private GameObject currentPlaceableObject;

        private float mouseWheelRotation;
        private int currentPrefabIndex = 1;

        public Camera camera;

        public LayerMask buildOnLayer;

        //[SerializeField] private Vector3 checkRadius;
        [SerializeField] private float checkRadius = 1.5f;

        private Collider[] canSpawnColliders;
        [SerializeField] private bool canSpawn;

        [SerializeField] private Material cantPlaceBuildingMat;
        private Material defaultMat;

        private void Update()
        {
            CanBuildingSpawn();
            HandleNewObject();
            if (currentPlaceableObject != null)
            {
                MoveCurrentObjectToMouse();
                RotateFromMouseWheel();
                ReleaseIfClicked();
            }
        }

        private void HandleNewObject()
        {
            for (int i = 0; i < placeableObjectPrefabs.Length; i++)
            {
                //can have around 9 buildings for testing since 
                if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
                {
                    if (PressedKeyOfCurrentPrefab(i))
                    {
                        Destroy(currentPlaceableObject);
                        currentPrefabIndex = i;
                    }
                    else
                    {
                        if (currentPlaceableObject != null)
                        {
                            Destroy(currentPlaceableObject);
                        }

                        if (canSpawn)
                        {
                            currentPlaceableObject = Instantiate(placeableObjectPrefabs[i]);
                            defaultMat = currentPlaceableObject.GetComponent<MeshRenderer>().sharedMaterial;
                        }

                        currentPrefabIndex = i;
                    }

                    break;
                }

                if (!canSpawn && currentPlaceableObject != null)
                {
                    //Destroy(currentPlaceableObject);
                    currentPlaceableObject.GetComponent<MeshRenderer>().sharedMaterial = cantPlaceBuildingMat;
                    Debug.Log("Can't Build Here");
                    break;
                }

                if (currentPlaceableObject != null)
                {
                    currentPlaceableObject.GetComponent<MeshRenderer>().sharedMaterial = defaultMat;
                }
            }
        }
        
        

        private bool PressedKeyOfCurrentPrefab(int a)
        {
            return currentPlaceableObject != null && currentPrefabIndex == a;
        }

        private void MoveCurrentObjectToMouse()
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitinfo;

            if (Physics.Raycast(ray, out hitinfo, buildOnLayer))
            {
                currentPlaceableObject.transform.position =
                    new Vector3(hitinfo.point.x, hitinfo.point.y + 0.5f, hitinfo.point.z);
                currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal);
            }
        }

        private void CanBuildingSpawn()
        {
            canSpawn = true;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit canSpawnRaycast;

            if (Physics.Raycast(ray, out canSpawnRaycast))
            {
                //change to box if wishing to use vector3's
                canSpawnColliders = Physics.OverlapSphere(canSpawnRaycast.point, checkRadius);
            }

            foreach (Collider col in canSpawnColliders)
            {
                canSpawn = !col.CompareTag("Building");
            }
        }

        private void RotateFromMouseWheel()
        {
            mouseWheelRotation += Input.mouseScrollDelta.y;
            currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
        }

        private void ReleaseIfClicked()
        {
            if (Input.GetMouseButtonDown(0) && canSpawn)
            {
                if (!canSpawn)
                {
                    Destroy(currentPlaceableObject);
                    Debug.Log("Can't Build Here");
                }

                currentPlaceableObject.GetComponent<Collider>().enabled = true;
                currentPlaceableObject = null;
            }
        }
    }
}