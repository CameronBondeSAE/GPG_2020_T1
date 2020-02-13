using BuildingSystemTest;
using UnityEngine;
using UnityEngine.UI;

namespace GPG220.Dylan.BuildingSystemTest.Scripts
{
    public class BuildingManager : MonoBehaviour
    {
        public GameObject[] buildings;

        private BuildingPlacement buildingPlacement;

        public Button[] buildingButton;

        private void Awake()
        {
            buildingPlacement = GetComponent<BuildingPlacement>();
        }

        private void OnGUI()
        {
            for (int i = 0; i < buildings.Length; i++)
            {
                // if (GUI.Button(new Rect(Screen.width / 20, Screen.height / 15 + Screen.height / 12 * i, 100, 30), buildings[i].name))
                // {
                //     buildingPlacement.SetItem(buildings[i]);
                // }
                foreach (Button button in buildingButton)
                {
                    
                }
            }
        }
    }
}
