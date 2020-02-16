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
        
        public void GenerateHeatmap(PathFinderSectorTile currentTile, PathFinderSector sector)
        {
            if (currentTile.position == targetPosition)
            {
                currentTile.flowFieldDistanceToTarget = 0;
            }
            
            //Debug.Log("GenHeat... Pos: "+currentTile.position+" neighbours1: "+currentTile.neighbourTiles.Count+" sector: "+currentTile.sector.bounds);
            ////////////// TODO UGLY HACKY
            // Clone neighbour tiles
            //currentTile.neighbourTiles = currentTile.neighbourTiles.Select(nt => (PathFinderSectorTile)nt.Clone()).ToList();
            var newNeighbourTilesList = new List<PathFinderSectorTile>();
            currentTile.neighbourTiles.ForEach(nt =>
            {
                if (nt == null)
                    return;

                if (nt.isTempCopy)
                {
                    if(!clones.Contains(nt))
                        clones.Add(nt);
                    newNeighbourTilesList.Add(nt);
                    return;
                }

                if (clones.Contains(nt))
                {
                    newNeighbourTilesList.Add(clones[clones.IndexOf(nt)]);
                    return;
                }
                
                
                var ntClone = (PathFinderSectorTile) nt.Clone();
                newNeighbourTilesList.Add(ntClone);
                clones.Add(ntClone);
            });
            currentTile.neighbourTiles = newNeighbourTilesList;
            //Debug.Log("GenHeat... Pos: "+currentTile.position+" neighbours2: "+currentTile.neighbourTiles.Count+" sector: "+currentTile.sector.bounds);
            //////////////
            
            currentTile.neighbourTiles?.ForEach(tile =>
            {
                //Debug.Log(" ===== > Neighbour1 "+" sector: "+tile.sector.bounds);
                if (tile == null || !(tile.flowFieldDistanceToTarget < 0) || (sector != null && tile.sector != sector)) return;
                //Debug.Log(" ===== > Neighbour2 "+" sector: "+tile.sector.bounds);
                tile.flowFieldLastNode = currentTile;
                tile.flowFieldDistanceToTarget = currentTile.flowFieldDistanceToTarget + 1;
                GenerateHeatmap(tile, sector);
            });

            //yield return null;
        }
        
        //public List<PathFinderSectorTile> TMPTSTDEBUGTILE = new List<PathFinderSectorTile>();

        public IEnumerator GenerateVectorField(PathFinderSectorTile currentTile, PathFinderSector sector)
        {
            if (currentTile == null)
                yield break;
            
            var dirVector = new Vector3(currentTile.GetLeftTile()?.flowFieldDistanceToTarget ?? 0 - currentTile.GetRightTile()?.flowFieldDistanceToTarget ?? 0, 0, currentTile.GetTopTile()?.flowFieldDistanceToTarget ?? 0 - currentTile.GetBottomTile()?.flowFieldDistanceToTarget ?? 0);
            currentTile.flowFieldDirection = dirVector.normalized;
            
            
            var rayStartPos = currentTile.position;
            rayStartPos.y += 0.5f;
            //Debug.Log("GEN VEC FIELD Pos: "+currentTile.position+" Vector: "+dirVector+" neighbours: "+currentTile.neighbourTiles.Count);
            Debug.DrawRay(rayStartPos, currentTile.flowFieldDirection, Color.red, 30f);
            yield return new WaitForEndOfFrame();
            currentTile.neighbourTiles?.ForEach(tile =>
            {
                //Debug.Log("Wus.. "+ (tile == null)+" - "+(sector != null && tile.sector != sector)+" - "+(currentTile.flowFieldDirection != Vector3.zero)+" - "+(tile.Equals(currentTile)));
                if (tile == null || (sector != null && tile.sector != sector) || tile.flowFieldDirection != Vector3.zero) return;
                GenerateVectorField(tile, sector);
            });
            
            yield return null;
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