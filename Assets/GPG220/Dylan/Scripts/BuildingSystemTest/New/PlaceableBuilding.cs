using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystemTest.New
{
    [RequireComponent(typeof(Collider)),RequireComponent(typeof(Rigidbody))]
    //Collider needs to be trigger, rigidbody uses no gravity, requires building Tag and building layer
    //make colliders slightly smaller than model
    public class PlaceableBuilding : MonoBehaviour
    {
        /// <summary>
        /// this script is put on any building that wishes to be placed in the game,
        /// all settings, for collider and rigidbody are set on awake so no need to change anything,
        /// however you need to add the cant build material for the building
        /// </summary>
        [HideInInspector] public List<Collider> colliders = new List<Collider>();
        
        private bool isSelected;
        private Rigidbody rb;
        private Collider collider;

        public Material cantBuildHere;
        private Material defaultMat;
        
        private void Awake()
        {
            //should auto set each required setting without needed to change in editor
            gameObject.tag = "Building";
            gameObject.layer = 10;
            rb = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            rb.useGravity = false;
            collider.isTrigger = true;
            defaultMat = GetComponent<MeshRenderer>().sharedMaterial;
        }

        //was test for allowing building selection early on
        private void OnGUI()
        {
            // if (isSelected)
            // {
            //     GUI.Box(new Rect(Screen.width / 6, Screen.height / 15, 150, 90), name);
            //     
            //     if (GUI.Button(new Rect(Screen.width / 5.5f, Screen.height / 9 , 100, 30), "Do Thing"))
            //     {
            //         
            //         TestFunction();
            //     }
            // }
        }

        //ensures player can't spawn building inside each other
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Building"))
            {
                colliders.Add(other);
                
                GetComponent<MeshRenderer>().sharedMaterial = cantBuildHere;
                
                Debug.Log("Can't Build Here");
            }

            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Building"))
            {
                colliders.Remove(other);
                GetComponent<MeshRenderer>().sharedMaterial = defaultMat;
                Debug.Log("Can Build Here");

            }
        }

        public void SetSelected(bool selected)
        {
            isSelected = selected;
        }

        private void TestFunction()
        {
            Debug.Log("Huh");
        }
    }
}