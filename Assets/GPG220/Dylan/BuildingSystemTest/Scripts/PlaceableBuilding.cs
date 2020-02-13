using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Dylan.BuildingSystemTest.Scripts
{
    [RequireComponent(typeof(Collider)),RequireComponent(typeof(Rigidbody))]
        //Collider needs to be trigger, rigidbody uses no gravity, requires building Tag and building layer
    public class PlaceableBuilding : MonoBehaviour
    {
        [HideInInspector] public List<Collider> colliders = new List<Collider>();

        private bool isSelected;
        private Rigidbody rb;
        private Collider collider;

        public Material cantBuildHere;
        [SerializeField] private Material defaultMat;
        
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