using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class PathFinderFlowField : ICloneable
    {
        /// <summary>
        /// Unpassable = Vector3.negativeInfinity
        /// FreeToSet = Vector3.zero
        /// </summary>

        public int[][] costField;
        public Vector3[][] flowField;
        public float tileSize;
        public int fieldSizeX;
        public int fieldSizeZ;
        
        public Vector3 startPosition;
        public Vector3 targetPosition;

        public PathFinderFlowField(float tileSize, int sizeX, int sizeZ)
        {
            flowField = Enumerable.Repeat(Enumerable.Repeat(Vector3.negativeInfinity, sizeZ).ToArray(),sizeX).ToArray(); //new Vector3[sizeX,sizeZ];   
        }
        
        // TMP HACK
        public List<PathFinderSectorTile> clones = new List<PathFinderSectorTile>();
        
        
        public Color32[] colors = new Color32[15];
        
        public void GenerateHeatmap(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path)
        {
            if (currentTileData.GetPosition() == targetPosition)
            {
                currentTileData.flowFieldDistanceToTarget = 0;
            }
            
            var neighboursToEvaluate = new List<PathFinderSectorTileData>();
            currentTileData.tile.neighbourTiles?.ForEach(neighbourTile =>
            {
                if (neighbourTile == null || (sector != null && neighbourTile.sector != sector)) return;
                path.tileDataList.TryGetValue(neighbourTile, out var neighbourTileData);
                if (neighbourTileData == null)
                {
                    neighbourTileData = new PathFinderSectorTileData(neighbourTile);
                    path.AddTileData(neighbourTileData);
                }
                
                var distToTargetNotSetYet = neighbourTileData.flowFieldDistanceToTarget < 0;
                if (distToTargetNotSetYet || neighbourTileData.flowFieldDistanceToTarget > currentTileData.flowFieldDistanceToTarget + 1)
                {
                    neighbourTileData.flowFieldLastTile = currentTileData.tile;
                    neighbourTileData.flowFieldLastTileData = currentTileData;
                    neighbourTileData.flowFieldDistanceToTarget = currentTileData.flowFieldDistanceToTarget + 1;
                    neighboursToEvaluate.Add(neighbourTileData);
                }
            });

            neighboursToEvaluate.ForEach(neighbourTileData =>
            {
                if (neighbourTileData == null || (sector != null && neighbourTileData.tile.sector != sector)) return;
                GenerateHeatmap(neighbourTileData, sector, path);
            });

        }
        
        public bool ContainsPoint(Vector3 point)
        {
            return true; // TODO
        }

        public object Clone()
        {
            var clone = new PathFinderFlowField(tileSize, fieldSizeX, fieldSizeZ) {flowField = flowField, targetPosition = targetPosition};

            return clone;
        }
    }
}