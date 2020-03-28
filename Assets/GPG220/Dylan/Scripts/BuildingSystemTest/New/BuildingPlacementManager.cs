using UnityEngine;


namespace BuildingSystemTest.New
{
    public class BuildingPlacementManager : MonoBehaviour
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
            
            //New way
            Ray rayCast = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitinfo;

            if (currentBuilding != null && !hasPlaced)
            {
                if (Physics.Raycast(rayCast, out hitinfo, buildingLayer))
                {
                    currentBuilding.transform.position =
                        new Vector3(Mathf.RoundToInt(hitinfo.point.x), Mathf.RoundToInt(hitinfo.point.y) + 0.5f, Mathf.RoundToInt(hitinfo.point.z));
                    currentBuilding.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal);
                }
            }

            //Old Way
            //controls position of building at mouse position
            // Vector3 mPos = Input.mousePosition;
            // mPos = new Vector3(mPos.x, transform.position.y, mPos.y);
            // Vector3 p = camera.ScreenToWorldPoint(mPos);
            // if (currentBuilding != null && !hasPlaced)
            // {
            //     //currentBuilding.position = new Vector3(p.x, 1, p.z);
            //
            // }
            
            //controls clicking on the buildings
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit = new RaycastHit();
                    //the 8 refers to the highest building height, if building is taller increase to desired number
                    //Ray ray = new Ray(new Vector3(p.x, 8, p.z), Vector3.down);
                    if (Physics.Raycast(rayCast, out hit, Mathf.Infinity, buildingLayer))
                    {
                        if (placeableBuildingOld != null)
                        {
                            placeableBuildingOld.SetSelected(false);
                        }
                        
                        hit.collider.gameObject.GetComponent<PlaceableBuilding>().SetSelected(true);
                        placeableBuilding.gameObject.layer = 10;
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
            currentBuilding.gameObject.layer = 2;
            placeableBuilding = currentBuilding.GetComponent<PlaceableBuilding>();
        }
    }
}