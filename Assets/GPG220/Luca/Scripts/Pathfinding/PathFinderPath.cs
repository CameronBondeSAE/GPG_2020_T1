using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class PathFinderPath
    {
        public PathFinderController controller;
        
        [NonSerialized, HideInInspector]
        public Dictionary<PathFinderSectorTile, PathFinderSectorTileData> tileDataList = new Dictionary<PathFinderSectorTile, PathFinderSectorTileData>();
        [NonSerialized, HideInInspector]
        public List<PathFinderSectorTileData> tilePath;

        public bool flowFieldAvailable = false;
        
        // TODO? MAYBE DELETE
        public List<Vector3> waypoints;
        public List<PathFinderFlowField> flowFields;

        public PathFinderPath(PathFinderController pfController)
        {
            controller = pfController;
        }

        public bool AddTileData(PathFinderSectorTileData data, bool overrideExisting = false)
        {
            if (data?.tile == null || (tileDataList.ContainsKey(data.tile) && overrideExisting == false))
                return false;

            if (tileDataList.ContainsKey(data.tile))
            {
                tileDataList[data.tile] = data;
            }
            else
            {
                tileDataList.Add(data.tile, data);
            }

            return true;
        }

        public Vector3 GetFlowDirectionAtPos(Vector3 position)
        {
            var tile = controller.GetNearestNode(position);
            if (tile == null || !tileDataList.ContainsKey(tile))
                return default;
            var tileData = tileDataList[tile];

            return tileData?.flowFieldDirection ?? default;
        }

        public float GetAproxDistanceToTargetAtPos(Vector3 position)
        {
            var tile = controller.GetNearestNode(position);
            if (tile == null || !tileDataList.ContainsKey(tile))
                return default;
            var tileData = tileDataList[tile];

            return tileData?.flowFieldDistanceToTarget ?? -1f;
        }
    }
}
