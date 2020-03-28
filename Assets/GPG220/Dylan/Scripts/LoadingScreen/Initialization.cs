using System;
using System.Collections;
using UnityEngine;

namespace LoadingScreen
{
    public class Initialization : MonoBehaviour
    {
        public static Initialization current;

        public float progress;
        public bool isDone;
        
        private void Awake()
        {
            current = this;
        }

        IEnumerator Spawn()
        {
            //int count = 0;
            //use progress divided by the things your spawning in for the float
            //count is the current interation and thingsThatAreSpawning is the total your spawning in
            //progress = ((float)count / (float)thingsThatAreSpawning);    
            
            yield return new WaitForSeconds(0.5f);
            isDone = true;
        }
        
        
    }
}
