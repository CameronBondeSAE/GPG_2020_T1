using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Pathfinding;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class PathFinderController : MonoBehaviour
{
    public bool goToNearestPossiblePos = true;
    //private PathFinderSector[,] sectors;
    
    public List<PathFinderSector> sectors;
    
    [Button("Load All Sectors In Scene")]
    public void LoadAllSectorsInScene(bool reloadExisting = true)
    {
        var sectorsInScene = FindObjectsOfType<PathFinderSector>();

        // Load all sectors
        sectorsInScene.ForEach(s => StartCoroutine(LoadSector(s, reloadExisting, false)));
        // Connect all sectors
        sectorsInScene.ForEach(s => StartCoroutine(ConnectSector(s)));
    }
    
    // TODO SUPER HACKY & POTENTIALLY BUGGY DEBUG SYSTEM
    [FoldoutGroup("Debug")]
    public bool debugPathGeneration = false;
    [FoldoutGroup("Debug"), ShowInInspector]
    public List<PathFinderSectorTile> debugPathList = new List<PathFinderSectorTile>();
    [FoldoutGroup("Debug")]
    public float debugPathGenerationDelay = 0.0001f; // seconds
    [FoldoutGroup("Debug")]
    public float debugPathGenerationDataDeletionDelay = 3f;
    [FoldoutGroup("Debug")]
    public Color32 debugStartColor = new Color32(0,0,255,200);
    [FoldoutGroup("Debug")]
    public Color32 debugEndColor = new Color32(255,200,50,200);
    [FoldoutGroup("Debug")]
    public Color32 debugGreenColor = new Color32(50,255,50,200);
    [FoldoutGroup("Debug")]
    public Color32 debugRedColor = new Color32(255,0,0,200);
    [FoldoutGroup("Debug")]
    public Color32 debugPathColor = new Color32(0,0,0,255);
    
    [FoldoutGroup("Debug")]
    public readonly Vector3 debugEndPointSize = new Vector3(0.4f, 2.5f, 0.4f);
    [FoldoutGroup("Debug")]
    public readonly Vector3 debugWaypointPointSize = new Vector3(0.4f, 1.5f, 0.4f);
    
    [FoldoutGroup("Debug")]
    public GameObject testStartPos;
    [FoldoutGroup("Debug")]
    public GameObject testEndPos;
    [FoldoutGroup("Debug")]
    public bool calculateFlowField;

    [FoldoutGroup("Debug"), ShowInInspector]
    public List<PathFinderSectorTile> waypointList;
    [FoldoutGroup("Debug"), Button("Find Path"), DisableInEditorMode]
    public void TestFindPath()
    {
        if (testStartPos == null || testEndPos == null)
            return;

        Action<List<PathFinderSectorTile>> onDoneFunc = list =>
        {
            Debug.Log("Done calculating path. " + list?.Count);
            if (calculateFlowField)
            {
                StartCoroutine(FindFlowFieldPath(list));
            }
            waypointList = list;
        };
        StartCoroutine(FindPath(testStartPos.transform.position, testEndPos.transform.position, onDoneFunc));
    }
    [FoldoutGroup("Debug"), Button("Test"), DisableInEditorMode]
    public void Test()
    {
        var tOrig = new PathFinderSectorTile();
        tOrig.position = transform.position;
        tOrig.sector = sectors[0];

        var tClone = (PathFinderSectorTile)tOrig.Clone();
        
        var list1 = new List<PathFinderSectorTile>(){tOrig};
        
        
        Debug.Log(list1.IndexOf(tOrig)+" <> "+list1.IndexOf(tClone)+" ;; "+tOrig.Equals(tClone));
    }

    private IEnumerator LoadSector(PathFinderSector sector, bool reload = false, bool connectSector = false)
    {
        if (sectors.Contains(sector) && !reload) // TODO Maybe Reload sector? Recalculate?
            yield break;
        sectors.Remove(sector);
        
        sector.CreateSectorTileGrid();
        if(connectSector)
            ConnectSector(sector);
        
        
        sectors.Add(sector);
        yield return 0;
    }

    private IEnumerator ConnectSector(PathFinderSector sector)
    {
        var connectingSectors = GetConnectingSectors(sector);
        //Debug.Log("Found "+connectingSectors.Count+" connecting sectors");
        /*foreach (var conSec in connectingSectors)
        {
            if(conSec == null || conSec.borderTiles == null || conSec.borderTiles.Count == 0)
                return;
        }*/
        foreach (var borderTileObject in sector.borderTileObjects)
        {
            if(borderTileObject.Tile == null)
                continue;
            
            borderTileObject.Tile.neighbourTiles.AddRange(borderTileObject.GetNeighbourTileObjects(true).Select(obj => obj.Tile).ToList());
            //Debug.Log("border tile test. tileRect Pos: "+borderTile.tileRect.position+" ;; tileRect Center: "+borderTile.tileRect.center+" ;; tileRect size: "+borderTile.tileRect.size);
            //var hits = Physics.OverlapBox(borderTile.tileRect.position, borderTile.tileRect.size * .5f);

        }
        yield return 0;
    }

    private List<PathFinderSector> GetConnectingSectors(PathFinderSector sector)
    {
        var hits = Physics.OverlapBox(sector.bounds.center, sector.bounds.extents);

        var connectingSectors = hits.Where(col =>
        {
            var pfs = col.GetComponent<PathFinderSector>();
            return pfs != null && pfs != sector;
        }).Select(col => col.GetComponent<PathFinderSector>()).ToList();
        
        return connectingSectors;
    }
    
    public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, Action<List<PathFinderSectorTile>> onDoneCallback = null)
    {
        var clonedTiles = new List<PathFinderSectorTile>();
        var tmpDebugPathList = new List<PathFinderSectorTile>();
        var startTile = (PathFinderSectorTile)GetNearestNode(startPos)?.Clone();
        clonedTiles.Add(startTile);
        
        var endTile = GetNearestNode(targetPos);

        if (startTile == null || endTile == null)
            yield break;
        
        startTile.GCost = 0;
        startTile.HCost = PathFinderSectorTile.CalculateHCost(startTile, endTile);
        
        if (debugPathGeneration)
        {
            startTile.isStart = true;
            startTile.isGreen = true;
            debugPathList.Add(startTile);
            tmpDebugPathList.Add(startTile);
        }
        
        var greenTiles = new List<PathFinderSectorTile>(){startTile};
        var redTiles = new List<PathFinderSectorTile>();
        
        PathFinderSectorTile finalEndNode = null;
        
        Debug.Log("PathFinder: Start while...");
        while (true) // TODO Add security break!
        {
            var currentTile = GetLowestFCostTile(greenTiles);
            
            if(currentTile == null)
                break;

            greenTiles.Remove(currentTile);
            if (debugPathGeneration)
                currentTile.isGreen = false;
            redTiles.Add(currentTile);
            if (debugPathGeneration)
                currentTile.isRed = true;
            /*

            if (debugPathGeneration)
                yield return new WaitForSeconds(debugPathGenerationDelay);*/

            if (currentTile.position.Equals(endTile.position) || currentTile.neighbourTiles == null || currentTile.neighbourTiles.Count == 0)
            {
                if (goToNearestPossiblePos || currentTile.position.Equals(endTile.position))
                {
                    finalEndNode = currentTile;
                    if (debugPathGeneration)
                        currentTile.isEnd = true;
                }
                break;
            }

            // Clone neighbour tiles
            //currentTile.neighbourTiles = currentTile.neighbourTiles.Select(nt => (PathFinderSectorTile)nt.Clone()).ToList();
            var newNeighbourTilesList = new List<PathFinderSectorTile>();
            currentTile.neighbourTiles.ForEach(nt =>
            {
                if (nt == null)
                    return;
                
                var clonedIndex = clonedTiles.IndexOf(nt);
                if (clonedIndex >= 0 )
                {
                    newNeighbourTilesList.Add(clonedTiles[clonedIndex]);
                }
                else
                {
                    var ntClone = (PathFinderSectorTile) nt.Clone();
                    clonedTiles.Add(ntClone);
                    newNeighbourTilesList.Add(ntClone);
                }
            });
            currentTile.neighbourTiles = newNeighbourTilesList;
            
            
            foreach (var neighbourTile in currentTile.neighbourTiles)
            {
                if(neighbourTile == null || redTiles.Contains(neighbourTile))
                    continue;

                if (debugPathGeneration && !debugPathList.Contains(neighbourTile))
                {
                    debugPathList.Add(neighbourTile);
                    tmpDebugPathList.Add(neighbourTile);
                }

                var neighbourGCost = Vector3.Distance(neighbourTile.position, currentTile.position) +
                                     currentTile.GCost;
                var neighbourHCost = PathFinderSectorTile.CalculateHCost(neighbourTile, endTile);
                var neighbourFCost = neighbourGCost + neighbourHCost;

                if ((neighbourTile.fCost >= 0 && neighbourFCost < neighbourTile.fCost) ||
                    !greenTiles.Contains(neighbourTile))
                {
                    neighbourTile.GCost = neighbourGCost;
                    neighbourTile.HCost = neighbourHCost;
                    neighbourTile.lastNode = currentTile;
                    
                    if (!greenTiles.Contains(neighbourTile))
                    {
                        greenTiles.Add(neighbourTile);
                        if (debugPathGeneration)
                            neighbourTile.isGreen = true;
                    }
                }
                
                /*////////////
                if ((!(neighbourTile.fCost >= 0) || !(neighbourFCost < neighbourTile.fCost)) &&
                    greenTiles.Contains(neighbourTile)) continue;
                
                neighbourTile.GCost = neighbourGCost;
                neighbourTile.HCost = neighbourHCost;
                neighbourTile.lastNode = currentTile;

                if (greenTiles.Contains(neighbourTile)) continue;
                
                greenTiles.Add(neighbourTile);
                if (debugPathGeneration)
                    neighbourTile.isGreen = true;*/
                
                /*if (debugPathGeneration)
                    yield return new WaitForSeconds(debugPathGenerationDelay);*/

                 //yield return 0;
            }
            if (debugPathGeneration && debugPathGenerationDelay > 0)
                yield return new WaitForSeconds(debugPathGenerationDelay);
        }
        
        Debug.Log("FindPath: Done w/ While Loop");

        var waypointList = CreateWaypointsListFromTile(finalEndNode);
        onDoneCallback?.Invoke(waypointList);

        if (debugPathGeneration)
        {
            waypointList?.ForEach(wp => { wp.isPath = true; });
            
            yield return new WaitForSeconds(debugPathGenerationDataDeletionDelay);
            debugPathList = debugPathList.Except(tmpDebugPathList).ToList(); // TODO SUPER HACKY & POTENTIALLY BUGGY
        }
        
        Debug.Log("FindPath: End");

        yield return 0;
    }

    public Color32[] colors = new Color32[15];
    public IEnumerator FindFlowFieldPath(List<PathFinderSectorTile> tilePath)
    {
        //tilePath.Reverse(); // Have last node first
        var flowFields = new Dictionary<PathFinderSector, PathFinderFlowField>();
        Debug.Log("Start Calc Flow Field");
        for (int i = 0; i < tilePath.Count; i++)
        {
            var currentTile = tilePath[i];
            if(currentTile == null || currentTile.sector == null)
                continue;
            
            if (i == tilePath.Count-1 || tilePath[i+1].sector != currentTile.sector)
            {
                PathFinderFlowField secFlowField;

                if (!flowFields.ContainsKey(currentTile.sector))
                {
                    secFlowField = (PathFinderFlowField) currentTile.sector.pathFinderFlowFieldTemplate?.Clone();

                    if (secFlowField == null)
                    {
                        var rowsX = (int)(currentTile.sector.bounds.size.x / currentTile.sector.sectorTileSize);
                        var rowsZ = (int)(currentTile.sector.bounds.size.z / currentTile.sector.sectorTileSize);
                        secFlowField = new PathFinderFlowField(currentTile.sector.sectorTileSize, rowsX, rowsZ);
                    }
                    flowFields.Add(currentTile.sector, secFlowField);
                }
                else
                {
                    secFlowField = flowFields[currentTile.sector];
                }

                secFlowField.colors = colors;
                secFlowField.targetPosition = currentTile.position;
                Debug.Log("Start Gen HeatMap");
                secFlowField.GenerateHeatmap(currentTile, currentTile.sector);
                //StartCoroutine(secFlowField.GenerateHeatmap(currentTile, currentTile.sector));
                //yield return new WaitForSeconds(1); // TODO SUPER UGLY HACK;; Need to wait until the previous coroutine is done, then execute next coroutine.
                //Debug.Log("Start Gen VecField");
                Debug.DrawRay(currentTile.position, Vector3.up*5, Color.cyan,30f);
                StartCoroutine(GenerateVectorField(currentTile, currentTile.sector));
                //secFlowField.GenerateVectorField(currentTile, currentTile.sector);
                
            }
        }
        StartCoroutine(DrawFlowFieldTEMPTEST(tilePath[0], tilePath[0].sector));
        
        Debug.Log("Done with function...");
        
        yield return 0;
    }

    public IEnumerator DrawFlowFieldTEMPTEST(PathFinderSectorTile currentTile, PathFinderSector sector)
    {
        var rayStartPos = currentTile.position;
        rayStartPos.y += 0.2f;
        //Debug.Log("GEN VEC FIELD Pos: "+currentTile.position+" Vector: "+dirVector+" neighbours: "+currentTile.neighbourTiles.Count);
        Debug.DrawRay(currentTile.position, Vector3.up * currentTile.flowFieldDistanceToTarget, colors[Mathf.Clamp((int)currentTile.flowFieldDistanceToTarget,0,14)], 30f);
        Debug.DrawRay(rayStartPos-(currentTile.flowFieldDirection/2), currentTile.flowFieldDirection, Color.red, 30f);
        Debug.DrawRay(rayStartPos+(currentTile.flowFieldDirection/2*.3f),currentTile.flowFieldDirection*.3f, Color.yellow, 30f);
        currentTile.vecDirDrawed = true;
        yield return new WaitForEndOfFrame();
        currentTile.neighbourTiles?.ForEach(tile =>
        {
            if (tile == null || (sector != null && tile.sector != sector) || tile.vecDirDrawed) return;
            var generateVectorField = DrawFlowFieldTEMPTEST(tile, sector);
            while(generateVectorField.MoveNext())
            {
            }
        });

        yield return 0;
    }
    
    public IEnumerator GenerateVectorField(PathFinderSectorTile currentTile, PathFinderSector sector)
    {
        if (currentTile == null || !currentTile.flowFieldDirection.Equals(Vector3.negativeInfinity))
            yield break;
            
        var dirVector = new Vector3((currentTile.GetLeftTile()?.flowFieldDistanceToTarget ?? currentTile.flowFieldDistanceToTarget) - (currentTile.GetRightTile()?.flowFieldDistanceToTarget ?? currentTile.flowFieldDistanceToTarget), 
                0, 
                (currentTile.GetBottomTile()?.flowFieldDistanceToTarget ?? currentTile.flowFieldDistanceToTarget) - (currentTile.GetTopTile()?.flowFieldDistanceToTarget ?? currentTile.flowFieldDistanceToTarget));
        currentTile.flowFieldDirection = dirVector.normalized;

        
        
        if (Mathf.Approximately(currentTile.flowFieldDistanceToTarget, 0))
        {
            Debug.Log("@TARGET : "+currentTile.flowFieldDirection+" ;; "+dirVector+" ;; "+dirVector.normalized);
            Debug.Log("LEFT: "+currentTile.GetLeftTile()?.flowFieldDistanceToTarget+
                      " ;;right: "+currentTile.GetRightTile()?.flowFieldDistanceToTarget+
                      " ;;top: "+currentTile.GetTopTile()?.flowFieldDistanceToTarget+
                      " ;;bottom: "+currentTile.GetBottomTile()?.flowFieldDistanceToTarget);
            
            if(currentTile.GetLeftTile() != null)
                Debug.DrawRay(currentTile.GetLeftTile().position, Vector3.up * 5, Color.red, 30f);
            if(currentTile.GetRightTile() != null)
                Debug.DrawRay(currentTile.GetRightTile().position, Vector3.up * 5, Color.red, 30f);
            if(currentTile.GetTopTile() != null)
                Debug.DrawRay(currentTile.GetTopTile().position, Vector3.up * 5, Color.blue, 30f);
            if(currentTile.GetBottomTile() != null)
                Debug.DrawRay(currentTile.GetBottomTile().position, Vector3.up * 5, Color.blue, 30f);
        }
            
        /*var rayStartPos = currentTile.position;
        rayStartPos.y += 0.2f;
        //Debug.Log("GEN VEC FIELD Pos: "+currentTile.position+" Vector: "+dirVector+" neighbours: "+currentTile.neighbourTiles.Count);
        Debug.DrawRay(rayStartPos-(currentTile.flowFieldDirection/2), currentTile.flowFieldDirection, Color.red, 30f);
        Debug.DrawRay(rayStartPos+(currentTile.flowFieldDirection/2*.3f),currentTile.flowFieldDirection*.3f, Color.yellow, 30f);
        yield return new WaitForEndOfFrame();*/
        currentTile.neighbourTiles?.ForEach(tile =>
        {
            //Debug.Log("Wus.. "+ (tile == null)+" - "+(sector != null && tile.sector != sector)+" - "+(currentTile.flowFieldDirection != Vector3.zero)+" - "+(tile.Equals(currentTile)));
            if (tile == null || (sector != null && tile.sector != sector) || !tile.flowFieldDirection.Equals(Vector3.negativeInfinity)) return;
            var generateVectorField = GenerateVectorField(tile, sector);
            while(generateVectorField.MoveNext())
            {
            }
        });
            
        yield return null;
    }
    
    private static List<PathFinderSectorTile> CreateWaypointsListFromTile(PathFinderSectorTile endTile)
    {
        var waypoints = new List<PathFinderSectorTile>();
        if (endTile == null)
            return waypoints;

        var currentTile = endTile;
        while (currentTile.lastNode != null)
        {
            waypoints.Add(currentTile); // .position
            currentTile = currentTile.lastNode;
        }

        waypoints.Reverse();

        return waypoints;
    }

    public PathFinderSector GetSectorByPoint(Vector3 point)
    {
        return sectors.FirstOrDefault(s => s.ContainsPoint(point));
    }
    
    public PathFinderSectorTile GetNearestNode(Vector3 position)
    {
        var sector = GetSectorByPoint(position);
        
        if (sector == null) return null;
        
        PathFinderSectorTile nearestNode = null;
        var nearestNodeDist = 0f;
        
        if (sector.sectorTiles == null || sector.sectorTiles.Count <= 0) return null;
        
        foreach (var tile in sector.sectorTiles)
        {
            var dist = Vector3.Distance(tile.position, position);
            
            if (!(dist < nearestNodeDist) && nearestNode != null) continue;
            
            nearestNode = tile;
            nearestNodeDist = dist;
        }

        return nearestNode;
    }

    private PathFinderSectorTile GetLowestFCostTile(List<PathFinderSectorTile> tiles)
    {
        PathFinderSectorTile lowestCostTile = null;
        foreach (var tile in tiles)
        {
            if (lowestCostTile == null || tile.fCost < lowestCostTile.fCost)
            {
                lowestCostTile = tile;
            }   
        }

        return lowestCostTile;
    }

    private void OnDrawGizmos()
    {
        if(!debugPathGeneration || debugPathList == null || debugPathList.Count == 0)
            return;

        
        foreach (var tile in debugPathList)
        {
            Gizmos.color = tile.isStart ? debugStartColor : tile.isEnd ? debugEndColor : tile.isPath ? debugPathColor : tile.isRed ? debugRedColor : tile.isGreen ? debugGreenColor : new Color32(0,0,0,0);
            
            Gizmos.DrawCube(tile.position, tile.isStart || tile.isEnd ? debugEndPointSize : debugWaypointPointSize);
        }
    }
    
    
    /*public void CreateSectors()
    {
        var walkableObjects = GetWalkableGameObjects();
    }

    private List<GameObject> GetWalkableGameObjects()
    {
        var gameObjects = FindObjectsOfType<GameObject>();
        var walkableObjects = new List<GameObject>();

        for (var i = 0; i < gameObjects.Length; i++)
        {
            if (((1 << gameObjects[i].layer) & walkableLayers) != 0)
            {
                walkableObjects.Add(gameObjects[i]);
            }
        }

        return walkableObjects;
    }*/
}
