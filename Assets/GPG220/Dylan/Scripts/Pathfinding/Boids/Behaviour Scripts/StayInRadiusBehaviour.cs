using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Boids.Behaviour_Scripts
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/Stay InRadius")]
    public class StayInRadiusBehaviour : FlockBehaviour
    {
        public Vector3 center;
        public float radius = 15f;


        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            Vector3 centerOffset = center - agent.transform.position;
            float t = centerOffset.magnitude / radius;
            if (t < 0.9)
            {
                return Vector3.zero;
            }

            return centerOffset * t * t;
        }
    }
}