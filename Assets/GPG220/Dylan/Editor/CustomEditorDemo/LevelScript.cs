using UnityEngine;

namespace CustomEditorDemo
{
    public class LevelScript : MonoBehaviour
    {
        public int experience;

    
        public int Level
        {
            get
            {
                return experience / 750;
        
            } 
        }

        public GameObject objectPrefab;
        public Vector3 spawnPoint;
        
        public void BuildObject()
        {
            
            Instantiate(objectPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
