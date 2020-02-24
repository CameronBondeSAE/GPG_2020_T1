using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class PathFinderSectorTileData : IEquatable<PathFinderSectorTileData>
    {
        public PathFinderSectorTile tile;

        public PathFinderSectorTileData(PathFinderSectorTile tile)
        {
            this.tile = tile;
        }
    
        // A* Data
        private float _gCost = 0; // Dist to start

        public float GCost
        {
            get => _gCost;
            set
            {
                _gCost = value;
                fCost = GCost + HCost;
            }
        }

        private float _hCost = 0; // Dist to end

        public float HCost
        {
            get => _hCost;
            set
            {
                _hCost = value;
                fCost = HCost + GCost;
            }
        }

        public float fCost = -1; // gCost + hCost
        public PathFinderSectorTile lastTile;
        public PathFinderSectorTileData lastTileData;
        
        public readonly List<PathFinderSectorTileData> neighbourTiles = new List<PathFinderSectorTileData>();
    
        // FlowField stuff
        public float flowFieldDistanceToTarget = -1f;
        public Vector3 flowFieldDirection = Vector3.negativeInfinity;
        public PathFinderSectorTile flowFieldLastTile;
        public PathFinderSectorTileData flowFieldLastTileData;

        public PathFinderSectorTileData flowFieldSurroundingTargetTileData;
    
        // TMP Debug stuff
        public bool isRed = false;
        public bool isGreen = false;
        public bool isStart = false;
        public bool isEnd = false;
        public bool isPath = false;

        public bool vecDirDrawed = false; // TODO DELETE

        public Vector3 GetPosition()
        {
            return tile?.position ?? Vector3.negativeInfinity;
        }
    
        public static float CalculateHCost(PathFinderSectorTile fromTile, PathFinderSectorTile toTile)
        {
            return Vector3.Distance(fromTile.position, toTile.position);
        }
    
        public override bool Equals(object obj)
        {
            var other = (PathFinderSectorTileData) obj;
            if (ReferenceEquals(tile, null)) return false;
            if (ReferenceEquals(other, null)) return false;
            return tile.Equals(other.tile);
        }

        public override int GetHashCode()
        {
            if (ReferenceEquals(tile, null)) return 0;
            unchecked
            {
                return ((tile.sector != null ? tile.sector.GetHashCode() : 0) * 397) ^ tile.position.GetHashCode();
            }
        }

        public bool Equals(PathFinderSectorTileData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(null, tile)) return false;
            //if (ReferenceEquals(this, other)) return true;
            return tile.Equals(other.tile);
        }
    }
}
