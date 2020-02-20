using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Loids
{
   
    public class Neighbour : MonoBehaviour
    {
        public float far,near;   
        
        public List<Neighbour> neighbours;

        public Vector3 averageOfNeighbourPositions()
        {
            Vector3 runningTotal = Vector3.zero;
            foreach (Neighbour neighbour in neighbours)
            {
                runningTotal += neighbour.transform.position;
            }
            return runningTotal / neighbours.Count;
        }

        public Quaternion averageOfNeighbourRotations()
        {
            //TODO Actually find the average rotation of neighbours,.. I hate quaturnions and their dumb eulers.
            return Quaternion.Euler(Vector3.zero);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Neighbour>())
            {
                Neighbour neighbourino = other.gameObject.GetComponent<Neighbour>();
                neighbours.Add(neighbourino);  
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<Neighbour>())
            {
                Neighbour neighbourino = other.gameObject.GetComponent<Neighbour>();
                neighbours.Remove(neighbourino);
            }
        }
    }
}
