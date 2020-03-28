using System.Collections;
using System.Collections.Generic;
using Pathfinding.Boids;
using UnityEngine;

public abstract class FilteredFlockBehaviour : FlockBehaviour
{
    public ContextFilter filter;
}
