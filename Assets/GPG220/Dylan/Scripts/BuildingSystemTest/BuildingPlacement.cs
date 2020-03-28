using UnityEngine;

namespace BuildingSystemTest
{
    public class BuildingPlacement : MonoBehaviour
    {
        private Transform currentBuilding;
        [SerializeField] private Camera camera;

        private PlaceableBuilding placeableBuilding;
        private PlaceableBuilding placeableBuildingOld;
        [SerializeField] private LayerMask buildingLayer;
        
        private bool hasPlaced;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void Update()
        {
            
            MoveToMousePosition();
            PlaceBuilding();
        }

        private void MoveToMousePosition()
        {
            Vector3 mPos = Input.mousePosition;
            mPos = new Vector3(mPos.x, mPos.y, transform.position.y);
            Vector3 p = camera.ScreenToWorldPoint(mPos);
            if (currentBuilding != null && !hasPlaced)
            {
                currentBuilding.position = new Vector3(p.x, 0, p.z);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit = new RaycastHit();
                    //the 8 refers to the highest building height, if building is taller increase to desired number
                    Ray ray = new Ray(new Vector3(p.x, 8, p.z), Vector3.down);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingLayer))
                    {
                        if (placeableBuildingOld != null)
                        {
                            placeableBuildingOld.SetSelected(false);
                        }
                        
                        hit.collider.gameObject.GetComponent<PlaceableBuilding>().SetSelected(true);
                        placeableBuildingOld = hit.collider.gameObject.GetComponent<PlaceableBuilding>();
                    }
                    else if (placeableBuildingOld != null)
                    {
                        placeableBuildingOld.SetSelected(false);
                        
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (placeableBuildingOld != null)
                    {
                        placeableBuildingOld.SetSelected(false);
                    }
                }
            }
        }

        private void PlaceBuilding()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CanSpawnHere())
                {
                    hasPlaced = true;
                }
            }
        }

        private bool CanSpawnHere()
        {
            if (placeableBuilding.colliders.Count > 0)
            {
                return false;
            }

            return true;
        }

        public void SetItem(GameObject b)
        {
            hasPlaced = false;

            currentBuilding = ((GameObject) Instantiate(b)).transform;
            placeableBuilding = currentBuilding.GetComponent<PlaceableBuilding>();
        }
    }
}
