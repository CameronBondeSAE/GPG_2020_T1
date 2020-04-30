using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEvents
{
    // Start is called before the first frame update
    public static event Action<WorldPosAndBounds> PathFindingObstacleChange;

    public static void OnPathFindingObstacleChange(WorldPosAndBounds obj)
    {
        PathFindingObstacleChange?.Invoke(obj);
    }
}
public struct WorldPosAndBounds
{
    public Vector3 worldPos;
    public Bounds bounds;

    public WorldPosAndBounds(Vector3 worldPos, Bounds bounds)
    {
        this.worldPos = worldPos;
        this.bounds = bounds;
    }
}
