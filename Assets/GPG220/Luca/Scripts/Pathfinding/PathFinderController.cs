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
    public List<PathFinderSectorTileData> debugPathList = new List<PathFinderSectorTileData>();
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
    public List<PathFinderSectorTileData> waypointList;
    [FoldoutGroup("Debug"), Button("Find Path"), DisableInEditorMode]
    public void TestFindPath()
    {
        if (testStartPos == null || testEndPos == null)
            return;

        /*Action<List<PathFinderSectorTileData>> onDoneFunc = list =>
        {
            Debug.Log("Done calculating path. " + list?.Count);
            if (calculateFlowField)
            {
                StartCoroutine(FindFlowFieldPath(list));
            }
            waypointList = list;
        };*/
        Action<PathFinderPath> onDoneFunc = path =>
        {
            Debug.Log("Done calculating path. " + path.tilePath?.Count);
            if (calculateFlowField)
            {
                StartCoroutine(FindFlowFieldPath(path));
            }
            waypointList = path.tilePath;
        };
        StartCoroutine(FindPath(testStartPos.transform.position, testEndPos.transform.position, onDoneFunc));
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
        foreach (var borderTileObject in sector.borderTileObjects)
        {
            if(borderTileObject.Tile == null)
                continue;
            
            borderTileObject.Tile.neighbourTiles.AddRange(borderTileObject.GetNeighbourTileObjects(true).Select(obj => obj.Tile).ToList());
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
    
    public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, Action<PathFinderPath> onDoneCallback = null)
    {
        var path = new PathFinderPath();
        
        var tmpDebugPathList = new List<PathFinderSectorTileData>();

        var startTileData = new PathFinderSectorTileData(GetNearestNode(startPos));
        
        var targetTile = GetNearestNode(targetPos);

        if (startTileData.tile == null || targetTile == null)
            yield break;
        
        startTileData.GCost = 0;
        startTileData.HCost = PathFinderSectorTileData.CalculateHCost(startTileData.tile, targetTile);
        
        if (debugPathGeneration)
        {
            startTileData.isStart = true;
            startTileData.isGreen = true;
            debugPathList.Add(startTileData);
            tmpDebugPathList.Add(startTileData);
        }
        
        var greenTilesData = new List<PathFinderSectorTileData>(){startTileData};
        var redTilesData = new List<PathFinderSectorTileData>();
        
        PathFinderSectorTileData targetTileData = null;
        
        while (true) // TODO Add security break!
        {
            var currentTileData = GetLowestFCostTile(greenTilesData);
            
            if(currentTileData == null)
                break;

            greenTilesData.Remove(currentTileData);
            if (debugPathGeneration)
                currentTileData.isGreen = false;
            redTilesData.Add(currentTileData);
            if (debugPathGeneration)
                currentTileData.isRed = true;

            if (currentTileData.tile.Equals(targetTile) || currentTileData.tile.neighbourTiles == null || currentTileData.tile.neighbourTiles.Count == 0)
            {
                if (goToNearestPossiblePos || currentTileData.tile.Equals(targetTile))
                {
                    targetTileData = currentTileData;
                    if (debugPathGeneration)
                        currentTileData.isEnd = true;
                }
                break;
            }
            
            foreach (var neighbourTile in currentTileData.tile.neighbourTiles)
            {
                if(neighbourTile == null) continue;
                
                path.tileData.TryGetValue(neighbourTile, out var neighbourTileData);
                if(neighbourTileData == null)
                    neighbourTileData = new PathFinderSectorTileData(neighbourTile);
                
                if(redTilesData.Contains(neighbourTileData)) continue;

                if (debugPathGeneration && !debugPathList.Contains(neighbourTileData))
                {
                    debugPathList.Add(neighbourTileData);
                    tmpDebugPathList.Add(neighbourTileData);
                }

                var neighbourGCost = Vector3.Distance(neighbourTile.position, currentTileData.tile.position) +
                                     currentTileData.GCost;
                var neighbourHCost = PathFinderSectorTileData.CalculateHCost(neighbourTile, targetTile);
                var neighbourFCost = neighbourGCost + neighbourHCost;

                if ((neighbourTileData.fCost >= 0 && neighbourFCost < neighbourTileData.fCost) ||
                    !greenTilesData.Contains(neighbourTileData))
                {
                    neighbourTileData.GCost = neighbourGCost;
                    neighbourTileData.HCost = neighbourHCost;
                    neighbourTileData.lastTile = currentTileData.tile;
                    neighbourTileData.lastTileData = currentTileData;
                    
                    if (!greenTilesData.Contains(neighbourTileData))
                    {
                        greenTilesData.Add(neighbourTileData);
                        if (debugPathGeneration)
                            neighbourTileData.isGreen = true;
                    }
                }
            }
            if (debugPathGeneration && debugPathGenerationDelay > 0)
                yield return new WaitForSeconds(debugPathGenerationDelay);
        }

        var waypointDataList = CreateWaypointsListFromTileData(targetTileData);
        path.tilePath = waypointDataList;
        onDoneCallback?.Invoke(path);

        if (debugPathGeneration)
        {
            waypointDataList?.ForEach(wp => { wp.isPath = true; });
            
            yield return new WaitForSeconds(debugPathGenerationDataDeletionDelay);
            debugPathList = debugPathList.Except(tmpDebugPathList).ToList(); // TODO SUPER HACKY & POTENTIALLY BUGGY
        }

        yield return 0;
    }

    public Color32[] colors = new Color32[15];
    public IEnumerator FindFlowFieldPath(PathFinderPath path)
    {
        var flowFields = new Dictionary<PathFinderSector, PathFinderFlowField>();
        
        for (int i = 0; i < path.tilePath.Count; i++)
        {
            var currentTileData = path.tilePath[i];
            if(currentTileData == null || currentTileData.tile.sector == null)
                continue;
            
            if (i == path.tilePath.Count-1 || path.tilePath[i+1].tile.sector != currentTileData.tile.sector)
            {
                PathFinderFlowField secFlowField;

                if (!flowFields.ContainsKey(currentTileData.tile.sector))
                {
                    secFlowField = (PathFinderFlowField) currentTileData.tile.sector.pathFinderFlowFieldTemplate?.Clone();

                    if (secFlowField == null)
                    {
                        var rowsX = (int)(currentTileData.tile.sector.bounds.size.x / currentTileData.tile.sector.sectorTileSize);
                        var rowsZ = (int)(currentTileData.tile.sector.bounds.size.z / currentTileData.tile.sector.sectorTileSize);
                        secFlowField = new PathFinderFlowField(currentTileData.tile.sector.sectorTileSize, rowsX, rowsZ);
                    }
                    flowFields.Add(currentTileData.tile.sector, secFlowField);
                }
                else
                {
                    secFlowField = flowFields[currentTileData.tile.sector];
                }

                secFlowField.colors = colors;
                secFlowField.targetPosition = currentTileData.tile.position;
                Debug.Log("Start Gen HeatMap");
                secFlowField.GenerateHeatmap(currentTileData, currentTileData.tile.sector, path);
                //StartCoroutine(secFlowField.GenerateHeatmap(currentTile, currentTile.sector));
                //yield return new WaitForSeconds(1); // TODO SUPER UGLY HACK;; Need to wait until the previous coroutine is done, then execute next coroutine.
                //Debug.Log("Start Gen VecField");
                Debug.DrawRay(currentTileData.GetPosition(), Vector3.up*5, Color.cyan,30f);
                StartCoroutine(GenerateVectorField(currentTileData, currentTileData.tile.sector, path));
                //secFlowField.GenerateVectorField(currentTile, currentTile.sector);
                
            }
        }
        StartCoroutine(DrawFlowFieldTEMPTEST(path.tilePath[0], path.tilePath[0].tile.sector, path));
        
        Debug.Log("Done with function...");
        
        yield return 0;
    }

    public IEnumerator DrawFlowFieldTEMPTEST(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path)
    {
        var rayStartPos = currentTileData.GetPosition();
        rayStartPos.y += 0.2f;
        //Debug.Log("GEN VEC FIELD Pos: "+currentTile.position+" Vector: "+dirVector+" neighbours: "+currentTile.neighbourTiles.Count);
        Debug.DrawRay(currentTileData.GetPosition(), Vector3.up * currentTileData.flowFieldDistanceToTarget, colors[Mathf.Clamp((int)currentTileData.flowFieldDistanceToTarget,0,14)], 30f);
        Debug.DrawRay(rayStartPos-(currentTileData.flowFieldDirection/2), currentTileData.flowFieldDirection, Color.red, 30f);
        Debug.DrawRay(rayStartPos+(currentTileData.flowFieldDirection/2*.3f),currentTileData.flowFieldDirection*.3f, Color.yellow, 30f);
        currentTileData.vecDirDrawed = true;
        yield return new WaitForEndOfFrame();
        currentTileData.tile.neighbourTiles?.ForEach(neighbourTile =>
        {
            if (neighbourTile == null || (sector != null && neighbourTile.sector != sector)) return;
            path.tileData.TryGetValue(neighbourTile, out var neighbourTileData);
            if(neighbourTileData == null || neighbourTileData.vecDirDrawed) return;
            
            var generateVectorField = DrawFlowFieldTEMPTEST(neighbourTileData, sector, path);
            while(generateVectorField.MoveNext())
            {
            }
        });

        yield return 0;
    }
    
    public IEnumerator GenerateVectorField(PathFinderSectorTileData currentTileData, PathFinderSector sector, PathFinderPath path)
    {
        if (currentTileData == null || !currentTileData.flowFieldDirection.Equals(Vector3.negativeInfinity))
            yield break;

        // Calculate Flow Field Direction
        PathFinderSectorTileData leftTileData = null, rightTileData = null, topTileData = null, bottomTileData = null;
        PathFinderSectorTile leftTile = currentTileData.tile.GetLeftTile(),
            rightTile = currentTileData.tile.GetRightTile(),
            topTile = currentTileData.tile.GetTopTile(),
            bottomTile = currentTileData.tile.GetBottomTile();
        if(leftTile != null) path.tileData.TryGetValue(leftTile, out leftTileData);
        if(rightTile != null) path.tileData.TryGetValue(rightTile, out rightTileData);
        if(topTile != null) path.tileData.TryGetValue(topTile, out topTileData);
        if(bottomTile != null) path.tileData.TryGetValue(bottomTile, out bottomTileData);
        
        var dirVector = new Vector3((leftTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget) - (rightTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget), 
                0, 
                (bottomTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget) - (topTileData?.flowFieldDistanceToTarget ?? currentTileData.flowFieldDistanceToTarget));
        currentTileData.flowFieldDirection = dirVector.normalized;

        
        /*// Debug Stuff
        if (debugPathGeneration && Mathf.Approximately(currentTileData.flowFieldDistanceToTarget, 0))
        {
            Debug.Log("@TARGET : "+currentTileData.flowFieldDirection+" ;; "+dirVector+" ;; "+dirVector.normalized);
            Debug.Log("LEFT: "+leftTileData?.flowFieldDistanceToTarget+
                      " ;;right: "+rightTileData?.flowFieldDistanceToTarget+
                      " ;;top: "+topTileData?.flowFieldDistanceToTarget+
                      " ;;bottom: "+bottomTileData?.flowFieldDistanceToTarget);
            
            if(leftTileData != null)
                Debug.DrawRay(leftTileData.GetPosition(), Vector3.up * 5, Color.red, 30f);
            if(rightTileData != null)
                Debug.DrawRay(rightTileData.GetPosition(), Vector3.up * 5, Color.red, 30f);
            if(topTileData != null)
                Debug.DrawRay(topTileData.GetPosition(), Vector3.up * 5, Color.blue, 30f);
            if(bottomTileData != null)
                Debug.DrawRay(bottomTileData.GetPosition(), Vector3.up * 5, Color.blue, 30f);
        }*/
            
        currentTileData.tile.neighbourTiles?.ForEach(neighbourTile =>
        {
            if (neighbourTile == null || (sector != null && neighbourTile.sector != sector)) return;
            path.tileData.TryGetValue(neighbourTile, out var neighbourTileData);
            if (neighbourTileData == null || !neighbourTileData.flowFieldDirection.Equals(Vector3.negativeInfinity)) return;
            
            var generateVectorField = GenerateVectorField(neighbourTileData, sector, path);
            while(generateVectorField.MoveNext())
            {
            }
        });
            
        yield return null;
    }
    
    private static List<PathFinderSectorTileData> CreateWaypointsListFromTileData(PathFinderSectorTileData targetTileData)
    {
        var waypoints = new List<PathFinderSectorTileData>();
        if (targetTileData == null)
            return waypoints;

        var currentTileData = targetTileData;
        while (currentTileData.lastTile != null)
        {
            waypoints.Add(currentTileData); // .position
            currentTileData = currentTileData.lastTileData;
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

    private static PathFinderSectorTileData GetLowestFCostTile(IEnumerable<PathFinderSectorTileData> tileDataList)
    {
        PathFinderSectorTileData lowestCostTileData = null;
        foreach (var tileData in tileDataList)
        {
            if (lowestCostTileData == null || tileData.fCost < lowestCostTileData.fCost)
            {
                lowestCostTileData = tileData;
            }   
        }

        return lowestCostTileData;
    }

    private void OnDrawGizmos()
    {
        if(!debugPathGeneration || debugPathList == null || debugPathList.Count == 0)
            return;

        
        foreach (var tileData in debugPathList)
        {
            Gizmos.color = tileData.isStart ? debugStartColor : tileData.isEnd ? debugEndColor : tileData.isPath ? debugPathColor : tileData.isRed ? debugRedColor : tileData.isGreen ? debugGreenColor : new Color32(0,0,0,0);
            
            Gizmos.DrawCube(tileData.GetPosition(), tileData.isStart || tileData.isEnd ? debugEndPointSize : debugWaypointPointSize);
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
