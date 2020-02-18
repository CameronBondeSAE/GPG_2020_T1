using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GPG220.Luca.Scripts.Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PathFinderSector : MonoBehaviour
{
    public PathFinderSectorTile[,] sectorTileGrid;
    
    public LayerMask walkableMask;
    public LayerMask ignoreMask;
    
    
    public float sectorTileSize = 1;
    public float sectorFlowFieldTileSize = 1;
    
    public Bounds bounds;

    public List<PathFinderSector> connectedSectors = new List<PathFinderSector>();

    public GameObject tileObjectPrefab;
    
    [ShowInInspector]
    public List<PathFinderSectorTile> sectorTiles;
    [ShowInInspector]
    public List<PathFinderSectorTile> borderTiles; // TODO needed? Where? how?
    [ShowInInspector]
    public List<PathFinderTileObject> borderTileObjects;

    public bool doDebug = false;
    public Color32 debugSectorColor = new Color32(0, 255, 255, 100);
    public bool debugDrawTiles = false;

    public PathFinderFlowField pathFinderFlowFieldTemplate;
    
    public void CreateSectorTileGrid()
    {
        borderTiles = new List<PathFinderSectorTile>(); // TODO needed? Where? how?
        sectorTiles = new List<PathFinderSectorTile>();
        if (borderTileObjects != null)
        {
            borderTileObjects.ForEach(bto => DestroyImmediate(bto?.gameObject));
            borderTileObjects.Clear();
        }else
            borderTileObjects = new List<PathFinderTileObject>();

        var floatRowsX = (bounds.size.x / sectorTileSize);
        var floatRowsZ = (bounds.size.z / sectorTileSize);
        
        var rowsX = (int)floatRowsX;
        var rowsZ = (int)floatRowsZ;
        
        pathFinderFlowFieldTemplate = new PathFinderFlowField(sectorTileSize, rowsX, rowsZ);
/*

        var specialSizeX = floatRowsX - rowsX;
        var specialSizeZ = floatRowsZ - rowsZ;*/
        
        var currentPos = transform.position;
        currentPos.x -= bounds.extents.x - (sectorTileSize/2);
        //currentPos.x += sectorTileSize/2;
        var zOrigin = currentPos.z - bounds.extents.z + (sectorTileSize/2);
        //var zOrigin = currentPos.z + (sectorTileSize/2);
        currentPos.z = zOrigin;
        currentPos.y += bounds.extents.y;
        
        var tiles = new PathFinderSectorTile[rowsX,rowsZ];
        var checkBoxExtents = new Vector3(sectorTileSize/2, sectorTileSize/2, sectorTileSize/2);
        // Create Nodes
        for (var x = 0; x < rowsX; x++)
        {
            for (var z = 0; z < rowsZ; z++)
            {
                /*var tileSize = sectorTileSize;
                var tileSizeX = sectorTileSize;
                var tileSizeZ = sectorTileSize;
                
                
                if (x == rowsX - 1 && specialSizeX > 0)
                    tileSizeX = specialSizeX;
                if (z == rowsZ - 1 && specialSizeZ > 0)
                    tileSizeZ = specialSizeZ;*/
                
                //Debug.DrawLine(currentPos, currentPos + Vector3.down * bounds.size.y, Color.red, 2f);
                
                if (Physics.SphereCast(currentPos, sectorTileSize/2, Vector3.down, out var hit,
                    bounds.size.y, walkableMask))
                {
                    var startPoint = hit.point;
                    startPoint.y += sectorTileSize/2;
                    //Debug.DrawLine(startPoint, startPoint + Vector3.down * 5, Color.yellow, 2f);
                    // Obstacle Check
                    if (!Physics.CheckBox(startPoint, checkBoxExtents, Quaternion.identity, ~(walkableMask | ignoreMask))) // !Physics.CheckSphere(hit.point, sectorTileSize, ~(walkableMask | ignoreMask))
                    {
                        var tile = new PathFinderSectorTile
                        {
                            position = hit.point,
                            tileRect = new Rect(hit.point.x, hit.point.z, sectorTileSize, sectorTileSize),
                            sector = this
                        };
                        tiles[x, z] = tile;

                        pathFinderFlowFieldTemplate.flowField[x][z] = Vector3.zero;
                        
                        sectorTiles.Add(tile);
                    
                        // TODO needed?
                        if (x == 0 || x == rowsX-1 || z == 0 || z == rowsZ-1)
                        {
                            borderTiles.Add(tile);
                            if (tileObjectPrefab != null)
                            {
                                var go = Instantiate(tileObjectPrefab, transform);
                                var pfto = go.GetComponent<PathFinderTileObject>() ?? go.AddComponent<PathFinderTileObject>();
                                pfto.transform.position = hit.point;
                                pfto.sector = this;
                                pfto.Tile = tile;
                                pfto.InitObject();
                                borderTileObjects.Add(pfto);
                            }
                            
                        }
                    }
                }
                

                currentPos.z += sectorTileSize;
            }
            currentPos.z = zOrigin;
            currentPos.x += sectorTileSize;
        }
        
        // Add Neighbours
        for (var x = 0; x < rowsX; x++)
        {
            for (var z = 0; z < rowsZ; z++)
            {
                var tile = tiles[x,z];
                if (tile == null) continue;
                if (x > 0 && tiles[x - 1, z] != null) // Left
                    tile.neighbourTiles.Add(tiles[x - 1, z]);
                if (x < rowsX-1 && tiles[x + 1, z] != null) // Right
                    tile.neighbourTiles.Add(tiles[x + 1, z]);

                if (z > 0)
                {
                    if(tiles[x, z - 1] != null)
                        tile.neighbourTiles.Add(tiles[x, z - 1]);// Bottom
                    if(x > 0 && tiles[x - 1, z - 1] != null)
                        tile.neighbourTiles.Add(tiles[x - 1, z - 1]); // Bottom Left
                    if(x < rowsX-1 && tiles[x + 1, z - 1] != null)
                        tile.neighbourTiles.Add(tiles[x + 1, z - 1]); // Bottom Right
                }
                        
                    
                if (z < rowsZ - 1) 
                {
                    if(tiles[x, z + 1] != null)
                        tile.neighbourTiles.Add(tiles[x, z + 1]);// Top
                    if(x > 0 && tiles[x - 1, z + 1] != null)
                        tile.neighbourTiles.Add(tiles[x - 1, z + 1]); // Top Left
                    if(x < rowsX-1 && tiles[x + 1, z + 1] != null)
                        tile.neighbourTiles.Add(tiles[x + 1, z + 1]); // Top Right
                }
            }
        }

        sectorTileGrid = tiles;
    }
    
    public PathFinderSectorTile GetNearestNode(Vector3 position)
    {
        PathFinderSectorTile nearestNode = null;
        var nearestNodeDist = 0f;
        
        if (sectorTiles == null || sectorTiles.Count <= 0) return null;
        
        foreach (var tile in sectorTiles)
        {
            var dist = Vector3.Distance(tile.position, position);
            
            if (!(dist < nearestNodeDist) && nearestNode != null) continue;
            
            nearestNode = tile;
            nearestNodeDist = dist;
        }

        return nearestNode;
    }

    public bool ContainsPoint(Vector3 point)
    {
        return bounds.Contains(point);
    }

    private void OnDrawGizmos()
    {
        if(!doDebug)
            return;
        
        Gizmos.color = debugSectorColor;
        Gizmos.DrawCube(bounds.center, bounds.size);

        if (debugDrawTiles && sectorTiles != null && sectorTiles.Count > 0)
        {
            Vector3 tileSize = new Vector3(sectorTileSize, .5f, sectorTileSize);
            foreach (var tile in sectorTiles)
            {
                if(tile == null)continue;
                Gizmos.color = new Color32(0,200,0,100);
                Gizmos.DrawCube(tile.position, tileSize);

                if (tile.neighbourTiles.Count > 0)
                {
                    foreach (var neighbour in tile.neighbourTiles)
                    {
                        if(neighbour == null)continue;
                        Gizmos.color = Color.black;
                        Gizmos.DrawLine(tile.position, neighbour.position);
                    }
                }
            }
        }
    }
}
