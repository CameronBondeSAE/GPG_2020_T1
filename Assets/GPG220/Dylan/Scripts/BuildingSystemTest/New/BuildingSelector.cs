using UnityEngine;

namespace BuildingSystemTest.New
{
    
    public class BuildingSelector : MonoBehaviour
    {
        /// <summary>
        ///  //to use script simply drag prefab button in, rename to appropriate name,
        /// drag in the building placement manager and select the select prefab for button
        /// press function and drag in building you wish to spawn in from that button 
        /// </summary>
        private BuildingPlacementManager buildingPlacementManager;
        
        private void Awake()
        {
            buildingPlacementManager = GetComponent<BuildingPlacementManager>();
        }
        
        public void SelectPrefab(GameObject prefab)
        {
            buildingPlacementManager.SetItem(prefab);
        }

        //old version of script
        // public GameObject[] buildings;
        //
        // public Button[] buildingButton;
        //
        // private void Awake()
        // {
        //     buildingPlacement = GetComponent<BuildingPlacement>();
        // }
        //
        // private void OnGUI()
        // {
        //     for (int i = 0; i < buildings.Length; i++)
        //     {
        //         // if (GUI.Button(new Rect(Screen.width / 20, Screen.height / 15 + Screen.height / 12 * i, 100, 30), buildings[i].name))
        //         // {
        //         //     buildingPlacement.SetItem(buildings[i]);
        //         // }
        //     }
        // }

    }
}
