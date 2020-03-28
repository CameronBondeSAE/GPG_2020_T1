using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuildingSystemTest
{
    public class PlaceableBuilding : MonoBehaviour
    {
        [HideInInspector] public List<Collider> colliders = new List<Collider>();

        private bool isSelected;

        public GameObject button;

        private void OnGUI()
        {
            if (isSelected)
            {
                GUI.Box(new Rect(Screen.width / 6, Screen.height / 15, 150, 90), name);
                
                if (GUI.Button(new Rect(Screen.width / 5.5f, Screen.height / 9 , 100, 30), "Do Thing"))
                {
                    
                    TestFunction();
                }
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Building"))
            {
                colliders.Add(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Building"))
            {
                colliders.Remove(other);
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