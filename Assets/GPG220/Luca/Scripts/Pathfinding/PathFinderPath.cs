using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class PathFinderPath
    {
        public readonly PathFinderController controller;
        
        [NonSerialized, HideInInspector]
        public Dictionary<PathFinderSectorTile, PathFinderSectorTileData> tileDataList = new Dictionary<PathFinderSectorTile, PathFinderSectorTileData>();
        [NonSerialized, HideInInspector]
        public List<PathFinderSectorTileData> tilePath;

        public bool flowFieldAvailable = false;
        public bool continuousFlowField = false;
        
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

        public Vector3 GetDirectionAtPos(Vector3 position, bool flowDirIfAvail = true)
        {
            var tile = controller.GetNearestNode(position);
            if (tile == null)
                return default;
            
            tileDataList.TryGetValue(tile, out var tileData);

            if (tileDataList == null)
                return position-tile.position;

            if (flowFieldAvailable && flowDirIfAvail && tileData != null)
            {
                var dir = tileData.flowFieldDirection;
                if (dir.Equals(Vector3.negativeInfinity))
                {
                    
                    
                    var x = controller.GenerateSurroundingVectorField(tileData, tileData.tile.sector,this, null,
                        controller.proximityFlowFieldRadius);
                    while (x.MoveNext())
                    {
                                
                    }
                    
                }
                
                return dir.Equals(Vector3.negativeInfinity) ? Vector3.zero : dir;
            }

            return position - (GetNextTileOnPath(position)?.GetPosition() ?? tile.position);
        }

        // TODO NOT WORKING YET
        private PathFinderSectorTileData GetNextTileOnPath(Vector3 position)
        {
            Debug.LogError("!!! PathFinderPath.GetNextTileOnPath() Not working yet. Needs impl.");
            /*position.x = Mathf.Abs(position.x);
            position.y = Mathf.Abs(position.y);
            position.z = Mathf.Abs(position.z);*/
        
            PathFinderSectorTileData lastTileData = null;
            var lastTileRelPos = Vector3.zero;
            foreach (var tileData in tilePath)
            {
                var relPos = tileData.GetPosition()-position;
                relPos.x = Mathf.Abs(relPos.x);
                relPos.y = Mathf.Abs(relPos.y);
                relPos.z = Mathf.Abs(relPos.z);
                if (lastTileData == null)
                {
                    lastTileData = tileData;
                    lastTileRelPos = relPos;
                    continue;
                }

                // TODO NOT WORKING YET
                if (position.x >= lastTileData.GetPosition().x &&
                    position.z >= lastTileData.GetPosition().z &&
                    position.x <= tileData.GetPosition().x &&
                    position.z <= tileData.GetPosition().z)
                {
                    return tileData;
                }
                
                lastTileData = tileData;
            }

            return null;
        }

        public float GetAproxDistanceToTargetAtPos(Vector3 position)
        {
            var tile = controller.GetNearestNode(position);
            if (tile == null || !tileDataList.ContainsKey(tile))
                return default;
            var tileData = tileDataList[tile];

            return tileData?.flowFieldDistanceToTarget ?? -1f; // TODO Not working with this kind of flowfields over several sectors...
        }
    }
}
