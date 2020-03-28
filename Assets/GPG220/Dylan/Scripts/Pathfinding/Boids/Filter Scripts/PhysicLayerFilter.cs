using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding.Boids.Filter_Scripts
{
    [CreateAssetMenu(menuName = "Flock/Filter/Physics Layer")]
    public class PhysicLayerFilter : ContextFilter
    {
        public LayerMask mask;
        
        public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
        {
            List<Transform> filtered = new List<Transform>();
            foreach (Transform item in original)
            {
                //checks if physics mask and agent are on same layer
                if (mask == (mask | (1 << item.gameObject.layer)))
                {
                    filtered.Add(item);
                }
            }

            return filtered;
        }
    }
}