using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class PathFinderPath
    {
        public Dictionary<PathFinderSectorTile, PathFinderSectorTileData> tileData = new Dictionary<PathFinderSectorTile, PathFinderSectorTileData>();
        public List<PathFinderSectorTileData> tilePath;
        
        
        // TODO? MAYBE DELETE
        public List<Vector3> waypoints;
        public List<PathFinderFlowField> flowFields;


        public bool AddTileData(PathFinderSectorTileData data, bool overrideExisting = false)
        {
            if (data?.tile == null || (tileData.ContainsKey(data.tile) && overrideExisting == false))
                return false;

            if (tileData.ContainsKey(data.tile))
            {
                tileData[data.tile] = data;
            }
            else
            {
                tileData.Add(data.tile, data);
            }

            return true;
        }
    }
}
